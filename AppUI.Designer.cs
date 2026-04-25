namespace DecaySchemeTool
{
    partial class AppUI
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppUI));
            btnBrowseFolder = new Button();
            txtFolderPath = new TextBox();
            txtEnergy = new TextBox();
            btnSearch = new Button();
            txtTolerance = new TextBox();
            lblGateFile = new Label();
            lblStatus = new Label();
            dgvGatePeaks = new DataGridView();
            dgvMatches = new DataGridView();
            lblOccurrences = new Label();
            cmbGateEnergies = new ComboBox();
            btnOccurrenceSummary = new Button();
            lblCoinc = new Label();
            nudCoinc = new NumericUpDown();
            lblMatch = new Label();
            lblMatrix = new LinkLabel();
            btnWildcard = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvGatePeaks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMatches).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCoinc).BeginInit();
            SuspendLayout();
            // 
            // btnBrowseFolder
            // 
            btnBrowseFolder.BackColor = SystemColors.Control;
            btnBrowseFolder.Location = new Point(384, 21);
            btnBrowseFolder.Name = "btnBrowseFolder";
            btnBrowseFolder.Size = new Size(94, 29);
            btnBrowseFolder.TabIndex = 0;
            btnBrowseFolder.Text = "Browse";
            btnBrowseFolder.UseVisualStyleBackColor = false;
            btnBrowseFolder.Click += btnBrowseFolder_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.BackColor = SystemColors.Control;
            txtFolderPath.Location = new Point(58, 22);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.ReadOnly = true;
            txtFolderPath.Size = new Size(320, 27);
            txtFolderPath.TabIndex = 1;
            txtFolderPath.Text = "Folder path";
            // 
            // txtEnergy
            // 
            txtEnergy.BackColor = SystemColors.Control;
            txtEnergy.Location = new Point(894, 22);
            txtEnergy.Name = "txtEnergy";
            txtEnergy.Size = new Size(105, 27);
            txtEnergy.TabIndex = 2;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = SystemColors.Control;
            btnSearch.Location = new Point(1116, 21);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 29);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtTolerance
            // 
            txtTolerance.BackColor = SystemColors.Control;
            txtTolerance.Location = new Point(1005, 22);
            txtTolerance.Name = "txtTolerance";
            txtTolerance.Size = new Size(105, 27);
            txtTolerance.TabIndex = 4;
            // 
            // lblGateFile
            // 
            lblGateFile.AutoSize = true;
            lblGateFile.BackColor = Color.Transparent;
            lblGateFile.ForeColor = SystemColors.ControlDark;
            lblGateFile.Location = new Point(427, 124);
            lblGateFile.Name = "lblGateFile";
            lblGateFile.Size = new Size(181, 20);
            lblGateFile.TabIndex = 5;
            lblGateFile.Text = "Coincident photopeaks: --";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.Transparent;
            lblStatus.ForeColor = SystemColors.ControlDark;
            lblStatus.Location = new Point(58, 52);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(124, 20);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "No folder loaded";
            // 
            // dgvGatePeaks
            // 
            dgvGatePeaks.AllowUserToAddRows = false;
            dgvGatePeaks.AllowUserToDeleteRows = false;
            dgvGatePeaks.AllowUserToResizeColumns = false;
            dgvGatePeaks.AllowUserToResizeRows = false;
            dgvGatePeaks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvGatePeaks.BackgroundColor = Color.Black;
            dgvGatePeaks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGatePeaks.Cursor = Cursors.Cross;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = Color.Transparent;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvGatePeaks.DefaultCellStyle = dataGridViewCellStyle1;
            dgvGatePeaks.GridColor = SystemColors.ButtonFace;
            dgvGatePeaks.Location = new Point(58, 150);
            dgvGatePeaks.Name = "dgvGatePeaks";
            dgvGatePeaks.ReadOnly = true;
            dgvGatePeaks.RowHeadersWidth = 51;
            dgvGatePeaks.ScrollBars = ScrollBars.Vertical;
            dgvGatePeaks.Size = new Size(550, 485);
            dgvGatePeaks.TabIndex = 7;
            // 
            // dgvMatches
            // 
            dgvMatches.AllowUserToAddRows = false;
            dgvMatches.AllowUserToDeleteRows = false;
            dgvMatches.AllowUserToResizeColumns = false;
            dgvMatches.AllowUserToResizeRows = false;
            dgvMatches.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMatches.BackgroundColor = Color.Black;
            dgvMatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMatches.Cursor = Cursors.Cross;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = Color.Transparent;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvMatches.DefaultCellStyle = dataGridViewCellStyle2;
            dgvMatches.GridColor = SystemColors.ButtonFace;
            dgvMatches.Location = new Point(660, 150);
            dgvMatches.Name = "dgvMatches";
            dgvMatches.RowHeadersWidth = 51;
            dgvMatches.ScrollBars = ScrollBars.Vertical;
            dgvMatches.Size = new Size(550, 485);
            dgvMatches.TabIndex = 8;
            dgvMatches.CellDoubleClick += dgvMatches_CellDoubleClick;
            // 
            // lblOccurrences
            // 
            lblOccurrences.AutoSize = true;
            lblOccurrences.BackColor = Color.Transparent;
            lblOccurrences.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblOccurrences.ForeColor = SystemColors.ControlDark;
            lblOccurrences.Location = new Point(660, 124);
            lblOccurrences.Name = "lblOccurrences";
            lblOccurrences.Size = new Size(222, 20);
            lblOccurrences.TabIndex = 9;
            lblOccurrences.Text = "Occurrences in other datasets: --";
            // 
            // cmbGateEnergies
            // 
            cmbGateEnergies.BackColor = SystemColors.Control;
            cmbGateEnergies.FormattingEnabled = true;
            cmbGateEnergies.Location = new Point(783, 21);
            cmbGateEnergies.Name = "cmbGateEnergies";
            cmbGateEnergies.Size = new Size(105, 28);
            cmbGateEnergies.TabIndex = 10;
            cmbGateEnergies.SelectedIndexChanged += cmbGateEnergies_SelectedIndexChanged;
            // 
            // btnOccurrenceSummary
            // 
            btnOccurrenceSummary.BackColor = SystemColors.Control;
            btnOccurrenceSummary.Location = new Point(1116, 56);
            btnOccurrenceSummary.Name = "btnOccurrenceSummary";
            btnOccurrenceSummary.Size = new Size(94, 29);
            btnOccurrenceSummary.TabIndex = 11;
            btnOccurrenceSummary.Text = "Summary";
            btnOccurrenceSummary.UseVisualStyleBackColor = false;
            btnOccurrenceSummary.Click += btnOccurrenceSummary_Click;
            // 
            // lblCoinc
            // 
            lblCoinc.AutoSize = true;
            lblCoinc.BackColor = Color.Transparent;
            lblCoinc.ForeColor = SystemColors.ControlDark;
            lblCoinc.Location = new Point(873, 59);
            lblCoinc.Name = "lblCoinc";
            lblCoinc.Size = new Size(126, 20);
            lblCoinc.TabIndex = 13;
            lblCoinc.Text = "Min. coincidences";
            // 
            // nudCoinc
            // 
            nudCoinc.Location = new Point(1005, 57);
            nudCoinc.Name = "nudCoinc";
            nudCoinc.Size = new Size(105, 27);
            nudCoinc.TabIndex = 12;
            // 
            // lblMatch
            // 
            lblMatch.AutoSize = true;
            lblMatch.BackColor = Color.Transparent;
            lblMatch.ForeColor = SystemColors.ControlDark;
            lblMatch.Location = new Point(58, 124);
            lblMatch.Name = "lblMatch";
            lblMatch.Size = new Size(111, 20);
            lblMatch.TabIndex = 14;
            lblMatch.Text = "Matched file: --";
            // 
            // lblMatrix
            // 
            lblMatrix.ActiveLinkColor = Color.WhiteSmoke;
            lblMatrix.AutoSize = true;
            lblMatrix.BackColor = Color.Transparent;
            lblMatrix.LinkColor = Color.Gray;
            lblMatrix.Location = new Point(1112, 638);
            lblMatrix.Name = "lblMatrix";
            lblMatrix.Size = new Size(98, 20);
            lblMatrix.TabIndex = 15;
            lblMatrix.TabStop = true;
            lblMatrix.Text = "Export matrix";
            lblMatrix.VisitedLinkColor = Color.Gray;
            lblMatrix.Click += lblMatrix_Click;
            // 
            // btnWildcard
            // 
            btnWildcard.Location = new Point(1116, 115);
            btnWildcard.Name = "btnWildcard";
            btnWildcard.Size = new Size(94, 29);
            btnWildcard.TabIndex = 16;
            btnWildcard.Text = "Wildcard";
            btnWildcard.UseVisualStyleBackColor = true;
            btnWildcard.Click += btnWildcard_Click;
            // 
            // AppUI
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 673);
            Controls.Add(btnWildcard);
            Controls.Add(lblMatrix);
            Controls.Add(lblMatch);
            Controls.Add(lblCoinc);
            Controls.Add(nudCoinc);
            Controls.Add(btnOccurrenceSummary);
            Controls.Add(cmbGateEnergies);
            Controls.Add(lblOccurrences);
            Controls.Add(dgvMatches);
            Controls.Add(dgvGatePeaks);
            Controls.Add(lblStatus);
            Controls.Add(lblGateFile);
            Controls.Add(txtTolerance);
            Controls.Add(btnSearch);
            Controls.Add(txtEnergy);
            Controls.Add(txtFolderPath);
            Controls.Add(btnBrowseFolder);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AppUI";
            Text = "Decay Scheme Tool";
            Load += AppUI_Load;
            ((System.ComponentModel.ISupportInitialize)dgvGatePeaks).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMatches).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCoinc).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button btnBrowseFolder;
        private TextBox txtFolderPath;
        private TextBox txtEnergy;
        private Button btnSearch;
        private TextBox txtTolerance;
        private Label lblGateFile;
        private Label lblStatus;
        private DataGridView dgvGatePeaks;
        private DataGridView dgvMatches;
        private Label lblOccurrences;
        private ComboBox cmbGateEnergies;
		private Button btnOccurrenceSummary;
        private Label lblCoinc;
        private NumericUpDown nudCoinc;
        private Label lblMatch;
        private LinkLabel lblMatrix;
        private Button btnWildcard;
    }
}
