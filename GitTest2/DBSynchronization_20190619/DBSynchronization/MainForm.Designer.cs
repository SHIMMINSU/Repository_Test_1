namespace DBSynchronization
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tmrDbCopy = new System.Windows.Forms.Timer(this.components);
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.openIniCtrlOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_창 = new System.Windows.Forms.ToolStripMenuItem();
            this.LogClaenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tvwTables = new System.Windows.Forms.TreeView();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mnsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrDbCopy
            // 
            this.tmrDbCopy.Tick += new System.EventHandler(this.tmrDbCopy_Tick);
            // 
            // mnsMain
            // 
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_File,
            this.menu_창});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.mnsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mnsMain.Size = new System.Drawing.Size(658, 25);
            this.mnsMain.TabIndex = 2;
            this.mnsMain.Text = "menuStrip1";
            // 
            // menu_File
            // 
            this.menu_File.BackColor = System.Drawing.Color.White;
            this.menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openIniCtrlOToolStripMenuItem,
            this.viewLogToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new System.Drawing.Size(57, 19);
            this.menu_File.Text = "파일(&F)";
            // 
            // openIniCtrlOToolStripMenuItem
            // 
            this.openIniCtrlOToolStripMenuItem.Name = "openIniCtrlOToolStripMenuItem";
            this.openIniCtrlOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openIniCtrlOToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openIniCtrlOToolStripMenuItem.Text = "&Open ini";
            this.openIniCtrlOToolStripMenuItem.Click += new System.EventHandler(this.openIniCtrlOToolStripMenuItem_Click);
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.viewLogToolStripMenuItem.Text = "&View log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menu_창
            // 
            this.menu_창.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LogClaenToolStripMenuItem});
            this.menu_창.Name = "menu_창";
            this.menu_창.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.menu_창.Size = new System.Drawing.Size(50, 19);
            this.menu_창.Text = "창(&W)";
            // 
            // LogClaenToolStripMenuItem
            // 
            this.LogClaenToolStripMenuItem.Name = "LogClaenToolStripMenuItem";
            this.LogClaenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogClaenToolStripMenuItem.Text = "Log Clean";
            this.LogClaenToolStripMenuItem.Click += new System.EventHandler(this.LogClaenToolStripMenuItem_Click);
            // 
            // tvwTables
            // 
            this.tvwTables.BackColor = System.Drawing.Color.White;
            this.tvwTables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvwTables.Cursor = System.Windows.Forms.Cursors.Default;
            this.tvwTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwTables.Location = new System.Drawing.Point(0, 0);
            this.tvwTables.Margin = new System.Windows.Forms.Padding(1);
            this.tvwTables.Name = "tvwTables";
            this.tvwTables.Size = new System.Drawing.Size(253, 374);
            this.tvwTables.TabIndex = 1;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.White;
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(401, 374);
            this.rtbLog.TabIndex = 10;
            this.rtbLog.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvwTables);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtbLog);
            this.splitContainer1.Size = new System.Drawing.Size(658, 374);
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(658, 399);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mnsMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnsMain;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "테이블 동기화 프로그램";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrDbCopy;
        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem menu_File;
        private System.Windows.Forms.ToolStripMenuItem openIniCtrlOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu_창;
        private System.Windows.Forms.ToolStripMenuItem LogClaenToolStripMenuItem;
        private System.Windows.Forms.TreeView tvwTables;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

