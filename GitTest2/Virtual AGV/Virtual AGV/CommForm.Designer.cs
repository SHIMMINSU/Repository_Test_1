namespace Virtual_AGV
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWayPoint = new System.Windows.Forms.Label();
            this.btnPosSet = new System.Windows.Forms.Button();
            this.txtWayPoint = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.btnPauseOn = new System.Windows.Forms.Button();
            this.btnResumeOn = new System.Windows.Forms.Button();
            this.btnDestinationOn = new System.Windows.Forms.Button();
            this.btnPauseOff = new System.Windows.Forms.Button();
            this.btnResumeOff = new System.Windows.Forms.Button();
            this.btnDestinationOff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWayPoint
            // 
            this.lblWayPoint.AutoSize = true;
            this.lblWayPoint.BackColor = System.Drawing.Color.Transparent;
            this.lblWayPoint.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWayPoint.ForeColor = System.Drawing.Color.Black;
            this.lblWayPoint.Location = new System.Drawing.Point(0, 8);
            this.lblWayPoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWayPoint.Name = "lblWayPoint";
            this.lblWayPoint.Size = new System.Drawing.Size(90, 12);
            this.lblWayPoint.TabIndex = 1;
            this.lblWayPoint.Text = "PosWayPoint";
            // 
            // btnPosSet
            // 
            this.btnPosSet.Location = new System.Drawing.Point(213, 3);
            this.btnPosSet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPosSet.Name = "btnPosSet";
            this.btnPosSet.Size = new System.Drawing.Size(78, 44);
            this.btnPosSet.TabIndex = 2;
            this.btnPosSet.Text = "PosSet";
            this.btnPosSet.UseVisualStyleBackColor = true;
            this.btnPosSet.Click += new System.EventHandler(this.btnWordComm_Click);
            // 
            // txtWayPoint
            // 
            this.txtWayPoint.Location = new System.Drawing.Point(105, 3);
            this.txtWayPoint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtWayPoint.Name = "txtWayPoint";
            this.txtWayPoint.Size = new System.Drawing.Size(105, 20);
            this.txtWayPoint.TabIndex = 3;
            this.txtWayPoint.Text = "3430";
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(105, 28);
            this.txtDestination.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(105, 20);
            this.txtDestination.TabIndex = 5;
            this.txtDestination.Text = "3430";
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.BackColor = System.Drawing.Color.Transparent;
            this.lblDestination.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDestination.ForeColor = System.Drawing.Color.Black;
            this.lblDestination.Location = new System.Drawing.Point(0, 33);
            this.lblDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(103, 12);
            this.lblDestination.TabIndex = 4;
            this.lblDestination.Text = "PosDestination";
            // 
            // btnPauseOn
            // 
            this.btnPauseOn.Location = new System.Drawing.Point(213, 53);
            this.btnPauseOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPauseOn.Name = "btnPauseOn";
            this.btnPauseOn.Size = new System.Drawing.Size(78, 21);
            this.btnPauseOn.TabIndex = 26;
            this.btnPauseOn.Text = "PauseOn";
            this.btnPauseOn.UseVisualStyleBackColor = true;
            this.btnPauseOn.Click += new System.EventHandler(this.btnPauseOn_Click);
            // 
            // btnResumeOn
            // 
            this.btnResumeOn.Location = new System.Drawing.Point(118, 53);
            this.btnResumeOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnResumeOn.Name = "btnResumeOn";
            this.btnResumeOn.Size = new System.Drawing.Size(90, 21);
            this.btnResumeOn.TabIndex = 25;
            this.btnResumeOn.Text = "ResumeOn";
            this.btnResumeOn.UseVisualStyleBackColor = true;
            this.btnResumeOn.Click += new System.EventHandler(this.btnResumeOn_Click);
            // 
            // btnDestinationOn
            // 
            this.btnDestinationOn.Location = new System.Drawing.Point(2, 53);
            this.btnDestinationOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDestinationOn.Name = "btnDestinationOn";
            this.btnDestinationOn.Size = new System.Drawing.Size(111, 21);
            this.btnDestinationOn.TabIndex = 24;
            this.btnDestinationOn.Text = "DestinationOn";
            this.btnDestinationOn.UseVisualStyleBackColor = true;
            this.btnDestinationOn.Click += new System.EventHandler(this.btnDestinationOn_Click);
            // 
            // btnPauseOff
            // 
            this.btnPauseOff.Location = new System.Drawing.Point(213, 76);
            this.btnPauseOff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPauseOff.Name = "btnPauseOff";
            this.btnPauseOff.Size = new System.Drawing.Size(78, 21);
            this.btnPauseOff.TabIndex = 29;
            this.btnPauseOff.Text = "PauseOff";
            this.btnPauseOff.UseVisualStyleBackColor = true;
            this.btnPauseOff.Click += new System.EventHandler(this.btnPauseOff_Click);
            // 
            // btnResumeOff
            // 
            this.btnResumeOff.Location = new System.Drawing.Point(118, 76);
            this.btnResumeOff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnResumeOff.Name = "btnResumeOff";
            this.btnResumeOff.Size = new System.Drawing.Size(90, 21);
            this.btnResumeOff.TabIndex = 28;
            this.btnResumeOff.Text = "ResumeOff";
            this.btnResumeOff.UseVisualStyleBackColor = true;
            this.btnResumeOff.Click += new System.EventHandler(this.btnResumeOff_Click);
            // 
            // btnDestinationOff
            // 
            this.btnDestinationOff.Location = new System.Drawing.Point(2, 76);
            this.btnDestinationOff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDestinationOff.Name = "btnDestinationOff";
            this.btnDestinationOff.Size = new System.Drawing.Size(111, 21);
            this.btnDestinationOff.TabIndex = 27;
            this.btnDestinationOff.Text = "DestinationOff";
            this.btnDestinationOff.UseVisualStyleBackColor = true;
            this.btnDestinationOff.Click += new System.EventHandler(this.btnDestinationOff_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 100);
            this.Controls.Add(this.btnPauseOff);
            this.Controls.Add(this.btnResumeOff);
            this.Controls.Add(this.btnDestinationOff);
            this.Controls.Add(this.btnPauseOn);
            this.Controls.Add(this.btnResumeOn);
            this.Controls.Add(this.btnDestinationOn);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.txtWayPoint);
            this.Controls.Add(this.btnPosSet);
            this.Controls.Add(this.lblWayPoint);
            this.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWayPoint;
        private System.Windows.Forms.Button btnPosSet;
        private System.Windows.Forms.TextBox txtWayPoint;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Button btnPauseOn;
        private System.Windows.Forms.Button btnResumeOn;
        private System.Windows.Forms.Button btnDestinationOn;
        private System.Windows.Forms.Button btnPauseOff;
        private System.Windows.Forms.Button btnResumeOff;
        private System.Windows.Forms.Button btnDestinationOff;
    }
}