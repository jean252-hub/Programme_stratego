using System;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon.LogiqueJeu
{
    public class LogiqueServeur : IGameLogic
    {
        public event EventHandler<GameStateEventArgs> GameStateUpdated;
        private readonly /* votre réseau serveur */ object reseauServeur;

        public LogiqueServeur(/* paramètres si besoin */)
        {
            // initialiser reseauServeur et abonnement aux messages entrants
        }

        public async Task StartAsync()
        {
            // Démarrer écoute réseau, attendre init client, démarrer partie serveur
        }

        public async Task HandleLocalMoveAsync(MoveInfo move)
        {
            // 1) Appliquer la logique de jeu côté serveur (résoudre combat, mettre à jour état)
            // 2) Construire GameStateEventArgs contenant les changements
            var update = new GameStateEventArgs { Moves = new[] { move } };
            // 3) Notifier UI local
            GameStateUpdated?.Invoke(this, update);
            // 4) Envoyer l'update au client via réseau
        }

        public async Task HandleOpponentInitAsync(PlayerInit init)
        {
            // Traiter initialisation envoyée par client
        }

        public void Dispose()
        {
            // fermer connexions réseau
        }
    }
}
