namespace Tetris
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureGameField = new System.Windows.Forms.PictureBox();
            this.pictureNextFigure = new System.Windows.Forms.PictureBox();
            this.labelScoreText = new System.Windows.Forms.Label();
            this.labelScoreCount = new System.Windows.Forms.Label();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureGameField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNextFigure)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureGameField
            // 
            this.pictureGameField.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureGameField.Location = new System.Drawing.Point(12, 12);
            this.pictureGameField.Name = "pictureGameField";
            this.pictureGameField.Size = new System.Drawing.Size(325, 650);
            this.pictureGameField.TabIndex = 0;
            this.pictureGameField.TabStop = false;
            this.pictureGameField.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureGameField_Paint);
            // 
            // pictureNextFigure
            // 
            this.pictureNextFigure.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureNextFigure.Location = new System.Drawing.Point(386, 12);
            this.pictureNextFigure.Name = "pictureNextFigure";
            this.pictureNextFigure.Size = new System.Drawing.Size(125, 125);
            this.pictureNextFigure.TabIndex = 1;
            this.pictureNextFigure.TabStop = false;
            this.pictureNextFigure.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureNextFigure_Paint);
            // 
            // labelScoreText
            // 
            this.labelScoreText.AutoSize = true;
            this.labelScoreText.Location = new System.Drawing.Point(419, 190);
            this.labelScoreText.Name = "labelScoreText";
            this.labelScoreText.Size = new System.Drawing.Size(49, 20);
            this.labelScoreText.TabIndex = 2;
            this.labelScoreText.Text = "Score:";
            // 
            // labelScoreCount
            // 
            this.labelScoreCount.AutoSize = true;
            this.labelScoreCount.Location = new System.Drawing.Point(419, 233);
            this.labelScoreCount.Name = "labelScoreCount";
            this.labelScoreCount.Size = new System.Drawing.Size(17, 20);
            this.labelScoreCount.TabIndex = 3;
            this.labelScoreCount.Text = "0";
            // 
            // mainTimer
            // 
            this.mainTimer.Enabled = true;
            this.mainTimer.Interval = 1000;
            this.mainTimer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 672);
            this.Controls.Add(this.labelScoreCount);
            this.Controls.Add(this.labelScoreText);
            this.Controls.Add(this.pictureNextFigure);
            this.Controls.Add(this.pictureGameField);
            this.Name = "MainForm";
            this.Text = "Tetris";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureGameField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNextFigure)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureGameField;
        private PictureBox pictureNextFigure;
        private Label labelScoreText;
        private Label labelScoreCount;
        private System.Windows.Forms.Timer mainTimer;
    }
}