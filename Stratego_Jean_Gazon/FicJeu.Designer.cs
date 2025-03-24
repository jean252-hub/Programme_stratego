namespace Stratego_Jean_Gazon
{
    partial class FicJeu
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
            this.PnlGrilleGame = new System.Windows.Forms.Panel();
            this.pnlMenuPause = new System.Windows.Forms.Panel();
            this.btnReprendre = new System.Windows.Forms.Button();
            this.btnJeuQuitter = new System.Windows.Forms.Button();
            this.pnlPausebtnrecommencer = new System.Windows.Forms.Button();
            this.lbPause = new System.Windows.Forms.Label();
            this.ptLac1 = new System.Windows.Forms.PictureBox();
            this.ptLac2 = new System.Windows.Forms.PictureBox();
            this.btnValider = new System.Windows.Forms.Button();
            this.PnlGrilleGame.SuspendLayout();
            this.pnlMenuPause.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac2)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlGrilleGame
            // 
            this.PnlGrilleGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlGrilleGame.BackColor = System.Drawing.Color.BurlyWood;
            this.PnlGrilleGame.Controls.Add(this.pnlMenuPause);
            this.PnlGrilleGame.Controls.Add(this.ptLac1);
            this.PnlGrilleGame.Controls.Add(this.ptLac2);
            this.PnlGrilleGame.Location = new System.Drawing.Point(116, 62);
            this.PnlGrilleGame.Name = "PnlGrilleGame";
            this.PnlGrilleGame.Size = new System.Drawing.Size(711, 371);
            this.PnlGrilleGame.TabIndex = 0;
            this.PnlGrilleGame.SizeChanged += new System.EventHandler(this.PnlGrilleGame_SizeChanged);
            this.PnlGrilleGame.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlGrilleGame_Paint);
            this.PnlGrilleGame.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PnlGrilleGame_MouseDown);
            // 
            // pnlMenuPause
            // 
            this.pnlMenuPause.BackColor = System.Drawing.Color.Brown;
            this.pnlMenuPause.Controls.Add(this.btnReprendre);
            this.pnlMenuPause.Controls.Add(this.btnJeuQuitter);
            this.pnlMenuPause.Controls.Add(this.pnlPausebtnrecommencer);
            this.pnlMenuPause.Controls.Add(this.lbPause);
            this.pnlMenuPause.Location = new System.Drawing.Point(260, 72);
            this.pnlMenuPause.Name = "pnlMenuPause";
            this.pnlMenuPause.Size = new System.Drawing.Size(302, 264);
            this.pnlMenuPause.TabIndex = 2;
            // 
            // btnReprendre
            // 
            this.btnReprendre.BackColor = System.Drawing.Color.Gold;
            this.btnReprendre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnReprendre.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReprendre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReprendre.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReprendre.Location = new System.Drawing.Point(79, 70);
            this.btnReprendre.Name = "btnReprendre";
            this.btnReprendre.Size = new System.Drawing.Size(147, 34);
            this.btnReprendre.TabIndex = 3;
            this.btnReprendre.Text = "Reprendre";
            this.btnReprendre.UseVisualStyleBackColor = false;
            // 
            // btnJeuQuitter
            // 
            this.btnJeuQuitter.BackColor = System.Drawing.Color.Gold;
            this.btnJeuQuitter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnJeuQuitter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJeuQuitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJeuQuitter.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJeuQuitter.Location = new System.Drawing.Point(79, 188);
            this.btnJeuQuitter.Name = "btnJeuQuitter";
            this.btnJeuQuitter.Size = new System.Drawing.Size(147, 34);
            this.btnJeuQuitter.TabIndex = 2;
            this.btnJeuQuitter.Text = "Quitter";
            this.btnJeuQuitter.UseVisualStyleBackColor = false;
            // 
            // pnlPausebtnrecommencer
            // 
            this.pnlPausebtnrecommencer.BackColor = System.Drawing.Color.Gold;
            this.pnlPausebtnrecommencer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlPausebtnrecommencer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlPausebtnrecommencer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnlPausebtnrecommencer.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlPausebtnrecommencer.Location = new System.Drawing.Point(79, 130);
            this.pnlPausebtnrecommencer.Name = "pnlPausebtnrecommencer";
            this.pnlPausebtnrecommencer.Size = new System.Drawing.Size(147, 34);
            this.pnlPausebtnrecommencer.TabIndex = 1;
            this.pnlPausebtnrecommencer.Text = "Recommencer";
            this.pnlPausebtnrecommencer.UseVisualStyleBackColor = false;
            // 
            // lbPause
            // 
            this.lbPause.AutoSize = true;
            this.lbPause.BackColor = System.Drawing.Color.Gold;
            this.lbPause.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPause.Location = new System.Drawing.Point(121, 22);
            this.lbPause.Name = "lbPause";
            this.lbPause.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbPause.Size = new System.Drawing.Size(65, 25);
            this.lbPause.TabIndex = 0;
            this.lbPause.Text = "Pause ";
            this.lbPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ptLac1
            // 
            this.ptLac1.BackgroundImage = global::Stratego_Jean_Gazon.Properties.Resources.image_etang1Stratego;
            this.ptLac1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptLac1.Location = new System.Drawing.Point(147, 164);
            this.ptLac1.Name = "ptLac1";
            this.ptLac1.Size = new System.Drawing.Size(135, 72);
            this.ptLac1.TabIndex = 0;
            this.ptLac1.TabStop = false;
            // 
            // ptLac2
            // 
            this.ptLac2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ptLac2.BackgroundImage = global::Stratego_Jean_Gazon.Properties.Resources.image_etang2Stratego;
            this.ptLac2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptLac2.Location = new System.Drawing.Point(466, 164);
            this.ptLac2.Name = "ptLac2";
            this.ptLac2.Size = new System.Drawing.Size(110, 72);
            this.ptLac2.TabIndex = 1;
            this.ptLac2.TabStop = false;
            // 
            // btnValider
            // 
            this.btnValider.BackColor = System.Drawing.Color.Gold;
            this.btnValider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnValider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValider.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValider.Location = new System.Drawing.Point(680, 439);
            this.btnValider.Name = "btnValider";
            this.btnValider.Size = new System.Drawing.Size(147, 34);
            this.btnValider.TabIndex = 4;
            this.btnValider.Text = "Valider";
            this.btnValider.UseVisualStyleBackColor = false;
            this.btnValider.Click += new System.EventHandler(this.btnValider_Click);
            // 
            // FicJeu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 489);
            this.Controls.Add(this.btnValider);
            this.Controls.Add(this.PnlGrilleGame);
            this.Name = "FicJeu";
            this.Text = "FicJeu";
            this.Load += new System.EventHandler(this.FicJeu_Load);
            this.PnlGrilleGame.ResumeLayout(false);
            this.pnlMenuPause.ResumeLayout(false);
            this.pnlMenuPause.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlGrilleGame;
        private System.Windows.Forms.PictureBox ptLac2;
        private System.Windows.Forms.PictureBox ptLac1;
        private System.Windows.Forms.Panel pnlMenuPause;
        private System.Windows.Forms.Label lbPause;
        private System.Windows.Forms.Button pnlPausebtnrecommencer;
        private System.Windows.Forms.Button btnJeuQuitter;
        private System.Windows.Forms.Button btnReprendre;
        private System.Windows.Forms.Button btnValider;
    }
}