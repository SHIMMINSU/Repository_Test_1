namespace MFQCIMLogChecker
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle53 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle54 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle55 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmbEQSelect = new System.Windows.Forms.ComboBox();
            this.lblEQSelect = new System.Windows.Forms.Label();
            this.txtReuslt = new System.Windows.Forms.TextBox();
            this.btnLogCheck = new System.Windows.Forms.Button();
            this.txtLogPath = new System.Windows.Forms.TextBox();
            this.lblLogPath = new System.Windows.Forms.Label();
            this.btnLogFilePathSet = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblView = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkACFCheck = new System.Windows.Forms.CheckBox();
            this.chkExportToExcelColor = new System.Windows.Forms.CheckBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.btnLoginCancel = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPW = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.adgvFilter = new ADGV.AdvancedDataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnFilerCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.adgvTKAverage = new ADGV.AdvancedDataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.adgvTKData = new ADGV.AdvancedDataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTKResult = new System.Windows.Forms.TextBox();
            this.txtTKOutLogPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTKOutLogSet = new System.Windows.Forms.Button();
            this.txtTKInLogPath = new System.Windows.Forms.TextBox();
            this.prgTKStatus = new System.Windows.Forms.ProgressBar();
            this.btnTKLogCheck = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTKExportExcel = new System.Windows.Forms.Button();
            this.btnTKInLogSet = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkFrameCOFCheck = new System.Windows.Forms.CheckBox();
            this.cmbLogVerSelect = new System.Windows.Forms.ComboBox();
            this.lblLogVer = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPLCSetValue = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtModifyDevice = new System.Windows.Forms.TextBox();
            this.btnPLCSet = new System.Windows.Forms.Button();
            this.dgvMemory = new System.Windows.Forms.DataGridView();
            this.Device = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Binary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ASCII = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.txtStationNo = new System.Windows.Forms.TextBox();
            this.lblstation = new System.Windows.Forms.Label();
            this.btnPLCConnect = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.btnALLAnalysis = new System.Windows.Forms.Button();
            this.btnRemote = new System.Windows.Forms.Button();
            this.btnPing = new System.Windows.Forms.Button();
            this.btnExplorer = new System.Windows.Forms.Button();
            this.cbEQPID = new System.Windows.Forms.ComboBox();
            this.ckbNonRealtime = new System.Windows.Forms.CheckBox();
            this.btnCount = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.tbRangeEnd = new System.Windows.Forms.TextBox();
            this.tbRangeStart = new System.Windows.Forms.TextBox();
            this.labelRange = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.rbtUnfixed = new System.Windows.Forms.RadioButton();
            this.rbtFixed = new System.Windows.Forms.RadioButton();
            this.btnExcel = new System.Windows.Forms.Button();
            this.tbLogItemCount = new System.Windows.Forms.TextBox();
            this.cbMessageSub = new System.Windows.Forms.ComboBox();
            this.cbMessage = new System.Windows.Forms.ComboBox();
            this.btnCimLogAnalysis = new System.Windows.Forms.Button();
            this.btnSelectLogFile = new System.Windows.Forms.Button();
            this.tbLogFile = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbLogAnalysis = new System.Windows.Forms.TextBox();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.pnlLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adgvFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvTKAverage)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvTKData)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMemory)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbEQSelect
            // 
            this.cmbEQSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEQSelect.FormattingEnabled = true;
            this.cmbEQSelect.Items.AddRange(new object[] {
            "COG",
            "FOG"});
            this.cmbEQSelect.Location = new System.Drawing.Point(60, 33);
            this.cmbEQSelect.Name = "cmbEQSelect";
            this.cmbEQSelect.Size = new System.Drawing.Size(121, 20);
            this.cmbEQSelect.TabIndex = 0;
            this.cmbEQSelect.SelectedIndexChanged += new System.EventHandler(this.cmbEQSelect_SelectedIndexChanged);
            // 
            // lblEQSelect
            // 
            this.lblEQSelect.AutoSize = true;
            this.lblEQSelect.Location = new System.Drawing.Point(5, 37);
            this.lblEQSelect.Name = "lblEQSelect";
            this.lblEQSelect.Size = new System.Drawing.Size(53, 12);
            this.lblEQSelect.TabIndex = 1;
            this.lblEQSelect.Text = "설비선택";
            // 
            // txtReuslt
            // 
            this.txtReuslt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReuslt.Location = new System.Drawing.Point(538, 24);
            this.txtReuslt.Multiline = true;
            this.txtReuslt.Name = "txtReuslt";
            this.txtReuslt.ReadOnly = true;
            this.txtReuslt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReuslt.Size = new System.Drawing.Size(392, 68);
            this.txtReuslt.TabIndex = 3;
            this.txtReuslt.Click += new System.EventHandler(this.txtReuslt_Click);
            // 
            // btnLogCheck
            // 
            this.btnLogCheck.Enabled = false;
            this.btnLogCheck.Location = new System.Drawing.Point(454, 31);
            this.btnLogCheck.Name = "btnLogCheck";
            this.btnLogCheck.Size = new System.Drawing.Size(77, 23);
            this.btnLogCheck.TabIndex = 4;
            this.btnLogCheck.Text = "로그검사";
            this.btnLogCheck.UseVisualStyleBackColor = true;
            this.btnLogCheck.Click += new System.EventHandler(this.btnLogCheck_Click);
            // 
            // txtLogPath
            // 
            this.txtLogPath.Location = new System.Drawing.Point(60, 6);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.ReadOnly = true;
            this.txtLogPath.Size = new System.Drawing.Size(391, 21);
            this.txtLogPath.TabIndex = 5;
            // 
            // lblLogPath
            // 
            this.lblLogPath.AutoSize = true;
            this.lblLogPath.Location = new System.Drawing.Point(5, 9);
            this.lblLogPath.Name = "lblLogPath";
            this.lblLogPath.Size = new System.Drawing.Size(53, 12);
            this.lblLogPath.TabIndex = 6;
            this.lblLogPath.Text = "로그경로";
            // 
            // btnLogFilePathSet
            // 
            this.btnLogFilePathSet.Enabled = false;
            this.btnLogFilePathSet.Location = new System.Drawing.Point(454, 4);
            this.btnLogFilePathSet.Name = "btnLogFilePathSet";
            this.btnLogFilePathSet.Size = new System.Drawing.Size(77, 23);
            this.btnLogFilePathSet.TabIndex = 7;
            this.btnLogFilePathSet.Text = "로그설정";
            this.btnLogFilePathSet.UseVisualStyleBackColor = true;
            this.btnLogFilePathSet.Click += new System.EventHandler(this.btnLogFilePathSet_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "*.log|*.log";
            // 
            // lblView
            // 
            this.lblView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblView.Location = new System.Drawing.Point(3, 95);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(928, 21);
            this.lblView.TabIndex = 15;
            this.lblView.Text = "LOG VIEW";
            this.lblView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(538, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(392, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "LOG 검사 결과(Click시 LOG VIEW 해당 ROW 선택)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkACFCheck
            // 
            this.chkACFCheck.AutoSize = true;
            this.chkACFCheck.Location = new System.Drawing.Point(363, 32);
            this.chkACFCheck.Name = "chkACFCheck";
            this.chkACFCheck.Size = new System.Drawing.Size(76, 16);
            this.chkACFCheck.TabIndex = 17;
            this.chkACFCheck.Text = "ACF 검사";
            this.chkACFCheck.UseVisualStyleBackColor = true;
            // 
            // chkExportToExcelColor
            // 
            this.chkExportToExcelColor.AutoSize = true;
            this.chkExportToExcelColor.BackColor = System.Drawing.Color.White;
            this.chkExportToExcelColor.Location = new System.Drawing.Point(363, 67);
            this.chkExportToExcelColor.Name = "chkExportToExcelColor";
            this.chkExportToExcelColor.Size = new System.Drawing.Size(90, 28);
            this.chkExportToExcelColor.TabIndex = 20;
            this.chkExportToExcelColor.Text = "Export To \r\nExcel Color";
            this.chkExportToExcelColor.UseVisualStyleBackColor = false;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Enabled = false;
            this.btnExportExcel.Location = new System.Drawing.Point(454, 57);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(77, 36);
            this.btnExportExcel.TabIndex = 19;
            this.btnExportExcel.Text = "Export\r\nTo Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // pnlLogin
            // 
            this.pnlLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLogin.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLogin.Controls.Add(this.btnLoginCancel);
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Controls.Add(this.txtPW);
            this.pnlLogin.Controls.Add(this.txtID);
            this.pnlLogin.Controls.Add(this.pictureBox1);
            this.pnlLogin.Controls.Add(this.lblPassword);
            this.pnlLogin.Controls.Add(this.lblID);
            this.pnlLogin.Controls.Add(this.label4);
            this.pnlLogin.Location = new System.Drawing.Point(353, 199);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(239, 138);
            this.pnlLogin.TabIndex = 22;
            // 
            // btnLoginCancel
            // 
            this.btnLoginCancel.Location = new System.Drawing.Point(178, 35);
            this.btnLoginCancel.Name = "btnLoginCancel";
            this.btnLoginCancel.Size = new System.Drawing.Size(54, 22);
            this.btnLoginCancel.TabIndex = 27;
            this.btnLoginCancel.Text = "취소";
            this.btnLoginCancel.UseVisualStyleBackColor = true;
            this.btnLoginCancel.Click += new System.EventHandler(this.btnLoginCancel_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(178, 7);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(54, 22);
            this.btnLogin.TabIndex = 23;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPW
            // 
            this.txtPW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPW.Location = new System.Drawing.Point(74, 35);
            this.txtPW.Multiline = true;
            this.txtPW.Name = "txtPW";
            this.txtPW.PasswordChar = '*';
            this.txtPW.Size = new System.Drawing.Size(103, 21);
            this.txtPW.TabIndex = 26;
            this.txtPW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPW_KeyDown);
            // 
            // txtID
            // 
            this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtID.Location = new System.Drawing.Point(74, 8);
            this.txtID.Multiline = true;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(103, 21);
            this.txtID.TabIndex = 23;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Image = global::MFQCIMLogChecker.Properties.Resources.Ucore_System;
            this.pictureBox1.Location = new System.Drawing.Point(11, 63);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(221, 57);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 40);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(69, 12);
            this.lblPassword.TabIndex = 24;
            this.lblPassword.Text = "비밀 번호 : ";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(10, 12);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(69, 12);
            this.lblID.TabIndex = 23;
            this.lblID.Text = "계정 입력 : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "Copyright ©  2019 - UcoreSystem(주)";
            // 
            // prgStatus
            // 
            this.prgStatus.Location = new System.Drawing.Point(8, 60);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(343, 28);
            this.prgStatus.Step = 1;
            this.prgStatus.TabIndex = 18;
            // 
            // adgvFilter
            // 
            this.adgvFilter.AllowUserToAddRows = false;
            this.adgvFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adgvFilter.AutoGenerateContextFilters = true;
            this.adgvFilter.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle45.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle45.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle45.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle45.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle45.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle45.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adgvFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle45;
            this.adgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvFilter.DateWithTime = false;
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle46.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle46.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle46.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle46.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle46.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle46.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.adgvFilter.DefaultCellStyle = dataGridViewCellStyle46;
            this.adgvFilter.Location = new System.Drawing.Point(3, 116);
            this.adgvFilter.Name = "adgvFilter";
            this.adgvFilter.ReadOnly = true;
            this.adgvFilter.RowHeadersWidth = 80;
            this.adgvFilter.RowTemplate.Height = 23;
            this.adgvFilter.Size = new System.Drawing.Size(928, 293);
            this.adgvFilter.TabIndex = 23;
            this.adgvFilter.TimeFilter = false;
            this.adgvFilter.SortStringChanged += new System.EventHandler(this.adgvFilter_SortStringChanged);
            this.adgvFilter.FilterStringChanged += new System.EventHandler(this.adgvFilter_FilterStringChanged);
            // 
            // btnFilerCancel
            // 
            this.btnFilerCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilerCancel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFilerCancel.Location = new System.Drawing.Point(847, 96);
            this.btnFilerCancel.Name = "btnFilerCancel";
            this.btnFilerCancel.Size = new System.Drawing.Size(84, 19);
            this.btnFilerCancel.TabIndex = 24;
            this.btnFilerCancel.Text = "Filter Cancel";
            this.btnFilerCancel.UseVisualStyleBackColor = true;
            this.btnFilerCancel.Click += new System.EventHandler(this.btnFilerCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(7, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(942, 439);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.txtTKResult);
            this.tabPage2.Controls.Add(this.txtTKOutLogPath);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.btnTKOutLogSet);
            this.tabPage2.Controls.Add(this.txtTKInLogPath);
            this.tabPage2.Controls.Add(this.prgTKStatus);
            this.tabPage2.Controls.Add(this.btnTKLogCheck);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.btnTKExportExcel);
            this.tabPage2.Controls.Add(this.btnTKInLogSet);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(934, 413);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Track In/Out";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(8, 96);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(922, 314);
            this.tabControl2.TabIndex = 27;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.adgvTKAverage);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(914, 288);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Tracking 정합성";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(6, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(902, 21);
            this.label6.TabIndex = 25;
            this.label6.Text = "Track In/Out 정합성 결과";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // adgvTKAverage
            // 
            this.adgvTKAverage.AllowUserToAddRows = false;
            this.adgvTKAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adgvTKAverage.AutoGenerateContextFilters = true;
            this.adgvTKAverage.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle47.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle47.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle47.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle47.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle47.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle47.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adgvTKAverage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle47;
            this.adgvTKAverage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvTKAverage.DateWithTime = false;
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.adgvTKAverage.DefaultCellStyle = dataGridViewCellStyle48;
            this.adgvTKAverage.Location = new System.Drawing.Point(6, 25);
            this.adgvTKAverage.Name = "adgvTKAverage";
            this.adgvTKAverage.ReadOnly = true;
            dataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle49.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle49.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle49.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle49.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle49.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle49.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adgvTKAverage.RowHeadersDefaultCellStyle = dataGridViewCellStyle49;
            this.adgvTKAverage.RowHeadersVisible = false;
            this.adgvTKAverage.RowHeadersWidth = 80;
            this.adgvTKAverage.RowTemplate.Height = 23;
            this.adgvTKAverage.Size = new System.Drawing.Size(902, 257);
            this.adgvTKAverage.TabIndex = 24;
            this.adgvTKAverage.TimeFilter = false;
            this.adgvTKAverage.SortStringChanged += new System.EventHandler(this.adgvTKAverage_SortStringChanged);
            this.adgvTKAverage.FilterStringChanged += new System.EventHandler(this.adgvTKAverage_FilterStringChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.adgvTKData);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(914, 288);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Data View";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(6, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(902, 21);
            this.label7.TabIndex = 26;
            this.label7.Text = "Track In/Out Data";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // adgvTKData
            // 
            this.adgvTKData.AllowUserToAddRows = false;
            this.adgvTKData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adgvTKData.AutoGenerateContextFilters = true;
            this.adgvTKData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle50.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle50.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle50.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle50.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle50.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle50.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adgvTKData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle50;
            this.adgvTKData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvTKData.DateWithTime = false;
            dataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle51.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle51.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle51.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle51.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle51.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle51.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.adgvTKData.DefaultCellStyle = dataGridViewCellStyle51;
            this.adgvTKData.Location = new System.Drawing.Point(6, 25);
            this.adgvTKData.Name = "adgvTKData";
            this.adgvTKData.ReadOnly = true;
            dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle52.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle52.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle52.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle52.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle52.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle52.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adgvTKData.RowHeadersDefaultCellStyle = dataGridViewCellStyle52;
            this.adgvTKData.RowHeadersVisible = false;
            this.adgvTKData.RowHeadersWidth = 80;
            this.adgvTKData.RowTemplate.Height = 23;
            this.adgvTKData.Size = new System.Drawing.Size(902, 257);
            this.adgvTKData.TabIndex = 25;
            this.adgvTKData.TimeFilter = false;
            this.adgvTKData.SortStringChanged += new System.EventHandler(this.adgvTKData_SortStringChanged);
            this.adgvTKData.FilterStringChanged += new System.EventHandler(this.adgvTKData_FilterStringChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(534, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "LOG 검사 결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTKResult
            // 
            this.txtTKResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTKResult.Location = new System.Drawing.Point(534, 24);
            this.txtTKResult.Multiline = true;
            this.txtTKResult.Name = "txtTKResult";
            this.txtTKResult.ReadOnly = true;
            this.txtTKResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTKResult.Size = new System.Drawing.Size(392, 68);
            this.txtTKResult.TabIndex = 24;
            // 
            // txtTKOutLogPath
            // 
            this.txtTKOutLogPath.Location = new System.Drawing.Point(67, 33);
            this.txtTKOutLogPath.Name = "txtTKOutLogPath";
            this.txtTKOutLogPath.ReadOnly = true;
            this.txtTKOutLogPath.Size = new System.Drawing.Size(303, 21);
            this.txtTKOutLogPath.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "Track Out";
            // 
            // btnTKOutLogSet
            // 
            this.btnTKOutLogSet.Enabled = false;
            this.btnTKOutLogSet.Location = new System.Drawing.Point(373, 32);
            this.btnTKOutLogSet.Name = "btnTKOutLogSet";
            this.btnTKOutLogSet.Size = new System.Drawing.Size(77, 23);
            this.btnTKOutLogSet.TabIndex = 23;
            this.btnTKOutLogSet.Text = "로그설정2";
            this.btnTKOutLogSet.UseVisualStyleBackColor = true;
            this.btnTKOutLogSet.Click += new System.EventHandler(this.btnTKOutLogSet_Click);
            // 
            // txtTKInLogPath
            // 
            this.txtTKInLogPath.Location = new System.Drawing.Point(67, 6);
            this.txtTKInLogPath.Name = "txtTKInLogPath";
            this.txtTKInLogPath.ReadOnly = true;
            this.txtTKInLogPath.Size = new System.Drawing.Size(303, 21);
            this.txtTKInLogPath.TabIndex = 5;
            // 
            // prgTKStatus
            // 
            this.prgTKStatus.Location = new System.Drawing.Point(8, 62);
            this.prgTKStatus.Name = "prgTKStatus";
            this.prgTKStatus.Size = new System.Drawing.Size(442, 28);
            this.prgTKStatus.TabIndex = 18;
            // 
            // btnTKLogCheck
            // 
            this.btnTKLogCheck.Enabled = false;
            this.btnTKLogCheck.Location = new System.Drawing.Point(454, 5);
            this.btnTKLogCheck.Name = "btnTKLogCheck";
            this.btnTKLogCheck.Size = new System.Drawing.Size(76, 50);
            this.btnTKLogCheck.TabIndex = 4;
            this.btnTKLogCheck.Text = "로그검사";
            this.btnTKLogCheck.UseVisualStyleBackColor = true;
            this.btnTKLogCheck.Click += new System.EventHandler(this.btnTKLogCheck_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "Track In";
            // 
            // btnTKExportExcel
            // 
            this.btnTKExportExcel.Enabled = false;
            this.btnTKExportExcel.Location = new System.Drawing.Point(455, 57);
            this.btnTKExportExcel.Name = "btnTKExportExcel";
            this.btnTKExportExcel.Size = new System.Drawing.Size(75, 36);
            this.btnTKExportExcel.TabIndex = 19;
            this.btnTKExportExcel.Text = "Export\r\nTo Excel";
            this.btnTKExportExcel.UseVisualStyleBackColor = true;
            this.btnTKExportExcel.Click += new System.EventHandler(this.btnTKExportExcel_Click);
            // 
            // btnTKInLogSet
            // 
            this.btnTKInLogSet.Enabled = false;
            this.btnTKInLogSet.Location = new System.Drawing.Point(373, 5);
            this.btnTKInLogSet.Name = "btnTKInLogSet";
            this.btnTKInLogSet.Size = new System.Drawing.Size(77, 23);
            this.btnTKInLogSet.TabIndex = 7;
            this.btnTKInLogSet.Text = "로그설정1";
            this.btnTKInLogSet.UseVisualStyleBackColor = true;
            this.btnTKInLogSet.Click += new System.EventHandler(this.btnTKInLogSet_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.btnLogCheck);
            this.tabPage1.Controls.Add(this.btnExportExcel);
            this.tabPage1.Controls.Add(this.chkFrameCOFCheck);
            this.tabPage1.Controls.Add(this.cmbLogVerSelect);
            this.tabPage1.Controls.Add(this.lblLogVer);
            this.tabPage1.Controls.Add(this.btnFilerCancel);
            this.tabPage1.Controls.Add(this.txtLogPath);
            this.tabPage1.Controls.Add(this.prgStatus);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtReuslt);
            this.tabPage1.Controls.Add(this.cmbEQSelect);
            this.tabPage1.Controls.Add(this.lblView);
            this.tabPage1.Controls.Add(this.lblEQSelect);
            this.tabPage1.Controls.Add(this.adgvFilter);
            this.tabPage1.Controls.Add(this.chkExportToExcelColor);
            this.tabPage1.Controls.Add(this.lblLogPath);
            this.tabPage1.Controls.Add(this.btnLogFilePathSet);
            this.tabPage1.Controls.Add(this.chkACFCheck);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(934, 413);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Material";
            // 
            // chkFrameCOFCheck
            // 
            this.chkFrameCOFCheck.AutoSize = true;
            this.chkFrameCOFCheck.Location = new System.Drawing.Point(363, 51);
            this.chkFrameCOFCheck.Name = "chkFrameCOFCheck";
            this.chkFrameCOFCheck.Size = new System.Drawing.Size(76, 16);
            this.chkFrameCOFCheck.TabIndex = 27;
            this.chkFrameCOFCheck.Text = "금형 검사";
            this.chkFrameCOFCheck.UseVisualStyleBackColor = true;
            // 
            // cmbLogVerSelect
            // 
            this.cmbLogVerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogVerSelect.FormattingEnabled = true;
            this.cmbLogVerSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbLogVerSelect.Items.AddRange(new object[] {
            "v1.19",
            "v1.18 이하 버전"});
            this.cmbLogVerSelect.Location = new System.Drawing.Point(238, 33);
            this.cmbLogVerSelect.Name = "cmbLogVerSelect";
            this.cmbLogVerSelect.Size = new System.Drawing.Size(113, 20);
            this.cmbLogVerSelect.TabIndex = 25;
            // 
            // lblLogVer
            // 
            this.lblLogVer.AutoSize = true;
            this.lblLogVer.Location = new System.Drawing.Point(183, 37);
            this.lblLogVer.Name = "lblLogVer";
            this.lblLogVer.Size = new System.Drawing.Size(53, 12);
            this.lblLogVer.TabIndex = 26;
            this.lblLogVer.Text = "로그버전";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Controls.Add(this.txtPLCSetValue);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.txtModifyDevice);
            this.tabPage5.Controls.Add(this.btnPLCSet);
            this.tabPage5.Controls.Add(this.dgvMemory);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.txtDevice);
            this.tabPage5.Controls.Add(this.txtStationNo);
            this.tabPage5.Controls.Add(this.lblstation);
            this.tabPage5.Controls.Add(this.btnPLCConnect);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(934, 413);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "PLC Control";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(579, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 12;
            this.label10.Text = "Set Value";
            // 
            // txtPLCSetValue
            // 
            this.txtPLCSetValue.Location = new System.Drawing.Point(640, 12);
            this.txtPLCSetValue.Name = "txtPLCSetValue";
            this.txtPLCSetValue.Size = new System.Drawing.Size(72, 21);
            this.txtPLCSetValue.TabIndex = 11;
            this.txtPLCSetValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLCSetValue_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(413, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "Modify Device";
            // 
            // txtModifyDevice
            // 
            this.txtModifyDevice.Location = new System.Drawing.Point(501, 12);
            this.txtModifyDevice.Name = "txtModifyDevice";
            this.txtModifyDevice.Size = new System.Drawing.Size(72, 21);
            this.txtModifyDevice.TabIndex = 9;
            // 
            // btnPLCSet
            // 
            this.btnPLCSet.Enabled = false;
            this.btnPLCSet.Location = new System.Drawing.Point(713, 11);
            this.btnPLCSet.Name = "btnPLCSet";
            this.btnPLCSet.Size = new System.Drawing.Size(75, 23);
            this.btnPLCSet.TabIndex = 8;
            this.btnPLCSet.Text = "PLC Set";
            this.btnPLCSet.UseVisualStyleBackColor = true;
            this.btnPLCSet.Click += new System.EventHandler(this.btnPLCSet_Click);
            // 
            // dgvMemory
            // 
            this.dgvMemory.AllowUserToAddRows = false;
            this.dgvMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMemory.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle53.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle53.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle53.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle53.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle53.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle53.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMemory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle53;
            this.dgvMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMemory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Device,
            this.DEC,
            this.Binary,
            this.HEX,
            this.ASCII});
            dataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle54.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle54.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle54.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle54.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle54.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle54.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMemory.DefaultCellStyle = dataGridViewCellStyle54;
            this.dgvMemory.Location = new System.Drawing.Point(8, 40);
            this.dgvMemory.Name = "dgvMemory";
            this.dgvMemory.ReadOnly = true;
            dataGridViewCellStyle55.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle55.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle55.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle55.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle55.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle55.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle55.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMemory.RowHeadersDefaultCellStyle = dataGridViewCellStyle55;
            this.dgvMemory.RowHeadersVisible = false;
            this.dgvMemory.RowTemplate.Height = 23;
            this.dgvMemory.Size = new System.Drawing.Size(920, 367);
            this.dgvMemory.TabIndex = 7;
            // 
            // Device
            // 
            this.Device.HeaderText = "Device";
            this.Device.Name = "Device";
            this.Device.ReadOnly = true;
            // 
            // DEC
            // 
            this.DEC.HeaderText = "DEC";
            this.DEC.Name = "DEC";
            this.DEC.ReadOnly = true;
            // 
            // Binary
            // 
            this.Binary.HeaderText = "Binary";
            this.Binary.Name = "Binary";
            this.Binary.ReadOnly = true;
            // 
            // HEX
            // 
            this.HEX.HeaderText = "HEX";
            this.HEX.Name = "HEX";
            this.HEX.ReadOnly = true;
            // 
            // ASCII
            // 
            this.ASCII.HeaderText = "ASCII";
            this.ASCII.Name = "ASCII";
            this.ASCII.ReadOnly = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(133, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "Device";
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(178, 12);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.Size = new System.Drawing.Size(72, 21);
            this.txtDevice.TabIndex = 5;
            // 
            // txtStationNo
            // 
            this.txtStationNo.Location = new System.Drawing.Point(58, 12);
            this.txtStationNo.Name = "txtStationNo";
            this.txtStationNo.Size = new System.Drawing.Size(72, 21);
            this.txtStationNo.TabIndex = 2;
            this.txtStationNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStationNo_KeyPress);
            // 
            // lblstation
            // 
            this.lblstation.AutoSize = true;
            this.lblstation.Location = new System.Drawing.Point(13, 16);
            this.lblstation.Name = "lblstation";
            this.lblstation.Size = new System.Drawing.Size(43, 12);
            this.lblstation.TabIndex = 1;
            this.lblstation.Text = "Station";
            // 
            // btnPLCConnect
            // 
            this.btnPLCConnect.Location = new System.Drawing.Point(251, 11);
            this.btnPLCConnect.Name = "btnPLCConnect";
            this.btnPLCConnect.Size = new System.Drawing.Size(75, 23);
            this.btnPLCConnect.TabIndex = 0;
            this.btnPLCConnect.Text = "Connect";
            this.btnPLCConnect.UseVisualStyleBackColor = true;
            this.btnPLCConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.btnALLAnalysis);
            this.tabPage6.Controls.Add(this.btnRemote);
            this.tabPage6.Controls.Add(this.btnPing);
            this.tabPage6.Controls.Add(this.btnExplorer);
            this.tabPage6.Controls.Add(this.cbEQPID);
            this.tabPage6.Controls.Add(this.ckbNonRealtime);
            this.tabPage6.Controls.Add(this.btnCount);
            this.tabPage6.Controls.Add(this.label11);
            this.tabPage6.Controls.Add(this.tbRangeEnd);
            this.tabPage6.Controls.Add(this.tbRangeStart);
            this.tabPage6.Controls.Add(this.labelRange);
            this.tabPage6.Controls.Add(this.tbMessage);
            this.tabPage6.Controls.Add(this.rbtUnfixed);
            this.tabPage6.Controls.Add(this.rbtFixed);
            this.tabPage6.Controls.Add(this.btnExcel);
            this.tabPage6.Controls.Add(this.tbLogItemCount);
            this.tabPage6.Controls.Add(this.cbMessageSub);
            this.tabPage6.Controls.Add(this.cbMessage);
            this.tabPage6.Controls.Add(this.btnCimLogAnalysis);
            this.tabPage6.Controls.Add(this.btnSelectLogFile);
            this.tabPage6.Controls.Add(this.tbLogFile);
            this.tabPage6.Controls.Add(this.btnClear);
            this.tabPage6.Controls.Add(this.tbLogAnalysis);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(934, 413);
            this.tabPage6.TabIndex = 4;
            this.tabPage6.Text = "Analysis";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // btnALLAnalysis
            // 
            this.btnALLAnalysis.Location = new System.Drawing.Point(575, 141);
            this.btnALLAnalysis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnALLAnalysis.Name = "btnALLAnalysis";
            this.btnALLAnalysis.Size = new System.Drawing.Size(84, 56);
            this.btnALLAnalysis.TabIndex = 48;
            this.btnALLAnalysis.Text = "ALL";
            this.btnALLAnalysis.UseVisualStyleBackColor = true;
            this.btnALLAnalysis.Click += new System.EventHandler(this.btnALLAnalysis_Click);
            // 
            // btnRemote
            // 
            this.btnRemote.Location = new System.Drawing.Point(667, 6);
            this.btnRemote.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.Size = new System.Drawing.Size(80, 34);
            this.btnRemote.TabIndex = 47;
            this.btnRemote.Text = "Remote";
            this.btnRemote.UseVisualStyleBackColor = true;
            this.btnRemote.Click += new System.EventHandler(this.btnRemote_Click);
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(580, 6);
            this.btnPing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(80, 34);
            this.btnPing.TabIndex = 46;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // btnExplorer
            // 
            this.btnExplorer.Location = new System.Drawing.Point(494, 6);
            this.btnExplorer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExplorer.Name = "btnExplorer";
            this.btnExplorer.Size = new System.Drawing.Size(80, 34);
            this.btnExplorer.TabIndex = 45;
            this.btnExplorer.Text = "Explorer";
            this.btnExplorer.UseVisualStyleBackColor = true;
            this.btnExplorer.Click += new System.EventHandler(this.btnExplorer_Click);
            // 
            // cbEQPID
            // 
            this.cbEQPID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEQPID.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbEQPID.FormattingEnabled = true;
            this.cbEQPID.Location = new System.Drawing.Point(9, 10);
            this.cbEQPID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbEQPID.Name = "cbEQPID";
            this.cbEQPID.Size = new System.Drawing.Size(386, 24);
            this.cbEQPID.TabIndex = 44;
            this.cbEQPID.SelectedIndexChanged += new System.EventHandler(this.cbEQPID_SelectedIndexChanged);
            // 
            // ckbNonRealtime
            // 
            this.ckbNonRealtime.AutoSize = true;
            this.ckbNonRealtime.Location = new System.Drawing.Point(9, 392);
            this.ckbNonRealtime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbNonRealtime.Name = "ckbNonRealtime";
            this.ckbNonRealtime.Size = new System.Drawing.Size(117, 16);
            this.ckbNonRealtime.TabIndex = 43;
            this.ckbNonRealtime.Text = "UI Non-Realtime";
            this.ckbNonRealtime.UseVisualStyleBackColor = true;
            // 
            // btnCount
            // 
            this.btnCount.Location = new System.Drawing.Point(665, 142);
            this.btnCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(82, 54);
            this.btnCount.TabIndex = 42;
            this.btnCount.Text = "Count";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(476, 175);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(18, 14);
            this.label11.TabIndex = 41;
            this.label11.Text = "~";
            // 
            // tbRangeEnd
            // 
            this.tbRangeEnd.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbRangeEnd.Location = new System.Drawing.Point(499, 172);
            this.tbRangeEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbRangeEnd.Name = "tbRangeEnd";
            this.tbRangeEnd.Size = new System.Drawing.Size(70, 24);
            this.tbRangeEnd.TabIndex = 40;
            this.tbRangeEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRangeStart
            // 
            this.tbRangeStart.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbRangeStart.Location = new System.Drawing.Point(401, 172);
            this.tbRangeStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbRangeStart.Name = "tbRangeStart";
            this.tbRangeStart.Size = new System.Drawing.Size(70, 24);
            this.tbRangeStart.TabIndex = 39;
            this.tbRangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelRange
            // 
            this.labelRange.AutoSize = true;
            this.labelRange.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRange.Location = new System.Drawing.Point(348, 175);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(49, 14);
            this.labelRange.TabIndex = 38;
            this.labelRange.Text = "Range";
            // 
            // tbMessage
            // 
            this.tbMessage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbMessage.Location = new System.Drawing.Point(113, 171);
            this.tbMessage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(231, 26);
            this.tbMessage.TabIndex = 37;
            this.tbMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rbtUnfixed
            // 
            this.rbtUnfixed.AutoSize = true;
            this.rbtUnfixed.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbtUnfixed.Location = new System.Drawing.Point(17, 173);
            this.rbtUnfixed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtUnfixed.Name = "rbtUnfixed";
            this.rbtUnfixed.Size = new System.Drawing.Size(81, 20);
            this.rbtUnfixed.TabIndex = 36;
            this.rbtUnfixed.TabStop = true;
            this.rbtUnfixed.Text = "Unfixed";
            this.rbtUnfixed.UseVisualStyleBackColor = true;
            // 
            // rbtFixed
            // 
            this.rbtFixed.AutoSize = true;
            this.rbtFixed.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbtFixed.Location = new System.Drawing.Point(17, 143);
            this.rbtFixed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtFixed.Name = "rbtFixed";
            this.rbtFixed.Size = new System.Drawing.Size(65, 20);
            this.rbtFixed.TabIndex = 35;
            this.rbtFixed.TabStop = true;
            this.rbtFixed.Text = "Fixed";
            this.rbtFixed.UseVisualStyleBackColor = true;
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.Location = new System.Drawing.Point(132, 370);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(615, 38);
            this.btnExcel.TabIndex = 34;
            this.btnExcel.Text = "EXCEL - Export";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // tbLogItemCount
            // 
            this.tbLogItemCount.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbLogItemCount.Location = new System.Drawing.Point(753, 142);
            this.tbLogItemCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLogItemCount.Multiline = true;
            this.tbLogItemCount.Name = "tbLogItemCount";
            this.tbLogItemCount.Size = new System.Drawing.Size(174, 54);
            this.tbLogItemCount.TabIndex = 33;
            this.tbLogItemCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbMessageSub
            // 
            this.cbMessageSub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMessageSub.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMessageSub.FormattingEnabled = true;
            this.cbMessageSub.Location = new System.Drawing.Point(363, 142);
            this.cbMessageSub.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbMessageSub.Name = "cbMessageSub";
            this.cbMessageSub.Size = new System.Drawing.Size(206, 24);
            this.cbMessageSub.TabIndex = 32;
            // 
            // cbMessage
            // 
            this.cbMessage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMessage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMessage.FormattingEnabled = true;
            this.cbMessage.Location = new System.Drawing.Point(113, 142);
            this.cbMessage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbMessage.Name = "cbMessage";
            this.cbMessage.Size = new System.Drawing.Size(231, 24);
            this.cbMessage.TabIndex = 31;
            this.cbMessage.SelectedIndexChanged += new System.EventHandler(this.cbMessage_SelectedIndexChanged);
            // 
            // btnCimLogAnalysis
            // 
            this.btnCimLogAnalysis.Location = new System.Drawing.Point(753, 47);
            this.btnCimLogAnalysis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCimLogAnalysis.Name = "btnCimLogAnalysis";
            this.btnCimLogAnalysis.Size = new System.Drawing.Size(174, 91);
            this.btnCimLogAnalysis.TabIndex = 30;
            this.btnCimLogAnalysis.Text = "CIM Log Analysis";
            this.btnCimLogAnalysis.UseVisualStyleBackColor = true;
            this.btnCimLogAnalysis.Click += new System.EventHandler(this.btnCimLogAnalysis_Click);
            // 
            // btnSelectLogFile
            // 
            this.btnSelectLogFile.Location = new System.Drawing.Point(753, 6);
            this.btnSelectLogFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectLogFile.Name = "btnSelectLogFile";
            this.btnSelectLogFile.Size = new System.Drawing.Size(174, 34);
            this.btnSelectLogFile.TabIndex = 29;
            this.btnSelectLogFile.Text = "Select Log File";
            this.btnSelectLogFile.UseVisualStyleBackColor = true;
            this.btnSelectLogFile.Click += new System.EventHandler(this.btnSelectLogFile_Click);
            // 
            // tbLogFile
            // 
            this.tbLogFile.Location = new System.Drawing.Point(9, 44);
            this.tbLogFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLogFile.Multiline = true;
            this.tbLogFile.Name = "tbLogFile";
            this.tbLogFile.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogFile.Size = new System.Drawing.Size(738, 94);
            this.tbLogFile.TabIndex = 28;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(753, 371);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(174, 38);
            this.btnClear.TabIndex = 27;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbLogAnalysis
            // 
            this.tbLogAnalysis.Location = new System.Drawing.Point(8, 200);
            this.tbLogAnalysis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLogAnalysis.MaxLength = 100000000;
            this.tbLogAnalysis.Multiline = true;
            this.tbLogAnalysis.Name = "tbLogAnalysis";
            this.tbLogAnalysis.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLogAnalysis.Size = new System.Drawing.Size(919, 166);
            this.tbLogAnalysis.TabIndex = 26;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 446);
            this.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "MainForm";
            this.Text = "MFQ CIM Log Checker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adgvFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvTKAverage)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvTKData)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMemory)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEQSelect;
        private System.Windows.Forms.Label lblEQSelect;
        private System.Windows.Forms.TextBox txtReuslt;
        private System.Windows.Forms.Button btnLogCheck;
        private System.Windows.Forms.TextBox txtLogPath;
        private System.Windows.Forms.Label lblLogPath;
        private System.Windows.Forms.Button btnLogFilePathSet;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkACFCheck;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.CheckBox chkExportToExcelColor;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPW;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnLoginCancel;
        private ADGV.AdvancedDataGridView adgvFilter;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button btnFilerCancel;
        private System.Windows.Forms.ProgressBar prgStatus;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtTKOutLogPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTKOutLogSet;
        private System.Windows.Forms.TextBox txtTKInLogPath;
        private System.Windows.Forms.ProgressBar prgTKStatus;
        private System.Windows.Forms.Button btnTKLogCheck;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTKExportExcel;
        private System.Windows.Forms.Button btnTKInLogSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTKResult;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private ADGV.AdvancedDataGridView adgvTKAverage;
        private System.Windows.Forms.TabPage tabPage4;
        private ADGV.AdvancedDataGridView adgvTKData;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.BindingSource bindingSource3;
        private System.Windows.Forms.ComboBox cmbLogVerSelect;
        private System.Windows.Forms.Label lblLogVer;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtStationNo;
        private System.Windows.Forms.Label lblstation;
        private System.Windows.Forms.Button btnPLCConnect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.DataGridView dgvMemory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Device;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Binary;
        private System.Windows.Forms.DataGridViewTextBoxColumn HEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ASCII;
        private System.Windows.Forms.Button btnPLCSet;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPLCSetValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtModifyDevice;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button btnALLAnalysis;
        private System.Windows.Forms.Button btnRemote;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Button btnExplorer;
        private System.Windows.Forms.ComboBox cbEQPID;
        private System.Windows.Forms.CheckBox ckbNonRealtime;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbRangeEnd;
        private System.Windows.Forms.TextBox tbRangeStart;
        private System.Windows.Forms.Label labelRange;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.RadioButton rbtUnfixed;
        private System.Windows.Forms.RadioButton rbtFixed;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.TextBox tbLogItemCount;
        private System.Windows.Forms.ComboBox cbMessageSub;
        private System.Windows.Forms.ComboBox cbMessage;
        private System.Windows.Forms.Button btnCimLogAnalysis;
        private System.Windows.Forms.Button btnSelectLogFile;
        private System.Windows.Forms.TextBox tbLogFile;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbLogAnalysis;
        private System.Windows.Forms.CheckBox chkFrameCOFCheck;
    }
}

