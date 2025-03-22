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
            this.ptLac2 = new System.Windows.Forms.PictureBox();
            this.ptLac1 = new System.Windows.Forms.PictureBox();
            this.PnlGrilleGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac1)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlGrilleGame
            // 
            this.PnlGrilleGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlGrilleGame.BackColor = System.Drawing.Color.Bisque;
            this.PnlGrilleGame.Controls.Add(this.ptLac2);
            this.PnlGrilleGame.Controls.Add(this.ptLac1);
            this.PnlGrilleGame.Location = new System.Drawing.Point(116, 62);
            this.PnlGrilleGame.Name = "PnlGrilleGame";
            this.PnlGrilleGame.Size = new System.Drawing.Size(711, 371);
            this.PnlGrilleGame.TabIndex = 0;
            this.PnlGrilleGame.SizeChanged += new System.EventHandler(this.PnlGrilleGame_SizeChanged);
            this.PnlGrilleGame.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlGrilleGame_Paint);
            this.PnlGrilleGame.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PnlGrilleGame_MouseDown);
            // 
            // ptLac2
            // 
            this.ptLac2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ptLac2.BackgroundImage = global::Stratego_Jean_Gazon.Properties.Resources.images__2_;
            this.ptLac2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptLac2.Location = new System.Drawing.Point(466, 164);
            this.ptLac2.Name = "ptLac2";
            this.ptLac2.Size = new System.Drawing.Size(110, 72);
            this.ptLac2.TabIndex = 1;
            this.ptLac2.TabStop = false;
            // 
            // ptLac1
            // 
            this.ptLac1.BackgroundImage = global::Stratego_Jean_Gazon.Properties.Resources._17674983_etang_dans_la_nature_dans_un_style_plat_de_dessin_anime_petit_lac_de_foret_dans_le_paysage_de_montagne_vectoriel;
            this.ptLac1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptLac1.Location = new System.Drawing.Point(147, 164);
            this.ptLac1.Name = "ptLac1";
            this.ptLac1.Size = new System.Drawing.Size(135, 72);
            this.ptLac1.TabIndex = 0;
            this.ptLac1.TabStop = false;
            // 
            // FicJeu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 489);
            this.Controls.Add(this.PnlGrilleGame);
            this.Name = "FicJeu";
            this.Text = "FicJeu";
            this.Load += new System.EventHandler(this.FicJeu_Load);
            this.PnlGrilleGame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptLac2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptLac1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlGrilleGame;
        private System.Windows.Forms.PictureBox ptLac2;
        private System.Windows.Forms.PictureBox ptLac1;
    }
}