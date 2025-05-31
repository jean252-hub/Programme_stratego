using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Stratego_Jean_Gazon.Stratego_Jean_Gazon;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu : Form
    {
        public GameTransitionManager transitionManager;

        private Players player;
        private MenuESC menuEsc;
        private Grille_Manager grille_manager;
        private Grille_GameEngine grilleGameEngine;
        private Other Other_option;
        private PictureBox pionSelectionne = null;
        private bool action_joue = false;
        public ImageList ImageListPions => ImgListPerso; // pointer vers la list d'images des pions
        public FicJeu()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            grille_manager = new Grille_Manager(PnlGrilleGame, pnlMenuPause, ptLac1, ptLac2, ImgListPerso); // grille manager gère tout l'aspect visuel de la grille
            grilleGameEngine = new Grille_GameEngine(); // grilleGameEngine gère la logique du jeu
            menuEsc = new MenuESC(this, pnlMenuPause, PnlGrilleGame, btnReprendre, btnJeuQuitter, pnlPausebtnrecommencer, btnValider);
            pnlMenuPause.Parent = this;
            transitionManager = new GameTransitionManager(this, Properties.Resources.Image_Transition); // transitionManager gère les transitions du jeu (placement, changement de joueur, combat)
            // adaptation de la grille manager à la taille de la fenêtre
            grille_manager.CenterPanel(this.ClientSize.Width, this.ClientSize.Height);
            grille_manager.RecalculerTaillesEtPositions();
            player = new Players();
            Other_option = new Other(btnValider, this, Btn_Pret);
            PositionnerTitreFenetre();
        }
        private async void FicJeu_Load(object sender, EventArgs e) // les méthodes async permet de ne pas bloquer l'interface pendant que les transition sont appélée pour rendre le jeu plus conviviale async est obligatoire quand on utilise await 
        {
            Debug_config();
            grille_manager.Piece_Init();
            Other_option.bValider_position();
            Btn_Pret.Visible = false;
            player.Initialisation_Jeu(grille_manager, Btn_Pret, this);
            await transitionManager.ShowPlacement(Player.Player_Blue);// await permet de ne pas bloquer l'interface pendant que la transition est en cours
        }

        private Image GetPionImageFromImageList(string nom) // sert à récupérer l'image d'un pion dans la liste d'images ImgListPerso
        {
            if (ImgListPerso.Images.ContainsKey(nom)) // l'image est recuperee grace a son nom les image de l'images listes et celle donnés au personnage on la même orthographes 
                return ImgListPerso.Images[nom];

            foreach (string key in ImgListPerso.Images.Keys)// permet de retrouver les images même si il y a une erreur dans l'ortographe entre l'image et le nom du personnage
            {
                if (string.Equals(key, nom, StringComparison.InvariantCultureIgnoreCase))
                    return ImgListPerso.Images[key];
            }
            return null;
        }

        private void Debug_config()
        {
            string logFilePath = "log.txt";
            File.WriteAllText(logFilePath, string.Empty);
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new TextWriterTraceListener(logFilePath));
            Debug.AutoFlush = true;
            Debug.WriteLine("Nouveau log démarré...");
            Debug.WriteLine($"Timestamp: {DateTime.Now}");
        }

        public Player GetJoueurActif()
        {
            return player.CurrentPlayer;
        }

        public void PnlGrilleGame_Paint(object sender, PaintEventArgs e)
        {
            grille_manager.PnlGrilleGame_Dessine(sender, e);
        }

        private void PnlGrilleGame_SizeChanged(object sender, EventArgs e)
        {
            this.UpdateStyles();
            if (grille_manager != null)
            {
                grille_manager.CenterPanel(this.ClientSize.Width, this.ClientSize.Height);
                grille_manager.RecalculerTaillesEtPositions();
                grille_manager.Piece_Rezise(true);
            }
        }

        

        private void btnValider_Click(object sender, EventArgs e)
        {
            grille_manager.RecalculerTaillesEtPositions();
            var (largeurCase, hauteurCase, _, _) = grille_manager.GetTaillesEtPositions();
            Initialisation_Pion.PositionnerTousLesPions(PnlGrilleGame, largeurCase, hauteurCase);
            Btn_Pret.Visible = false;
            Player_Game();
        }

        private async void Player_Game()
        {
            action_joue = false;
            await transitionManager.ShowChangeTurn(player.CurrentPlayer);

            grille_manager.TerminerPlacement();
            player.ChangerJoueur();
            grille_manager.Player_Grille_Change(player.CurrentPlayer);
            ActiverModeJeuPourJoueur(player.CurrentPlayer);
        }

        private void ActiverModeJeuPourJoueur(Player joueur)
        {
            foreach (Control ctrl in grille_manager.PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    pb.Click -= Pion_Jeu_Click;
                    if ((joueur == Player.Player_Blue && pion.Couleur) ||
                        (joueur == Player.Player_Red && !pion.Couleur))
                    {
                        pb.Click += Pion_Jeu_Click;
                    }
                }
            }
        }

        private void Pion_Jeu_Click(object sender, EventArgs e)
        {
            if (action_joue) return;

            var clickedPiece = sender as PictureBox;
            if (clickedPiece == null) return;

            var clickedInfo = clickedPiece.Tag as personnage_base;
            if (clickedInfo == null) return;

            if (pionSelectionne == null)
            {
                pionSelectionne = clickedPiece;
                pionSelectionne.BackColor = Color.Yellow;
            }
            else
            {
                var selectedInfo = pionSelectionne.Tag as personnage_base;
                if (pionSelectionne == clickedPiece)
                {
                    pionSelectionne.BackColor = clickedInfo.Couleur ? Color.LightBlue : Color.LightCoral;
                    pionSelectionne = null;
                    return;
                }
                pionSelectionne.BackColor = selectedInfo.Couleur ? Color.LightBlue : Color.LightCoral;
                pionSelectionne = null;
            }
        }

        private async void PnlGrilleGame_MouseDown(object sender, MouseEventArgs e)
        {
            if (action_joue) return;
            if (pionSelectionne == null) return;
            var info = pionSelectionne.Tag as personnage_base;
            if (info == null) return;

            grille_manager.RecalculerTaillesEtPositions();
            var (largeurCase, hauteurCase, _, _) = grille_manager.GetTaillesEtPositions();
            int col = e.X / largeurCase + 1;
            int row = e.Y / hauteurCase + 1;
            Point destination = new Point(col, row);

            if (info.PositionGrille.Equals(destination))
            {
                pionSelectionne.BackColor = info.Couleur ? Color.LightBlue : Color.LightCoral;
                pionSelectionne = null;
                return;
            }

            if (info.Grade == "Bombe" || info.Grade == "Drapeau")
                return;

            bool deplacementValide = false;

            if (info.Grade == "Éclaireur")
            {
                if (info.PositionGrille.X == destination.X && info.PositionGrille.Y != destination.Y)
                {
                    int minY = Math.Min(info.PositionGrille.Y, destination.Y);
                    int maxY = Math.Max(info.PositionGrille.Y, destination.Y);
                    deplacementValide = true;
                    for (int y = minY + 1; y < maxY; y++)
                    {
                        if (CaseOccupee(new Point(col, y)))
                        {
                            deplacementValide = false;
                            break;
                        }
                    }
                }
                else if (info.PositionGrille.Y == destination.Y && info.PositionGrille.X != destination.X)
                {
                    int minX = Math.Min(info.PositionGrille.X, destination.X);
                    int maxX = Math.Max(info.PositionGrille.X, destination.X);
                    deplacementValide = true;
                    for (int x = minX + 1; x < maxX; x++)
                    {
                        if (CaseOccupee(new Point(x, row)))
                        {
                            deplacementValide = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                int dx = Math.Abs(info.PositionGrille.X - destination.X);
                int dy = Math.Abs(info.PositionGrille.Y - destination.Y);
                deplacementValide = ((dx == 1 && dy == 0) || (dx == 0 && dy == 1));
            }

            if (!deplacementValide)
                return;

            PictureBox pbCible = null;
            personnage_base cibleInfo = null;
            foreach (Control ctrl in grille_manager.PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base ci)
                {
                    if (ci.PositionGrille.Equals(destination))
                    {
                        pbCible = pb;
                        cibleInfo = ci;
                        break;
                    }
                }
            }

            if (pbCible != null)
            {
                if (cibleInfo.Couleur == info.Couleur)
                {
                    MessageBox.Show("Case occupée par un de vos pions !");
                }
                else
                {
                    byte victoire = grilleGameEngine.ResoudreAffrontement(info, cibleInfo);


                    if (victoire == 1)
                    {
                        await transitionManager.ShowCombat(
                            GetPionImageFromImageList(info.Grade), info.Couleur,
                            GetPionImageFromImageList(cibleInfo.Grade), cibleInfo.Couleur,
                            "Gagné"
                        );
                        grille_manager.PnlGrilleGame.Controls.Remove(pbCible);
                        grille_manager.SupprimerPion(cibleInfo.PositionGrille, cibleInfo.Couleur);
                        grille_manager.DeplacerPion(info, pionSelectionne, destination);
                    }
                    if (victoire == 2)
                    {
                        await transitionManager.ShowCombat(
                            GetPionImageFromImageList(info.Grade), info.Couleur,
                            GetPionImageFromImageList(cibleInfo.Grade), cibleInfo.Couleur,
                            "Perdu"
                        );
                        grille_manager.PnlGrilleGame.Controls.Remove(pionSelectionne);
                        grille_manager.SupprimerPion(info.PositionGrille, info.Couleur);
                    }
                    if (victoire == 3)
                    {
                        await transitionManager.ShowCombat(
                            GetPionImageFromImageList(info.Grade), info.Couleur,
                            GetPionImageFromImageList(cibleInfo.Grade), cibleInfo.Couleur,
                            "Égalité"
                        );
                        grille_manager.PnlGrilleGame.Controls.Remove(pbCible);
                        grille_manager.PnlGrilleGame.Controls.Remove(pionSelectionne);
                        grille_manager.SupprimerPion(cibleInfo.PositionGrille, cibleInfo.Couleur);
                        grille_manager.SupprimerPion(info.PositionGrille, info.Couleur);
                        MessageBox.Show("Égalité ! Les deux pions sont retirés.");
                    }

                    if (victoire == 4)
                    {
                        AfficherFinPartie(player.CurrentPlayer);
                    }
                }
                pionSelectionne = null;
                action_joue = true;
                return;
            }

            grille_manager.DeplacerPion(info, pionSelectionne, destination);

            pionSelectionne = null;
            action_joue = true;
        }

        private void AfficherFinPartie(Player gagnant)
        {
            pnlFinPartie.Location = new Point(
                (this.ClientSize.Width - pnlFinPartie.Width) / 2,
                (this.ClientSize.Height - pnlFinPartie.Height) / 2
            );
            btnRetourMenu.Location = new Point((pnlFinPartie.Width - btnRetourMenu.Width) / 2, 210);
            this.picCoupe.Location = new Point(
                (this.pnlFinPartie.Width - this.picCoupe.Width) / 2,
                70
            );
            pnlFinPartie.Visible = true;
            pnlFinPartie.BringToFront();
            lblFinPartie.Text = $"Victoire {(gagnant == Player.Player_Blue ? "Bleue" : "Rouge")} !";
            lblFinPartie.ForeColor = Color.Yellow;
        }

        private void PositionnerTitreFenetre()
        {
            int titreX = this.ClientSize.Width / 2 - Titre_Fenetre.Width / 2;
            int titreY = 3;
            Titre_Fenetre.Location = new Point(titreX, titreY);
        }

        private bool CaseOccupee(Point pos)
        {
            foreach (Control ctrl in grille_manager.PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    if (pion.PositionGrille.Equals(pos))
                        return true;
                }
            }
            return false;
        }

        private void BtnRetourMenu_Click(object sender, EventArgs e)
        {
            FermerLog();
            this.Close();
        }

        private void FermerLog()
        {
            foreach (TraceListener listener in Debug.Listeners)
            {
                listener.Flush();
                listener.Close();
            }
            Debug.Listeners.Clear();
        }

        private void FicJeu_SizeChanged(object sender, EventArgs e)
        {
            if (Other_option != null)
            {
                Other_option.bValider_position();
            }

            if (grille_manager != null)
            {
                grille_manager.RecalculerTaillesEtPositions();
            }

            PositionnerTitreFenetre();
        }

        private void Btn_Pret_Click(object sender, EventArgs e)
        {
        }
    }
}
