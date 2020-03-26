namespace CIM_Log_Analysis
{
    partial class frmCimLogAnalysis
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
            this.tbLogAnalysis = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbLogFile = new System.Windows.Forms.TextBox();
            this.btnSelectLogFile = new System.Windows.Forms.Button();
            this.btnCimLogAnalysis = new System.Windows.Forms.Button();
            this.cbMessage = new System.Windows.Forms.ComboBox();
            this.cbMessageSub = new System.Windows.Forms.ComboBox();
            this.tbLogItemCount = new System.Windows.Forms.TextBox();
            this.btnExcel = new System.Windows.Forms.Button();
            this.rbtFixed = new System.Windows.Forms.RadioButton();
            this.rbtUnfixed = new System.Windows.Forms.RadioButton();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.labelRange = new System.Windows.Forms.Label();
            this.tbRangeStart = new System.Windows.Forms.TextBox();
            this.tbRangeEnd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCount = new System.Windows.Forms.Button();
            this.ckbNonRealtime = new System.Windows.Forms.CheckBox();
            this.cbEQPID = new System.Windows.Forms.ComboBox();
            this.btnExplorer = new System.Windows.Forms.Button();
            this.btnPing = new System.Windows.Forms.Button();
            this.btnRemote = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbLogAnalysis
            // 
            this.tbLogAnalysis.Location = new System.Drawing.Point(12, 270);
            this.tbLogAnalysis.MaxLength = 100000000;
            this.tbLogAnalysis.Multiline = true;
            this.tbLogAnalysis.Name = "tbLogAnalysis";
            this.tbLogAnalysis.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLogAnalysis.Size = new System.Drawing.Size(994, 231);
            this.tbLogAnalysis.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(845, 516);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(161, 43);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbLogFile
            // 
            this.tbLogFile.Location = new System.Drawing.Point(12, 58);
            this.tbLogFile.Multiline = true;
            this.tbLogFile.Name = "tbLogFile";
            this.tbLogFile.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogFile.Size = new System.Drawing.Size(794, 117);
            this.tbLogFile.TabIndex = 4;
            // 
            // btnSelectLogFile
            // 
            this.btnSelectLogFile.Location = new System.Drawing.Point(822, 10);
            this.btnSelectLogFile.Name = "btnSelectLogFile";
            this.btnSelectLogFile.Size = new System.Drawing.Size(184, 43);
            this.btnSelectLogFile.TabIndex = 5;
            this.btnSelectLogFile.Text = "Select Log File";
            this.btnSelectLogFile.UseVisualStyleBackColor = true;
            this.btnSelectLogFile.Click += new System.EventHandler(this.btnSelectLogFile_Click);
            // 
            // btnCimLogAnalysis
            // 
            this.btnCimLogAnalysis.Location = new System.Drawing.Point(822, 61);
            this.btnCimLogAnalysis.Name = "btnCimLogAnalysis";
            this.btnCimLogAnalysis.Size = new System.Drawing.Size(184, 114);
            this.btnCimLogAnalysis.TabIndex = 6;
            this.btnCimLogAnalysis.Text = "CIM Log Analysis";
            this.btnCimLogAnalysis.UseVisualStyleBackColor = true;
            this.btnCimLogAnalysis.Click += new System.EventHandler(this.btnCimLogAnalysis_Click);
            // 
            // cbMessage
            // 
            this.cbMessage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMessage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMessage.FormattingEnabled = true;
            this.cbMessage.Location = new System.Drawing.Point(130, 190);
            this.cbMessage.Name = "cbMessage";
            this.cbMessage.Size = new System.Drawing.Size(263, 28);
            this.cbMessage.TabIndex = 8;
            this.cbMessage.SelectedIndexChanged += new System.EventHandler(this.cbMessage_SelectedIndexChanged);
            // 
            // cbMessageSub
            // 
            this.cbMessageSub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMessageSub.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMessageSub.FormattingEnabled = true;
            this.cbMessageSub.Location = new System.Drawing.Point(416, 190);
            this.cbMessageSub.Name = "cbMessageSub";
            this.cbMessageSub.Size = new System.Drawing.Size(235, 28);
            this.cbMessageSub.TabIndex = 9;
            // 
            // tbLogItemCount
            // 
            this.tbLogItemCount.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbLogItemCount.Location = new System.Drawing.Point(822, 202);
            this.tbLogItemCount.Name = "tbLogItemCount";
            this.tbLogItemCount.Size = new System.Drawing.Size(184, 45);
            this.tbLogItemCount.TabIndex = 10;
            this.tbLogItemCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.Location = new System.Drawing.Point(352, 516);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(414, 43);
            this.btnExcel.TabIndex = 11;
            this.btnExcel.Text = "EXCEL - Export";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // rbtFixed
            // 
            this.rbtFixed.AutoSize = true;
            this.rbtFixed.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbtFixed.Location = new System.Drawing.Point(21, 191);
            this.rbtFixed.Name = "rbtFixed";
            this.rbtFixed.Size = new System.Drawing.Size(75, 24);
            this.rbtFixed.TabIndex = 13;
            this.rbtFixed.TabStop = true;
            this.rbtFixed.Text = "Fixed";
            this.rbtFixed.UseVisualStyleBackColor = true;
            this.rbtFixed.CheckedChanged += new System.EventHandler(this.rbtFixed_CheckedChanged);
            // 
            // rbtUnfixed
            // 
            this.rbtUnfixed.AutoSize = true;
            this.rbtUnfixed.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbtUnfixed.Location = new System.Drawing.Point(21, 229);
            this.rbtUnfixed.Name = "rbtUnfixed";
            this.rbtUnfixed.Size = new System.Drawing.Size(94, 24);
            this.rbtUnfixed.TabIndex = 14;
            this.rbtUnfixed.TabStop = true;
            this.rbtUnfixed.Text = "Unfixed";
            this.rbtUnfixed.UseVisualStyleBackColor = true;
            this.rbtUnfixed.CheckedChanged += new System.EventHandler(this.rbtUnfixed_CheckedChanged);
            // 
            // tbMessage
            // 
            this.tbMessage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbMessage.Location = new System.Drawing.Point(130, 226);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(263, 30);
            this.tbMessage.TabIndex = 15;
            this.tbMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelRange
            // 
            this.labelRange.AutoSize = true;
            this.labelRange.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRange.Location = new System.Drawing.Point(399, 231);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(54, 17);
            this.labelRange.TabIndex = 16;
            this.labelRange.Text = "Range";
            // 
            // tbRangeStart
            // 
            this.tbRangeStart.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbRangeStart.Location = new System.Drawing.Point(459, 228);
            this.tbRangeStart.Name = "tbRangeStart";
            this.tbRangeStart.Size = new System.Drawing.Size(80, 28);
            this.tbRangeStart.TabIndex = 17;
            this.tbRangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbRangeStart.TextChanged += new System.EventHandler(this.tbRangeStart_TextChanged);
            // 
            // tbRangeEnd
            // 
            this.tbRangeEnd.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbRangeEnd.Location = new System.Drawing.Point(571, 228);
            this.tbRangeEnd.Name = "tbRangeEnd";
            this.tbRangeEnd.Size = new System.Drawing.Size(80, 28);
            this.tbRangeEnd.TabIndex = 18;
            this.tbRangeEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(545, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "~";
            // 
            // btnCount
            // 
            this.btnCount.Location = new System.Drawing.Point(679, 201);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(127, 46);
            this.btnCount.TabIndex = 20;
            this.btnCount.Text = "Count";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // ckbNonRealtime
            // 
            this.ckbNonRealtime.AutoSize = true;
            this.ckbNonRealtime.Location = new System.Drawing.Point(12, 530);
            this.ckbNonRealtime.Name = "ckbNonRealtime";
            this.ckbNonRealtime.Size = new System.Drawing.Size(136, 19);
            this.ckbNonRealtime.TabIndex = 21;
            this.ckbNonRealtime.Text = "UI Non-Realtime";
            this.ckbNonRealtime.UseVisualStyleBackColor = true;
            // 
            // cbEQPID
            // 
            this.cbEQPID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEQPID.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbEQPID.FormattingEnabled = true;
            this.cbEQPID.Location = new System.Drawing.Point(12, 15);
            this.cbEQPID.Name = "cbEQPID";
            this.cbEQPID.Size = new System.Drawing.Size(441, 28);
            this.cbEQPID.TabIndex = 22;
            this.cbEQPID.SelectedIndexChanged += new System.EventHandler(this.cbEQPID_SelectedIndexChanged);
            // 
            // btnExplorer
            // 
            this.btnExplorer.Location = new System.Drawing.Point(486, 15);
            this.btnExplorer.Name = "btnExplorer";
            this.btnExplorer.Size = new System.Drawing.Size(92, 28);
            this.btnExplorer.TabIndex = 23;
            this.btnExplorer.Text = "Explorer";
            this.btnExplorer.UseVisualStyleBackColor = true;
            this.btnExplorer.Click += new System.EventHandler(this.btnExplorer_Click);
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(584, 15);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(92, 28);
            this.btnPing.TabIndex = 24;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // btnRemote
            // 
            this.btnRemote.Location = new System.Drawing.Point(683, 15);
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.Size = new System.Drawing.Size(92, 28);
            this.btnRemote.TabIndex = 25;
            this.btnRemote.Text = "Remote";
            this.btnRemote.UseVisualStyleBackColor = true;
            this.btnRemote.Click += new System.EventHandler(this.btnRemote_Click);
            // 
            // frmCimLogAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 570);
            this.Controls.Add(this.btnRemote);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.btnExplorer);
            this.Controls.Add(this.cbEQPID);
            this.Controls.Add(this.ckbNonRealtime);
            this.Controls.Add(this.btnCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbRangeEnd);
            this.Controls.Add(this.tbRangeStart);
            this.Controls.Add(this.labelRange);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.rbtUnfixed);
            this.Controls.Add(this.rbtFixed);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.tbLogItemCount);
            this.Controls.Add(this.cbMessageSub);
            this.Controls.Add(this.cbMessage);
            this.Controls.Add(this.btnCimLogAnalysis);
            this.Controls.Add(this.btnSelectLogFile);
            this.Controls.Add(this.tbLogFile);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.tbLogAnalysis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCimLogAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CIM Log Analysis";
            this.Load += new System.EventHandler(this.frmCimLogAnalysis_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLogAnalysis;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbLogFile;
        private System.Windows.Forms.Button btnSelectLogFile;
        private System.Windows.Forms.Button btnCimLogAnalysis;
        private System.Windows.Forms.ComboBox cbMessage;
        private System.Windows.Forms.ComboBox cbMessageSub;
        private System.Windows.Forms.TextBox tbLogItemCount;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.RadioButton rbtFixed;
        private System.Windows.Forms.RadioButton rbtUnfixed;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Label labelRange;
        private System.Windows.Forms.TextBox tbRangeStart;
        private System.Windows.Forms.TextBox tbRangeEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.CheckBox ckbNonRealtime;
        private System.Windows.Forms.ComboBox cbEQPID;
        private System.Windows.Forms.Button btnExplorer;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Button btnRemote;
    }
}

