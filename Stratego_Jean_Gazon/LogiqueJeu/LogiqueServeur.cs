using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon.LogiqueJeu
{
    public class LogiqueServeur
    {
        private Grille_Manager grille_serveur;
        public LogiqueServeur()
        {
            
           
        }
        public void Gestion_Operation(Grille_Manager grille)
        {
            grille_serveur = grille ;
            Receptionner_Donnees();
        } 



        private async void Receptionner_Donnees()
        {
           await grille_serveur.ReceptionPositionPions();
        }
        private async void Envoyer_Donnees()
        {
           await grille_serveur.EnvoyerPositionPionsBleu();
        }
    
    }
    
}
