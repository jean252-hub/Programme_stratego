using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public partial class FMenu : Form
    {
        private Timer timerClignotement;
        private bool couleurAlternee = true;
        
        public FMenu()
        {
            InitializeComponent();
            InitialiserEffetClignotant();
        }

        private void bquitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bregle_Click(object sender, EventArgs e)
        {
            FicRegles Regle_jeux = new FicRegles();
            Regle_jeux.ShowDialog(this);
        }
        private void InitialiserEffetClignotant()
        {
            // Création du Timer
            timerClignotement = new Timer();
            timerClignotement.Interval = 500; // Temps en millisecondes (500ms = 0.5 sec)
            timerClignotement.Tick += new EventHandler(ChangerCouleurBouton);
            timerClignotement.Start();
        }

        private void ChangerCouleurBouton(object sender, EventArgs e)
        {
            if (couleurAlternee == true) { 
                bjouer.BackColor = Color.Gold; // Couleur normale
                }
            else {
                bjouer.BackColor = Color.LightSalmon; // Couleur légèrement plus vive
            }
            couleurAlternee = !couleurAlternee; // Alterner la couleur à chaque tick
        }

        private void bjouer_Click(object sender, EventArgs e)
        {
            FicJeu MainPage = new FicJeu();
            MainPage.Show();
            
        }
       
    }

}

