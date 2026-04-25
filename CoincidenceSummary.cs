using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DecaySchemeTool
{
    public class CoincidenceSummary : Form
    {
        private DataGridView dgvSummary;
        private List<GateFile> _gateFiles;
        private LinkLabel lnkExport;
        private Label lblHeader;
        private double _tolerance;

        private List<CoincidenceSummaryRow> _summaryRows = new List<CoincidenceSummaryRow>();

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoincidenceSummary));
            dgvSummary = new DataGridView();
            lblHeader = new Label();
            lnkExport = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)dgvSummary).BeginInit();
            SuspendLayout();
            // 
            // dgvSummary
            // 
            dgvSummary.AllowUserToAddRows = false;
            dgvSummary.AllowUserToDeleteRows = false;
            dgvSummary.AllowUserToResizeColumns = false;
            dgvSummary.AllowUserToResizeRows = false;
            dgvSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSummary.BackgroundColor = Color.Black;
            dgvSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSummary.Cursor = Cursors.Cross;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = Color.Transparent;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvSummary.DefaultCellStyle = dataGridViewCellStyle1;
            dgvSummary.Location = new Point(25, 58);
            dgvSummary.Name = "dgvSummary";
            dgvSummary.ReadOnly = true;
            dgvSummary.RowHeadersWidth = 51;
            dgvSummary.ScrollBars = ScrollBars.Vertical;
            dgvSummary.Size = new Size(352, 503);
            dgvSummary.TabIndex = 0;
            dgvSummary.DataBindingComplete += dgvSummary_DataBindingComplete;
            dgvSummary.CellDoubleClick += dgvSummary_CellDoubleClick;
            dgvSummary.ColumnHeaderMouseClick += dgvSummary_ColumnHeaderMouseClick;
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.BackColor = Color.Transparent;
            lblHeader.ForeColor = SystemColors.ControlDark;
            lblHeader.Location = new Point(25, 35);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(261, 20);
            lblHeader.TabIndex = 1;
            lblHeader.Text = "Number of γ-γ coincidences per peak:";
            // 
            // lnkExport
            // 
            lnkExport.ActiveLinkColor = Color.WhiteSmoke;
            lnkExport.AutoSize = true;
            lnkExport.BackColor = Color.Transparent;
            lnkExport.LinkColor = Color.Gray;
            lnkExport.Location = new Point(325, 564);
            lnkExport.Name = "lnkExport";
            lnkExport.Size = new Size(52, 20);
            lnkExport.TabIndex = 2;
            lnkExport.TabStop = true;
            lnkExport.Text = "Export";
            lnkExport.VisitedLinkColor = Color.Gray;
            lnkExport.LinkClicked += lnkExport_LinkClicked;
            // 
            // CoincidenceSummary
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = SystemColors.Window;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(402, 603);
            Controls.Add(lnkExport);
            Controls.Add(lblHeader);
            Controls.Add(dgvSummary);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(420, 500);
            Name = "CoincidenceSummary";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Coincidences";
            ((System.ComponentModel.ISupportInitialize)dgvSummary).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public CoincidenceSummary(
            List<CoincidenceSummaryRow> rows,
            List<GateFile> gateFiles,
            double tolerance)
        {
            InitializeComponent();

            _gateFiles = gateFiles;
            _tolerance = tolerance;
            _summaryRows = rows.ToList();

            dgvSummary.ReadOnly = true;
            dgvSummary.MultiSelect = false;
            dgvSummary.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSummary.CurrentCell = null;
            dgvSummary.TabStop = false;

            BindSummaryRows(_summaryRows);
        }

        private void BindSummaryRows(IEnumerable<CoincidenceSummaryRow> rows)
        {
            dgvSummary.DataSource = rows.ToList();

            if (dgvSummary.Columns.Contains("Eγ"))
                dgvSummary.Columns["Eγ"].DefaultCellStyle.Format = "F3";
            if (dgvSummary.Columns.Contains("Coincidences"))
                dgvSummary.Columns["Coincidences"].DefaultCellStyle.Format = "F0";

            dgvSummary.ClearSelection();
            dgvSummary.CurrentCell = null;
        }

        private void HighlightRowsByRelativeCoincidences()
        {
            if (dgvSummary.Rows.Count == 0 || !dgvSummary.Columns.Contains("Coincidences"))
                return;

            double maxC = 0.0;

            foreach (DataGridViewRow row in dgvSummary.Rows)
            {
                if (row.IsNewRow || row.Cells["Coincidences"].Value == null)
                    continue;

                double coincidences = Convert.ToDouble(row.Cells["Coincidences"].Value);

                if (coincidences > maxC)
                    maxC = coincidences;
            }

            if (maxC <= 0.0)
                return;

            foreach (DataGridViewRow row in dgvSummary.Rows)
            {
                if (row.IsNewRow || row.Cells["Coincidences"].Value == null)
                    continue;

                double coincidences = Convert.ToDouble(row.Cells["Coincidences"].Value);
                double cRel = coincidences / maxC;

                cRel = Math.Pow(cRel, 0.5);
                cRel = Math.Max(0.0, Math.Min(1.0, cRel));

                int green = 255;
                int redBlue = (int)(255 * (1.0 - cRel));

                row.DefaultCellStyle.BackColor = Color.FromArgb(redBlue, green, redBlue);
            }
        }

        private void dgvSummary_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HighlightRowsByRelativeCoincidences();
            dgvSummary.ClearSelection();
            dgvSummary.CurrentCell = null;
        }

        private void dgvSummary_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_summaryRows.Count == 0)
                return;

            string columnName = dgvSummary.Columns[e.ColumnIndex].Name;

            IEnumerable<CoincidenceSummaryRow> sortedRows = columnName switch
            {
                "Eγ" => _summaryRows.OrderBy(r => r.Eγ),
                "Coincidences" => _summaryRows.OrderByDescending(r => r.Coincidences),
                _ => _summaryRows
            };

            _summaryRows = sortedRows.ToList();
            BindSummaryRows(_summaryRows);
        }

        private void dgvSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgvSummary.Rows[e.RowIndex];

            if (row.Cells["Eγ"].Value == null)
                return;

            double energy = Convert.ToDouble(row.Cells["Eγ"].Value);

            var partners = SearchEngine.FindCascadePartners(_gateFiles, energy, _tolerance);

            using var form = new CascadePartners(energy, partners);
            form.ShowDialog(this);
        }

        private void lnkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvSummary.Rows.Count == 0)
            {
                MessageBox.Show(
                    "There is no data to export.",
                    "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Export coincidence summary";
            saveDialog.Filter = "CSV files (*.csv)|*.csv";
            saveDialog.FileName = "coincidence-summary.csv";

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                ExportSummaryToCsv(saveDialog.FileName);

                MessageBox.Show(
                    "CSV exported successfully.",
                    "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to export CSV.\n\n{ex.Message}",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ExportSummaryToCsv(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Eγ,Coincidences");

            foreach (DataGridViewRow row in dgvSummary.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string energy = row.Cells["Eγ"].Value?.ToString() ?? "";
                string coincidences = row.Cells["Coincidences"].Value?.ToString() ?? "";

                sb.AppendLine($"{energy},{coincidences}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}