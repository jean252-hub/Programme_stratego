namespace Stratego_Jean_Gazon
{
    partial class FMenu
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.bjouer = new System.Windows.Forms.Button();
            this.bquitter = new System.Windows.Forms.Button();
            this.bregle = new System.Windows.Forms.Button();
            this.FicDemarageTitle = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.FicDemarageTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bjouer
            // 
            this.bjouer.BackColor = System.Drawing.Color.LightSalmon;
            this.bjouer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bjouer.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bjouer.Location = new System.Drawing.Point(257, 555);
            this.bjouer.Name = "bjouer";
            this.bjouer.Size = new System.Drawing.Size(140, 47);
            this.bjouer.TabIndex = 0;
            this.bjouer.Text = "Jouer";
            this.bjouer.UseVisualStyleBackColor = false;
            this.bjouer.Click += new System.EventHandler(this.bjouer_Click);
            // 
            // bquitter
            // 
            this.bquitter.BackColor = System.Drawing.Color.LightSalmon;
            this.bquitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bquitter.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bquitter.ForeColor = System.Drawing.Color.Black;
            this.bquitter.Location = new System.Drawing.Point(475, 555);
            this.bquitter.Name = "bquitter";
            this.bquitter.Size = new System.Drawing.Size(140, 47);
            this.bquitter.TabIndex = 1;
            this.bquitter.Text = "quitter";
            this.bquitter.UseVisualStyleBackColor = false;
            this.bquitter.Click += new System.EventHandler(this.bquitter_Click);
            // 
            // bregle
            // 
            this.bregle.BackColor = System.Drawing.Color.LightSalmon;
            this.bregle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bregle.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bregle.Location = new System.Drawing.Point(52, 555);
            this.bregle.Name = "bregle";
            this.bregle.Size = new System.Drawing.Size(140, 47);
            this.bregle.TabIndex = 2;
            this.bregle.Text = "Règle du jeu  ";
            this.bregle.UseVisualStyleBackColor = false;
            this.bregle.Click += new System.EventHandler(this.bregle_Click);
            // 
            // FicDemarageTitle
            // 
            this.FicDemarageTitle.BackColor = System.Drawing.Color.Maroon;
            this.FicDemarageTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.FicDemarageTitle.Image = global::Stratego_Jean_Gazon.Properties.Resources.titlestratego;
            this.FicDemarageTitle.Location = new System.Drawing.Point(52, 12);
            this.FicDemarageTitle.Name = "FicDemarageTitle";
            this.FicDemarageTitle.Size = new System.Drawing.Size(581, 149);
            this.FicDemarageTitle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FicDemarageTitle.TabIndex = 3;
            this.FicDemarageTitle.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Stratego_Jean_Gazon.Properties.Resources.image__1_;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(-60, -56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(792, 831);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // FMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.IndianRed;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(668, 667);
            this.Controls.Add(this.bregle);
            this.Controls.Add(this.bquitter);
            this.Controls.Add(this.bjouer);
            this.Controls.Add(this.FicDemarageTitle);
            this.Controls.Add(this.pictureBox1);
            this.MaximumSize = new System.Drawing.Size(686, 714);
            this.MinimumSize = new System.Drawing.Size(686, 714);
            this.Name = "FMenu";
            this.Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)(this.FicDemarageTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bjouer;
        private System.Windows.Forms.Button bquitter;
        private System.Windows.Forms.Button bregle;
        private System.Windows.Forms.PictureBox FicDemarageTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

