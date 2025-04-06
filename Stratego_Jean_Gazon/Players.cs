namespace Stratego_Jean_Gazon
{
    public enum Player
    {
        Player_Blue = 1,
        Player_Red = 2,
    }

    public class Players
    {
        public Player CurrentPlayer { get; private set; } // Utilise Player ici

        public Players()
        {
            CurrentPlayer = Player.Player_Blue; // Le joueur Rouge commence
        }

        public void ChangerJoueur()
        {

            CurrentPlayer = (CurrentPlayer == Player.Player_Red) ? Player.Player_Blue : Player.Player_Red;

        }
    }
}
