using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Stratego_Jean_Gazon
{
    public class MenuESC
    {
        private Panel pnlMenuPause;
        private Panel pnlGrilleGame;
        private Form parentForm;
        private Button btnReprendre;
        private Button btnQuitter;
        private Button btnRecommencer;
        private Button btnValider;


        // Constructeur qui prend le Form et le Panel
        public MenuESC(Form form, Panel panel,Panel Jeu_Enable,Button Reprendre, Button Quitter, Button Recommencer , Button Valider)
        {
            parentForm = form;
            pnlMenuPause = panel;
            pnlGrilleGame = Jeu_Enable;
            btnReprendre = Reprendre;
            btnQuitter = Quitter;
            btnRecommencer = Recommencer;
            btnValider = Valider;

            pnlMenuPause.Visible = false; // Caché par défaut
            btnReprendre.Click += BtnReprendre_Click;
            btnQuitter.Click += BtnQuitter_Click;
            btnRecommencer.Click += BtnRecommencer_Click;

            parentForm.KeyDown += ParentForm_KeyDown;
            parentForm.KeyPreview = true;
        }

        private void ParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            pnlMenuPause.Visible = !pnlMenuPause.Visible;
            pnlMenuPause.BringToFront();
            
            pnlGrilleGame.Enabled= !pnlGrilleGame.Enabled;
            btnValider.Enabled= !btnValider.Enabled;
          
        }
        private void BtnReprendre_Click(object sender, EventArgs e)
        {
            // Logique pour reprendre le jeu
            pnlMenuPause.Visible = false;  // Cache le menu
            pnlGrilleGame.Enabled = true;  // Active le jeu
            btnValider.Enabled = true;
        }

        // Gestionnaire d'événements pour le bouton "Quitter"
        private void BtnQuitter_Click(object sender, EventArgs e)
        {
            FMenu fMenu = new FMenu();
            fMenu.Show();
           parentForm.Close();
        }

        // Gestionnaire d'événements pour le bouton "Recommencer"
        private void BtnRecommencer_Click(object sender, EventArgs e)
        {
            // Logique pour recommencer le jeu
            MessageBox.Show("Le jeu recommence !");
            // Par exemple, réinitialiser le jeu ici
        }


    }
}
