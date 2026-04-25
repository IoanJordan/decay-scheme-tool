namespace DecaySchemeApp
{
    partial class CascadePartners
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CascadePartners));
            lblPartners = new Label();
            dgvCascade = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvCascade).BeginInit();
            SuspendLayout();
            // 
            // lblPartners
            // 
            lblPartners.AutoSize = true;
            lblPartners.BackColor = Color.Transparent;
            lblPartners.ForeColor = SystemColors.ControlDark;
            lblPartners.Location = new Point(25, 35);
            lblPartners.Name = "lblPartners";
            lblPartners.Size = new Size(244, 20);
            lblPartners.TabIndex = 0;
            lblPartners.Text = "Photopeaks in cascade with --- keV:";
            // 
            // dgvCascade
            // 
            dgvCascade.BackgroundColor = Color.Black;
            dgvCascade.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCascade.Cursor = Cursors.Cross;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvCascade.DefaultCellStyle = dataGridViewCellStyle1;
            dgvCascade.Location = new Point(25, 58);
            dgvCascade.Name = "dgvCascade";
            dgvCascade.RowHeadersWidth = 51;
            dgvCascade.Size = new Size(352, 503);
            dgvCascade.TabIndex = 1;
            dgvCascade.DataBindingComplete += dgvCascade_DataBindingComplete;
            // 
            // CascadePartners
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(402, 603);
            Controls.Add(dgvCascade);
            Controls.Add(lblPartners);
            Name = "CascadePartners";
            Text = "Cascade Partners";
            ((System.ComponentModel.ISupportInitialize)dgvCascade).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblPartners;
        private DataGridView dgvCascade;
    }
}