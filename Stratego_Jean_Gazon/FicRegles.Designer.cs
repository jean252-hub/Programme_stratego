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
            this.rtRegle = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // bRegle
            // 
            this.bRegle.AutoSize = true;
            this.bRegle.BackColor = System.Drawing.Color.Salmon;
            this.bRegle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bRegle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRegle.Location = new System.Drawing.Point(0, 532);
            this.bRegle.Name = "bRegle";
            this.bRegle.Size = new System.Drawing.Size(967, 39);
            this.bRegle.TabIndex = 0;
            this.bRegle.Text = "Compris ! ";
            this.bRegle.UseVisualStyleBackColor = false;
            this.bRegle.Click += new System.EventHandler(this.bRegle_Click);
            // 
            // rtRegle
            // 
            this.rtRegle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rtRegle.BackColor = System.Drawing.Color.Bisque;
            this.rtRegle.Location = new System.Drawing.Point(82, 48);
            this.rtRegle.MaximumSize = new System.Drawing.Size(888, 477);
            this.rtRegle.Name = "rtRegle";
            this.rtRegle.Size = new System.Drawing.Size(795, 407);
            this.rtRegle.TabIndex = 1;
            this.rtRegle.Text = "";
            this.rtRegle.TextChanged += new System.EventHandler(this.rtRegle_TextChanged);
            // 
            // FicRegles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 571);
            this.Controls.Add(this.rtRegle);
            this.Controls.Add(this.bRegle);
            this.Name = "FicRegles";
            this.Text = "Rêgles du jeux";
            this.Load += new System.EventHandler(this.FicRegles_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bRegle;
        private System.Windows.Forms.RichTextBox rtRegle;
    }
}