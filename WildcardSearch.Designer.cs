namespace DecaySchemeApp
{
    partial class WildcardSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WildcardSearch));
            btnSearch = new Button();
            lblWildcard = new Label();
            dgvWildcard = new DataGridView();
            txtWildcardEnergy = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvWildcard).BeginInit();
            SuspendLayout();
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(147, 11);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 29);
            btnSearch.TabIndex = 0;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblWildcard
            // 
            lblWildcard.AutoSize = true;
            lblWildcard.BackColor = Color.Transparent;
            lblWildcard.ForeColor = SystemColors.ControlDark;
            lblWildcard.Location = new Point(16, 57);
            lblWildcard.Name = "lblWildcard";
            lblWildcard.Size = new Size(211, 20);
            lblWildcard.TabIndex = 1;
            lblWildcard.Text = "Eγ entries across the database:";
            lblWildcard.Click += lblWildcard_Click;
            // 
            // dgvWildcard
            // 
            dgvWildcard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvWildcard.BackgroundColor = Color.Black;
            dgvWildcard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWildcard.Location = new Point(16, 80);
            dgvWildcard.Name = "dgvWildcard";
            dgvWildcard.RowHeadersWidth = 51;
            dgvWildcard.Size = new Size(352, 455);
            dgvWildcard.TabIndex = 2;
            dgvWildcard.ColumnHeaderMouseClick += dgvWildcard_ColumnHeaderMouseClick;
            dgvWildcard.DataBindingComplete += dgvWildcard_DataBindingComplete;
            // 
            // txtWildcardEnergy
            // 
            txtWildcardEnergy.Location = new Point(16, 12);
            txtWildcardEnergy.Name = "txtWildcardEnergy";
            txtWildcardEnergy.Size = new Size(125, 27);
            txtWildcardEnergy.TabIndex = 3;
            // 
            // WildcardSearch
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(384, 556);
            Controls.Add(txtWildcardEnergy);
            Controls.Add(dgvWildcard);
            Controls.Add(lblWildcard);
            Controls.Add(btnSearch);
            Name = "WildcardSearch";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Wildcard Search";
            ((System.ComponentModel.ISupportInitialize)dgvWildcard).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button btnSearch;
        private Label lblWildcard;
        private DataGridView dgvWildcard;
        private TextBox txtWildcardEnergy;
    }
}