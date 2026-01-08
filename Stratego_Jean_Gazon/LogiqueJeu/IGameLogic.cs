using System;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon.LogiqueJeu
{
    public class MoveInfo
    {
        public int FromCol, FromRow;
        public int ToCol, ToRow;
        public string Grade;
        public bool OwnerIsBlue;
    }

    public class PlayerInit
    {
        // Remplir avec les données d'initialisation nécessaires
    }

    public class GameStateEventArgs : EventArgs
    {
        // Contient les informations que l'UI doit appliquer
        public MoveInfo[] Moves { get; set; }
        // Ajouter d'autres champs : pions supprimés, placements initiaux, fin de partie, etc.
    }

    public interface IGameLogic : IDisposable
    {
        event EventHandler<GameStateEventArgs> GameStateUpdated;
        Task StartAsync();
        Task HandleLocalMoveAsync(MoveInfo move);
        Task HandleOpponentInitAsync(PlayerInit init);
    }
}