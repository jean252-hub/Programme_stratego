using System;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon.LogiqueJeu
{
    public class LogiqueClient : IGameLogic
    {
        public event EventHandler<GameStateEventArgs> GameStateUpdated;
        private readonly /* votre réseau client */ object reseauClient;

        public LogiqueClient(/* params */)
        {
            // initialiser reseauClient et abonnement aux messages serveurs
        }

        public async Task StartAsync()
        {
            // se connecter au serveur, envoyer init local, attendre début partie
        }

        public async Task HandleLocalMoveAsync(MoveInfo move)
        {
            // Envoyer le move au serveur; le serveur décidera et renverra l'update.
            // Optionnel: appliquer local optimistic update si désiré.
        }

        public async Task HandleOpponentInitAsync(PlayerInit init)
        {
            // Appliquer init reçue du serveur si nécessaire
        }

        public void Dispose()
        {
            // fermer connexions réseau
        }
    }
}
