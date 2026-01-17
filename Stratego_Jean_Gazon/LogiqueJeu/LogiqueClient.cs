using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon.LogiqueJeu
{
    public class LogiqueClient
    { 
        private Grille_Manager grille_client;
        public LogiqueClient() {
        }
        public async void Gestion_Operation(Grille_Manager grille)
        {
            grille_client = grille ;
            await Receptionner_Donnees();
        }
        private async Task Receptionner_Donnees()
        {
           await grille_client.ReceptionPositionPions();
        }
        private async Task Envoyer_Donnees()
        {
            await grille_client.EnvoyerPositionsPions();
        }

    }
}
