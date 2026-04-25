using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace DecaySchemeApp
{
    public partial class AppUI : Form
    {
        private List<GateFile> _gateFiles = new List<GateFile>();
        private List<object> _gatePeakRows = new List<object>();
        private List<object> _matchRows = new List<object>();

        public AppUI()
        {
            InitializeComponent();

            txtEnergy.PlaceholderText = "Eγ (keV)";
            txtTolerance.PlaceholderText = "Tol. (±keV)";

            dgvGatePeaks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cmbGateEnergies.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGateEnergies.MaxDropDownItems = 10;
            cmbGateEnergies.DrawMode = DrawMode.OwnerDrawFixed;
            cmbGateEnergies.DrawItem += cmbGateEnergies_DrawItem;

            dgvGatePeaks.ColumnHeaderMouseClick += dgvGatePeaks_ColumnHeaderMouseClick;
            dgvMatches.ColumnHeaderMouseClick += dgvMatches_ColumnHeaderMouseClick;
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            txtFolderPath.Text = dialog.SelectedPath;

            try
            {
                _gateFiles = CsvLoader.LoadFolder(dialog.SelectedPath);
                lblStatus.Text = $"Loaded {_gateFiles.Count} datasets";
                lblMatch.Text = "Matched file: --";
                lblGateFile.Text = "Coincident photopeaks: --";

                _gatePeakRows.Clear();
                _matchRows.Clear();

                dgvGatePeaks.DataSource = null;
                dgvMatches.DataSource = null;

                PopulateEnergyDropdown();
            }
            catch (Exception ex)
            {
                _gateFiles.Clear();
                _gatePeakRows.Clear();
                _matchRows.Clear();
                cmbGateEnergies.DataSource = null;
                lblStatus.Text = $"Error: {ex.Message}";
                lblMatch.Text = "Matched file: --";
                lblGateFile.Text = "Coincident photopeaks --";
            }
        }

        private void PopulateEnergyDropdown()
        {
            cmbGateEnergies.DataSource = null;

            var energies = _gateFiles
                .OrderBy(g => g.GateEnergy)
                .Select(g => g.GateEnergy.ToString("F3"))
                .ToList();

            energies.Insert(0, "- Select -");

            cmbGateEnergies.DataSource = energies;
            cmbGateEnergies.SelectedIndex = 0;
            txtEnergy.Text = string.Empty;
        }

        private void cmbGateEnergies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGateEnergies.SelectedIndex <= 0)
            {
                txtEnergy.Text = string.Empty;
                return;
            }

            txtEnergy.Text = cmbGateEnergies.SelectedItem.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_gateFiles == null || _gateFiles.Count == 0)
            {
                MessageBox.Show(
                    "No files loaded.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!double.TryParse(txtEnergy.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double searchEnergy))
            {
                lblStatus.Text = "Invalid energy value.";
                return;
            }

            if (!double.TryParse(txtTolerance.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double tolerance))
            {
                lblStatus.Text = "Invalid tolerance value.";
                return;
            }

            lblStatus.Text = $"Loaded {_gateFiles.Count} datasets";
            int minCoinc = (int)nudCoinc.Value;

            var gateFile = SearchEngine.FindGateFile(_gateFiles, searchEnergy, tolerance);
            var matches = SearchEngine.FindMatchingPeaks(_gateFiles, searchEnergy, tolerance);

            double canonicalSearchEnergy = gateFile != null
                ? gateFile.GateEnergy
                : searchEnergy;

            int searchEnergyCoinc = CountCoincidencesForEnergy(canonicalSearchEnergy, tolerance) - 1;

            if (searchEnergyCoinc < 0)
                searchEnergyCoinc = 0;

            if (gateFile != null)
            {
                _gatePeakRows = gateFile.Peaks
                    .OrderBy(p => p.Energy)
                    .Select(p => (object)new GatePeakRow
                    {
                        Eγ = p.Energy,
                        σEγ = p.EnergyError,
                        I = p.Intensity,
                        σI = p.IntensityError
                    })
                    .ToList();

                BindGatePeakRows(_gatePeakRows);

                lblMatch.Text = $"Matched file: {gateFile.FileName}";
                lblGateFile.Text = $"Coincident photopeaks: {gateFile.Peaks.Count}";
                SelectMatchingDropdownEnergy(gateFile.GateEnergy);
            }
            else
            {
                lblMatch.Text = "Matched file: --";
                lblGateFile.Text = "Coincident photopeaks: --";
                _gatePeakRows.Clear();
                dgvGatePeaks.DataSource = null;
            }

            _matchRows = matches
                .Select(m =>
                {
                    int pairCoinc = CountCoincidencesBetweenEnergies(
                        canonicalSearchEnergy,
                        m.Item1.GateEnergy,
                        tolerance);

                    return new MatchRow
                    {
                        GateEγ = m.Item1.GateEnergy,
                        Eγ = m.Item2.Energy,
                        σEγ = m.Item2.EnergyError,
                        I = m.Item2.Intensity,
                        σI = m.Item2.IntensityError,
                        Coincidences = pairCoinc
                    };
                })
                .Where(r => minCoinc == 0 || r.Coincidences >= minCoinc)
                .OrderByDescending(r => r.I)
                .Select(r => (object)r)
                .ToList();

            BindMatchRows(_matchRows);

            lblOccurrences.Text = $"Occurrences in other datasets: {searchEnergyCoinc}";
        }

        private void BindGatePeakRows(IEnumerable<object> rows)
        {
            dgvGatePeaks.DataSource = rows.Cast<GatePeakRow>().ToList();

            dgvGatePeaks.ClearSelection();
            dgvGatePeaks.CurrentCell = null;
            dgvGatePeaks.Refresh();

            if (dgvGatePeaks.Columns.Contains("Eγ"))
                dgvGatePeaks.Columns["Eγ"].DefaultCellStyle.Format = "F3";
            if (dgvGatePeaks.Columns.Contains("σEγ"))
                dgvGatePeaks.Columns["σEγ"].DefaultCellStyle.Format = "F3";
            if (dgvGatePeaks.Columns.Contains("I"))
                dgvGatePeaks.Columns["I"].DefaultCellStyle.Format = "F0";
            if (dgvGatePeaks.Columns.Contains("σI"))
                dgvGatePeaks.Columns["σI"].DefaultCellStyle.Format = "F0";

            HighlightRowsByRelativeIntensity(dgvGatePeaks);
        }

        private void BindMatchRows(IEnumerable<object> rows)
        {
            dgvMatches.DataSource = rows.Cast<MatchRow>().ToList();

            dgvMatches.ClearSelection();
            dgvMatches.CurrentCell = null;
            dgvMatches.Refresh();

            if (dgvMatches.Columns.Contains("GateEγ"))
                dgvMatches.Columns["GateEγ"].DefaultCellStyle.Format = "F3";
            if (dgvMatches.Columns.Contains("Eγ"))
                dgvMatches.Columns["Eγ"].DefaultCellStyle.Format = "F3";
            if (dgvMatches.Columns.Contains("σEγ"))
                dgvMatches.Columns["σEγ"].DefaultCellStyle.Format = "F3";
            if (dgvMatches.Columns.Contains("I"))
                dgvMatches.Columns["I"].DefaultCellStyle.Format = "F0";
            if (dgvMatches.Columns.Contains("σI"))
                dgvMatches.Columns["σI"].DefaultCellStyle.Format = "F0";
            if (dgvMatches.Columns.Contains("Coincidences"))
                dgvMatches.Columns["Coincidences"].DefaultCellStyle.Format = "F0";

            HighlightRowsByRelativeIntensity(dgvMatches);
        }

        private void dgvGatePeaks_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_gatePeakRows.Count == 0)
                return;

            string columnName = dgvGatePeaks.Columns[e.ColumnIndex].Name;

            IEnumerable<GatePeakRow> sortedRows = columnName switch
            {
                "Eγ" => _gatePeakRows.Cast<GatePeakRow>().OrderBy(r => r.Eγ),
                "σEγ" => _gatePeakRows.Cast<GatePeakRow>().OrderBy(r => r.σEγ),
                "I" => _gatePeakRows.Cast<GatePeakRow>().OrderByDescending(r => r.I),
                "σI" => _gatePeakRows.Cast<GatePeakRow>().OrderByDescending(r => r.σI),
                _ => _gatePeakRows.Cast<GatePeakRow>()
            };

            _gatePeakRows = sortedRows.Select(r => (object)r).ToList();
            BindGatePeakRows(_gatePeakRows);
        }

        private void dgvMatches_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_matchRows.Count == 0)
                return;

            string columnName = dgvMatches.Columns[e.ColumnIndex].Name;

            IEnumerable<MatchRow> sortedRows = columnName switch
            {
                "GateEγ" => _matchRows.Cast<MatchRow>().OrderBy(r => r.GateEγ),
                "Eγ" => _matchRows.Cast<MatchRow>().OrderBy(r => r.Eγ),
                "σEγ" => _matchRows.Cast<MatchRow>().OrderBy(r => r.σEγ),
                "I" => _matchRows.Cast<MatchRow>().OrderByDescending(r => r.I),
                "σI" => _matchRows.Cast<MatchRow>().OrderByDescending(r => r.σI),
                "Coincidences" => _matchRows.Cast<MatchRow>().OrderByDescending(r => r.Coincidences),
                _ => _matchRows.Cast<MatchRow>()
            };

            _matchRows = sortedRows.Select(r => (object)r).ToList();
            BindMatchRows(_matchRows);
        }

        private void SelectMatchingDropdownEnergy(double gateEnergy)
        {
            for (int i = 0; i < cmbGateEnergies.Items.Count; i++)
            {
                if (double.TryParse(
                        cmbGateEnergies.Items[i].ToString(),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out double value)
                    && Math.Abs(value - gateEnergy) < 1e-6)
                {
                    cmbGateEnergies.SelectedIndex = i;
                    break;
                }
            }
        }

        private int CountCoincidencesForEnergy(double energy, double tolerance)
        {
            return _gateFiles.Count(g =>
                g.Peaks.Any(p => Math.Abs(p.Energy - energy) <= tolerance));
        }

        private void cmbGateEnergies_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            var combo = (ComboBox)sender;
            string text = combo.Items[e.Index].ToString();

            e.DrawBackground();

            bool isPlaceholder = (e.Index == 0);

            Color textColor = isPlaceholder ? Color.Gray : SystemColors.ControlText;

            StringFormat format = new StringFormat();

            if (isPlaceholder)
                format.Alignment = StringAlignment.Center;
            else
                format.Alignment = StringAlignment.Near;

            format.LineAlignment = StringAlignment.Center;

            using (Brush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(
                    text,
                    combo.Font,
                    brush,
                    e.Bounds,
                    format);
            }

            e.DrawFocusRectangle();
        }

        private int CountCoincidencesBetweenEnergies(double energyA, double energyB, double tolerance)
        {
            return _gateFiles.Count(g =>
                g.Peaks.Any(p => Math.Abs(p.Energy - energyA) <= tolerance) &&
                g.Peaks.Any(p => Math.Abs(p.Energy - energyB) <= tolerance));
        }

        private void dgvMatches_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgvMatches.Rows[e.RowIndex];

            if (row.Cells["GateEγ"].Value != null)
            {
                txtEnergy.Text = Convert.ToString(row.Cells["GateEγ"].Value, CultureInfo.InvariantCulture);
                btnSearch.PerformClick();
            }
        }

        private void btnOccurrenceSummary_Click(object sender, EventArgs e)
        {
            if (_gateFiles == null || _gateFiles.Count == 0)
            {
                MessageBox.Show(
                    "No files loaded.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!double.TryParse(txtTolerance.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double tolerance))
            {
                lblStatus.Text = "Invalid tolerance value";
                return;
            }

            var rows = SearchEngine.BuildCoincidenceSummary(_gateFiles, tolerance);

            using var summaryForm = new CoincidenceSummary(rows, _gateFiles, tolerance);
            summaryForm.ShowDialog(this);
        }

        private void HighlightRowsByRelativeIntensity(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0 || !dgv.Columns.Contains("I"))
                return;

            double maxI = 0.0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow || row.Cells["I"].Value == null)
                    continue;

                double intensity = Convert.ToDouble(row.Cells["I"].Value, CultureInfo.InvariantCulture);

                if (intensity > maxI)
                    maxI = intensity;
            }

            if (maxI <= 0.0)
                return;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow || row.Cells["I"].Value == null)
                    continue;

                double intensity = Convert.ToDouble(row.Cells["I"].Value, CultureInfo.InvariantCulture);
                double iRel = intensity / maxI;

                iRel = Math.Pow(iRel, 0.5);
                iRel = Math.Max(0.0, Math.Min(1.0, iRel));
                int green = 255;
                int redBlue = (int)(255 * (1.0 - iRel));

                row.DefaultCellStyle.BackColor = Color.FromArgb(redBlue, green, redBlue);
            }
        }

        private void lblMatrix_Click(object sender, EventArgs e)
        {
            if (_gateFiles == null || _gateFiles.Count == 0)
            {
                MessageBox.Show(
                    "No files loaded.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtTolerance.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double tolerance))
            {
                lblStatus.Text = "Invalid tolerance value.";
                return;
            }

            using SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Export coincidence matrix";
            saveDialog.Filter = "CSV files (*.csv)|*.csv";
            saveDialog.FileName = "coincidence-matrix.csv";

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                ExportCoincidenceMatrix(saveDialog.FileName, tolerance);

                MessageBox.Show(
                    "Coincidence matrix exported successfully.",
                    "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to export coincidence matrix.\n\n{ex.Message}",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ExportCoincidenceMatrix(string filePath, double tolerance)
        {
            var energies = _gateFiles
                .OrderBy(g => g.GateEnergy)
                .Select(g => g.GateEnergy)
                .ToList();

            var matrix = SearchEngine.BuildCoincidenceCountMatrix(
                _gateFiles,
                tolerance);

            using var writer = new StreamWriter(filePath);

            writer.Write("Eγ");

            foreach (double energy in energies)
                writer.Write($",{energy:F3}");

            writer.WriteLine();

            for (int i = 0; i < energies.Count; i++)
            {
                writer.Write($"{energies[i]:F3}");

                for (int j = 0; j < energies.Count; j++)
                    writer.Write($",{matrix[i][j]}");

                writer.WriteLine();
            }
        }

        private void btnWildcard_Click(object sender, EventArgs e)
        {
            if (_gateFiles == null || _gateFiles.Count == 0)
            {
                MessageBox.Show(
                    "No files loaded.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(
                    txtTolerance.Text,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double tolerance))
            {
                lblStatus.Text = "Invalid tolerance value.";
                return;
            }

            using var form = new WildcardSearch(_gateFiles, tolerance);
            form.ShowDialog(this);
        }

        private void AppUI_Load(object sender, EventArgs e)
        {
        }

        private class GatePeakRow
        {
            public double Eγ { get; set; }
            public double σEγ { get; set; }
            public double I { get; set; }
            public double σI { get; set; }
        }

        private class MatchRow
        {
            public double GateEγ { get; set; }
            public double Eγ { get; set; }
            public double σEγ { get; set; }
            public double I { get; set; }
            public double σI { get; set; }

            public int Coincidences { get; set; }
        }
    }
}