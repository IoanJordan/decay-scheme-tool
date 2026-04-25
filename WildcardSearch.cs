using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace DecaySchemeApp
{
    public partial class WildcardSearch : Form
    {
        private readonly List<GateFile> _gateFiles;
        private readonly double _tolerance;

        private List<WildcardRow> _rows = new List<WildcardRow>();

        public WildcardSearch(List<GateFile> gateFiles, double tolerance)
        {
            InitializeComponent();

            _gateFiles = gateFiles;
            _tolerance = tolerance;

            lblWildcard.Text = $"Search Eγ entries across the database.";
            txtWildcardEnergy.PlaceholderText = "Energy (keV)";
            txtWildcardEnergy.Text = string.Empty;

            dgvWildcard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWildcard.ReadOnly = true;
            dgvWildcard.MultiSelect = false;
            dgvWildcard.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvWildcard.AllowUserToAddRows = false;
            dgvWildcard.AllowUserToDeleteRows = false;
            dgvWildcard.AllowUserToResizeColumns = false;
            dgvWildcard.AllowUserToResizeRows = false;
            dgvWildcard.TabStop = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(
                    txtWildcardEnergy.Text,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double searchEnergy))
            {
                MessageBox.Show(
                    "Invalid energy value.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var matches = SearchEngine.FindWildcardMatches(_gateFiles, searchEnergy, _tolerance);

            _rows = matches
                .Select(m => new WildcardRow
                {
                    GateEγ = m.GateFile.GateEnergy,
                    Eγ = m.Peak.Energy,
                    I = m.Peak.Intensity,
                    PercentErr = m.Peak.Intensity != 0.0
                        ? (m.Peak.IntensityError / m.Peak.Intensity) * 100.0
                        : 0.0
                })
                .ToList();

            BindRows(_rows);
        }

        private void BindRows(IEnumerable<WildcardRow> rows)
        {
            dgvWildcard.DataSource = rows.ToList();

            if (dgvWildcard.Columns.Contains("GateEγ"))
                dgvWildcard.Columns["GateEγ"].DefaultCellStyle.Format = "F3";

            if (dgvWildcard.Columns.Contains("Eγ"))
                dgvWildcard.Columns["Eγ"].DefaultCellStyle.Format = "F3";

            if (dgvWildcard.Columns.Contains("I"))
                dgvWildcard.Columns["I"].DefaultCellStyle.Format = "F0";

            if (dgvWildcard.Columns.Contains("PercentErr"))
            {
                dgvWildcard.Columns["PercentErr"].HeaderText = "%Err";
                dgvWildcard.Columns["PercentErr"].DefaultCellStyle.Format = "F1";
            }

            dgvWildcard.ClearSelection();
            dgvWildcard.CurrentCell = null;
        }

        private void dgvWildcard_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HighlightRowsByPercentError();
            dgvWildcard.ClearSelection();
            dgvWildcard.CurrentCell = null;
        }

        private void dgvWildcard_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_rows.Count == 0)
                return;

            string columnName = dgvWildcard.Columns[e.ColumnIndex].Name;

            IEnumerable<WildcardRow> sortedRows = columnName switch
            {
                "GateEγ" => _rows.OrderBy(r => r.GateEγ),
                "Eγ" => _rows.OrderBy(r => r.Eγ),
                "I" => _rows.OrderByDescending(r => r.I),
                "PercentErr" => _rows.OrderBy(r => r.PercentErr),
                _ => _rows
            };

            _rows = sortedRows.ToList();
            BindRows(_rows);
        }

        private void HighlightRowsByPercentError()
        {
            if (dgvWildcard.Rows.Count == 0 || !dgvWildcard.Columns.Contains("PercentErr"))
                return;

            double maxErr = 0.0;

            foreach (DataGridViewRow row in dgvWildcard.Rows)
            {
                if (row.IsNewRow || row.Cells["PercentErr"].Value == null)
                    continue;

                double err = Convert.ToDouble(row.Cells["PercentErr"].Value);

                if (err > maxErr)
                    maxErr = err;
            }

            if (maxErr <= 0.0)
                return;

            foreach (DataGridViewRow row in dgvWildcard.Rows)
            {
                if (row.IsNewRow || row.Cells["PercentErr"].Value == null)
                    continue;

                double err = Convert.ToDouble(row.Cells["PercentErr"].Value);
                double rel = err / maxErr;

                rel = Math.Max(0.0, Math.Min(1.0, rel));

                int red = 255;
                int greenBlue = (int)(255 * (1.0 - rel));

                row.DefaultCellStyle.BackColor = Color.FromArgb(red, greenBlue, greenBlue);
            }
        }

        private class WildcardRow
        {
            public double GateEγ { get; set; }
            public double Eγ { get; set; }
            public double I { get; set; }
            public double PercentErr { get; set; }
        }

        private void lblWildcard_Click(object sender, EventArgs e)
        {

        }
    }
}