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
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgGame)).BeginInit();
            this.pnlGameImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.strStatusTitle,
            this.strStatus,
            this.strWaiting});
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
            this.pnlGameImage.Text = "Game";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 335);
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
    }
}

