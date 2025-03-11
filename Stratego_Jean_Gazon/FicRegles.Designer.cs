namespace Stratego_Jean_Gazon
{
    partial class FicRegles
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
            this.bRegle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bRegle
            // 
            this.bRegle.Location = new System.Drawing.Point(312, 347);
            this.bRegle.Name = "bRegle";
            this.bRegle.Size = new System.Drawing.Size(151, 39);
            this.bRegle.TabIndex = 0;
            this.bRegle.Text = "Compris ! ";
            this.bRegle.UseVisualStyleBackColor = true;
            this.bRegle.Click += new System.EventHandler(this.bRegle_Click);
            // 
            // FicRegles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bRegle);
            this.Name = "FicRegles";
            this.Text = "Rêgles du jeux";
            this.Load += new System.EventHandler(this.FicRegles_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bRegle;
    }
}