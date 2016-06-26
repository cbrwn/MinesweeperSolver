using System.ComponentModel;
using System.Windows.Forms;

namespace MinesweeperSolver {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.strStatusTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.strStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.strWaiting = new System.Windows.Forms.ToolStripProgressBar();
            this.btnSolve = new System.Windows.Forms.Button();
            this.imgGame = new System.Windows.Forms.PictureBox();
            this.pnlGameImage = new System.Windows.Forms.GroupBox();
            this.chkStopWin = new System.Windows.Forms.CheckBox();
            this.tckTries = new System.Windows.Forms.NumericUpDown();
            this.lblMaxTries = new System.Windows.Forms.Label();
            this.chkSaveWin = new System.Windows.Forms.CheckBox();
            this.chkSaveLoss = new System.Windows.Forms.CheckBox();
            this.chkSaveGame = new System.Windows.Forms.CheckBox();
            this.chkSaveBrain = new System.Windows.Forms.CheckBox();
            this.strWinRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.strAvgClear = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgGame)).BeginInit();
            this.pnlGameImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tckTries)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.strStatusTitle,
            this.strStatus,
            this.strWaiting,
            this.strWinRate,
            this.strAvgClear});
            this.statusStrip1.Location = new System.Drawing.Point(0, 313);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(397, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // strStatusTitle
            // 
            this.strStatusTitle.Name = "strStatusTitle";
            this.strStatusTitle.Size = new System.Drawing.Size(42, 17);
            this.strStatusTitle.Text = "Status:";
            // 
            // strStatus
            // 
            this.strStatus.Name = "strStatus";
            this.strStatus.Size = new System.Drawing.Size(26, 17);
            this.strStatus.Text = "Idle";
            // 
            // strWaiting
            // 
            this.strWaiting.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.strWaiting.AutoSize = false;
            this.strWaiting.Name = "strWaiting";
            this.strWaiting.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.strWaiting.Size = new System.Drawing.Size(100, 16);
            this.strWaiting.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.strWaiting.Value = 30;
            this.strWaiting.Visible = false;
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(12, 12);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(73, 23);
            this.btnSolve.TabIndex = 3;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.StartSolve);
            // 
            // imgGame
            // 
            this.imgGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgGame.Location = new System.Drawing.Point(3, 16);
            this.imgGame.Name = "imgGame";
            this.imgGame.Size = new System.Drawing.Size(371, 244);
            this.imgGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgGame.TabIndex = 4;
            this.imgGame.TabStop = false;
            // 
            // pnlGameImage
            // 
            this.pnlGameImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGameImage.Controls.Add(this.imgGame);
            this.pnlGameImage.Location = new System.Drawing.Point(10, 41);
            this.pnlGameImage.Name = "pnlGameImage";
            this.pnlGameImage.Size = new System.Drawing.Size(377, 263);
            this.pnlGameImage.TabIndex = 5;
            this.pnlGameImage.TabStop = false;
            this.pnlGameImage.Text = "Bot Brain";
            // 
            // chkStopWin
            // 
            this.chkStopWin.AutoSize = true;
            this.chkStopWin.Checked = true;
            this.chkStopWin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStopWin.Location = new System.Drawing.Point(91, 4);
            this.chkStopWin.Name = "chkStopWin";
            this.chkStopWin.Size = new System.Drawing.Size(82, 17);
            this.chkStopWin.TabIndex = 6;
            this.chkStopWin.Text = "Stop on win";
            this.chkStopWin.UseVisualStyleBackColor = true;
            // 
            // tckTries
            // 
            this.tckTries.Location = new System.Drawing.Point(163, 23);
            this.tckTries.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.tckTries.Name = "tckTries";
            this.tckTries.Size = new System.Drawing.Size(48, 20);
            this.tckTries.TabIndex = 7;
            // 
            // lblMaxTries
            // 
            this.lblMaxTries.AutoSize = true;
            this.lblMaxTries.Location = new System.Drawing.Point(88, 25);
            this.lblMaxTries.Name = "lblMaxTries";
            this.lblMaxTries.Size = new System.Drawing.Size(73, 13);
            this.lblMaxTries.TabIndex = 8;
            this.lblMaxTries.Text = "Maximum tries";
            // 
            // chkSaveWin
            // 
            this.chkSaveWin.AutoSize = true;
            this.chkSaveWin.Checked = true;
            this.chkSaveWin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveWin.Location = new System.Drawing.Point(172, 4);
            this.chkSaveWin.Name = "chkSaveWin";
            this.chkSaveWin.Size = new System.Drawing.Size(107, 17);
            this.chkSaveWin.TabIndex = 9;
            this.chkSaveWin.Text = "Screenshot Wins";
            this.chkSaveWin.UseVisualStyleBackColor = true;
            // 
            // chkSaveLoss
            // 
            this.chkSaveLoss.AutoSize = true;
            this.chkSaveLoss.Checked = true;
            this.chkSaveLoss.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveLoss.Location = new System.Drawing.Point(280, 4);
            this.chkSaveLoss.Name = "chkSaveLoss";
            this.chkSaveLoss.Size = new System.Drawing.Size(104, 17);
            this.chkSaveLoss.TabIndex = 10;
            this.chkSaveLoss.Text = "Screenshot Fails";
            this.chkSaveLoss.UseVisualStyleBackColor = true;
            // 
            // chkSaveGame
            // 
            this.chkSaveGame.AutoSize = true;
            this.chkSaveGame.Checked = true;
            this.chkSaveGame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveGame.Location = new System.Drawing.Point(217, 24);
            this.chkSaveGame.Name = "chkSaveGame";
            this.chkSaveGame.Size = new System.Drawing.Size(54, 17);
            this.chkSaveGame.TabIndex = 11;
            this.chkSaveGame.Text = "Game";
            this.chkSaveGame.UseVisualStyleBackColor = true;
            // 
            // chkSaveBrain
            // 
            this.chkSaveBrain.AutoSize = true;
            this.chkSaveBrain.Checked = true;
            this.chkSaveBrain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveBrain.Location = new System.Drawing.Point(270, 24);
            this.chkSaveBrain.Name = "chkSaveBrain";
            this.chkSaveBrain.Size = new System.Drawing.Size(50, 17);
            this.chkSaveBrain.TabIndex = 12;
            this.chkSaveBrain.Text = "Brain";
            this.chkSaveBrain.UseVisualStyleBackColor = true;
            // 
            // strWinRate
            // 
            this.strWinRate.Name = "strWinRate";
            this.strWinRate.Size = new System.Drawing.Size(85, 17);
            this.strWinRate.Text = "100% Win Rate";
            // 
            // strAvgClear
            // 
            this.strAvgClear.Name = "strAvgClear";
            this.strAvgClear.Size = new System.Drawing.Size(92, 17);
            this.strAvgClear.Text = "100% Avg. Clear";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 335);
            this.Controls.Add(this.chkSaveBrain);
            this.Controls.Add(this.chkSaveGame);
            this.Controls.Add(this.chkSaveLoss);
            this.Controls.Add(this.chkSaveWin);
            this.Controls.Add(this.lblMaxTries);
            this.Controls.Add(this.tckTries);
            this.Controls.Add(this.chkStopWin);
            this.Controls.Add(this.pnlGameImage);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(413, 374);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Minesweeper Solver";
            this.TopMost = true;
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgGame)).EndInit();
            this.pnlGameImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tckTries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel strStatusTitle;
        private ToolStripStatusLabel strStatus;
        private ToolStripProgressBar strWaiting;
        private Button btnSolve;
        private PictureBox imgGame;
        private GroupBox pnlGameImage;
        private CheckBox chkStopWin;
        private NumericUpDown tckTries;
        private Label lblMaxTries;
        private CheckBox chkSaveWin;
        private CheckBox chkSaveLoss;
        private CheckBox chkSaveGame;
        private CheckBox chkSaveBrain;
        private ToolStripStatusLabel strWinRate;
        private ToolStripStatusLabel strAvgClear;
    }
}

