using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.AxHost;
using System.Runtime.InteropServices;
using Stratego_Jean_Gazon.Stratego_Jean_Gazon;

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu : Form
    {
        private Players player;
        private MenuESC menuEsc;
        private Grille_Manager grille_manager;
        private Other Other_option;
        int[] PositionPnlClicG = new int[2];
        int[] positioncase = new int[2];

        private void Debug_config()
        {
            string logFilePath = "log.txt";

            // Réinitialiser le fichier log (vider son contenu sans le supprimer)
            File.WriteAllText(logFilePath, string.Empty);

            // Configuration du Debug Listener
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new TextWriterTraceListener(logFilePath));
            Debug.AutoFlush = true;

            // Exemple de log
            Debug.WriteLine("Nouveau log démarré...");
            Debug.WriteLine($"Timestamp: {DateTime.Now}");

        }

        private const string WINFORM_EVENTS = "WinForm Events";
        public FicJeu()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            grille_manager = new Grille_Manager(PnlGrilleGame, pnlMenuPause, ptLac1, ptLac2,ImgListPerso);
            menuEsc = new MenuESC(this, pnlMenuPause, PnlGrilleGame, btnReprendre, btnJeuQuitter, pnlPausebtnrecommencer ,btnValider);
            pnlMenuPause.Parent = this;
            grille_manager.CenterPanel(this.ClientSize.Width,this.ClientSize.Height);
            player = new Players();
            Other_option = new Other(btnValider, this);
        }
        public Player GetJoueurActif()
        {
            return player.CurrentPlayer; // Retourne directement le joueur actif
        }

        public void PnlGrilleGame_Paint(object sender, PaintEventArgs e)
        {
            grille_manager.PnlGrilleGame_Dessine( sender, e);
        }

        private void PnlGrilleGame_SizeChanged(object sender, EventArgs e)
        {
            this.UpdateStyles();
            if (grille_manager != null)
            {
                grille_manager.Player_Grille_Change(player.CurrentPlayer);
                grille_manager.CenterPanel(this.ClientSize.Width, this.ClientSize.Height);
            }
            else { Debug.WriteLine("grille pas inistialsier"); }


        }

        private void PnlGrilleGame_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PositionPnlClicG[0] = e.X / (PnlGrilleGame.Width / 10) + 1;
                PositionPnlClicG[1] = e.Y / (PnlGrilleGame.Height / 10) + 1;
                     
            }
        }

        private void FicJeu_Load(object sender, EventArgs e)
        {
            Debug_config();
            grille_manager.Piece_Init();
            Other_option.bValider_position();


        }


        private void btnValider_Click(object sender, EventArgs e)
        {
            Player_Game();
           
        }
        
        private void Player_Game()
        {
            player.ChangerJoueur();
            grille_manager.Player_Grille_Change(player.CurrentPlayer);
            
            
        }

        private void FicJeu_SizeChanged(object sender, EventArgs e)
        {
            if (Other_option != null)
            {
                Other_option.bValider_position();
            }
        }
    }
}
