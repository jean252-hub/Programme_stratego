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
            this.SuspendLayout();
            // 
            // bjouer
            // 
            this.bjouer.Location = new System.Drawing.Point(322, 241);
            this.bjouer.Name = "bjouer";
            this.bjouer.Size = new System.Drawing.Size(136, 37);
            this.bjouer.TabIndex = 0;
            this.bjouer.Text = "Jouer";
            this.bjouer.UseVisualStyleBackColor = true;
            // 
            // bquitter
            // 
            this.bquitter.Location = new System.Drawing.Point(322, 296);
            this.bquitter.Name = "bquitter";
            this.bquitter.Size = new System.Drawing.Size(136, 37);
            this.bquitter.TabIndex = 1;
            this.bquitter.Text = "quitter";
            this.bquitter.UseVisualStyleBackColor = true;
            // 
            // bregle
            // 
            this.bregle.Location = new System.Drawing.Point(322, 198);
            this.bregle.Name = "bregle";
            this.bregle.Size = new System.Drawing.Size(136, 37);
            this.bregle.TabIndex = 2;
            this.bregle.Text = "Règle du jeu  ";
            this.bregle.UseVisualStyleBackColor = true;
            // 
            // FMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bregle);
            this.Controls.Add(this.bquitter);
            this.Controls.Add(this.bjouer);
            this.Name = "FMenu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bjouer;
        private System.Windows.Forms.Button bquitter;
        private System.Windows.Forms.Button bregle;
    }
}

