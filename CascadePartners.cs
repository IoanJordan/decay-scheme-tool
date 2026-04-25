using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DecaySchemeApp
{
    public partial class CascadePartners : Form
    {
        private List<CascadePartnerRow> _cascadeRows = new List<CascadePartnerRow>();

        public CascadePartners(double energy, List<(GateFile, Peak)> partners)
        {
            InitializeComponent();

            dgvCascade.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCascade.ReadOnly = true;
            dgvCascade.MultiSelect = false;
            dgvCascade.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCascade.TabStop = false;

            lblPartners.Text = $"Photopeaks in cascade with {energy:F3} keV:";

            dgvCascade.ColumnHeaderMouseClick += dgvCascade_ColumnHeaderMouseClick;

            _cascadeRows = partners
                .OrderByDescending(p => p.Item2.Intensity)
                .Select(p => new CascadePartnerRow
                {
                    GateEγ = p.Item1.GateEnergy,
                    Eγ = p.Item2.Energy,
                    I = p.Item2.Intensity,
                })
                .ToList();

            BindCascadeRows(_cascadeRows);
        }

        private void BindCascadeRows(IEnumerable<CascadePartnerRow> rows)
        {
            dgvCascade.DataSource = rows.ToList();

            if (dgvCascade.Columns.Contains("GateEγ"))
                dgvCascade.Columns["GateEγ"].DefaultCellStyle.Format = "F3";
            if (dgvCascade.Columns.Contains("Eγ"))
                dgvCascade.Columns["Eγ"].DefaultCellStyle.Format = "F3";
            if (dgvCascade.Columns.Contains("I"))
                dgvCascade.Columns["I"].DefaultCellStyle.Format = "F0";

            HighlightRowsByRelativeIntensity();
            dgvCascade.ClearSelection();
            dgvCascade.CurrentCell = null;
        }

        private void dgvCascade_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_cascadeRows.Count == 0)
                return;

            string columnName = dgvCascade.Columns[e.ColumnIndex].Name;

            IEnumerable<CascadePartnerRow> sortedRows = columnName switch
            {
                "GateEγ" => _cascadeRows.OrderBy(r => r.GateEγ),
                "Eγ" => _cascadeRows.OrderBy(r => r.Eγ),
                "I" => _cascadeRows.OrderByDescending(r => r.I),
                _ => _cascadeRows
            };

            _cascadeRows = sortedRows.ToList();
            BindCascadeRows(_cascadeRows);
        }

        private void HighlightRowsByRelativeIntensity()
        {
            if (dgvCascade.Rows.Count == 0 || !dgvCascade.Columns.Contains("I"))
                return;

            double maxI = 0.0;

            foreach (DataGridViewRow row in dgvCascade.Rows)
            {
                if (row.IsNewRow || row.Cells["I"].Value == null)
                    continue;

                double intensity = Convert.ToDouble(row.Cells["I"].Value);

                if (intensity > maxI)
                    maxI = intensity;
            }

            if (maxI <= 0.0)
                return;

            foreach (DataGridViewRow row in dgvCascade.Rows)
            {
                if (row.IsNewRow || row.Cells["I"].Value == null)
                    continue;

                double intensity = Convert.ToDouble(row.Cells["I"].Value);
                double iRel = intensity / maxI;

                iRel = Math.Pow(iRel, 0.5);
                iRel = Math.Max(0.0, Math.Min(1.0, iRel));

                int green = 255;
                int redBlue = (int)(255 * (1.0 - iRel));

                row.DefaultCellStyle.BackColor = Color.FromArgb(redBlue, green, redBlue);
            }
        }

        private void dgvCascade_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HighlightRowsByRelativeIntensity();
            dgvCascade.ClearSelection();
            dgvCascade.CurrentCell = null;
        }

        private class CascadePartnerRow
        {
            public double GateEγ { get; set; }
            public double Eγ { get; set; }
            public double I { get; set; }
        }
    }
}