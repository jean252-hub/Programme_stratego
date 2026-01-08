namespace Stratego_Jean_Gazon
{
    partial class FicSettings
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
            this.btnValidersettings = new System.Windows.Forms.Button();
            this.lbServeur = new System.Windows.Forms.Label();
            this.lbPortSettings = new System.Windows.Forms.Label();
            this.tbAddressIP = new System.Windows.Forms.TextBox();
            this.tbPortServeur = new System.Windows.Forms.TextBox();
            this.IsServeur = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnValidersettings
            // 
            this.btnValidersettings.Location = new System.Drawing.Point(74, 211);
            this.btnValidersettings.Name = "btnValidersettings";
            this.btnValidersettings.Size = new System.Drawing.Size(114, 34);
            this.btnValidersettings.TabIndex = 0;
            this.btnValidersettings.Text = "Valider";
            this.btnValidersettings.UseVisualStyleBackColor = true;
            this.btnValidersettings.Click += new System.EventHandler(this.btnValidersettings_Click);
            // 
            // lbServeur
            // 
            this.lbServeur.AutoSize = true;
            this.lbServeur.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbServeur.Location = new System.Drawing.Point(28, 25);
            this.lbServeur.Name = "lbServeur";
            this.lbServeur.Size = new System.Drawing.Size(160, 20);
            this.lbServeur.TabIndex = 1;
            this.lbServeur.Text = "Adresse du  serveur";
            // 
            // lbPortSettings
            // 
            this.lbPortSettings.AutoSize = true;
            this.lbPortSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPortSettings.Location = new System.Drawing.Point(28, 103);
            this.lbPortSettings.Name = "lbPortSettings";
            this.lbPortSettings.Size = new System.Drawing.Size(129, 20);
            this.lbPortSettings.TabIndex = 2;
            this.lbPortSettings.Text = "Port du serveur ";
            // 
            // tbAddressIP
            // 
            this.tbAddressIP.Location = new System.Drawing.Point(32, 64);
            this.tbAddressIP.Name = "tbAddressIP";
            this.tbAddressIP.Size = new System.Drawing.Size(199, 22);
            this.tbAddressIP.TabIndex = 3;
            // 
            // tbPortServeur
            // 
            this.tbPortServeur.Location = new System.Drawing.Point(32, 142);
            this.tbPortServeur.Name = "tbPortServeur";
            this.tbPortServeur.Size = new System.Drawing.Size(199, 22);
            this.tbPortServeur.TabIndex = 4;
            // 
            // IsServeur
            // 
            this.IsServeur.AutoSize = true;
            this.IsServeur.Location = new System.Drawing.Point(32, 171);
            this.IsServeur.Name = "IsServeur";
            this.IsServeur.Size = new System.Drawing.Size(76, 20);
            this.IsServeur.TabIndex = 5;
            this.IsServeur.Text = "Serveur";
            this.IsServeur.UseVisualStyleBackColor = true;
            this.IsServeur.CheckedChanged += new System.EventHandler(this.IsServeur_CheckedChanged);
            // 
            // FicSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 281);
            this.Controls.Add(this.IsServeur);
            this.Controls.Add(this.tbPortServeur);
            this.Controls.Add(this.tbAddressIP);
            this.Controls.Add(this.lbPortSettings);
            this.Controls.Add(this.lbServeur);
            this.Controls.Add(this.btnValidersettings);
            this.Name = "FicSettings";
            this.Text = "FicSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnValidersettings;
        private System.Windows.Forms.Label lbServeur;
        private System.Windows.Forms.Label lbPortSettings;
        private System.Windows.Forms.TextBox tbAddressIP;
        private System.Windows.Forms.TextBox tbPortServeur;
        private System.Windows.Forms.CheckBox IsServeur;
    }
}