using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Stratego_Jean_Gazon.Stratego_Jean_Gazon;

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu : Form
    {
        private Players player;
        private MenuESC menuEsc;
        private Grille_Manager grille_manager;
        private Other Other_option;
        private PictureBox pionSelectionne = null;
        private bool action_joue = false;

        public FicJeu()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            grille_manager = new Grille_Manager(PnlGrilleGame, pnlMenuPause, ptLac1, ptLac2, ImgListPerso);
            menuEsc = new MenuESC(this, pnlMenuPause, PnlGrilleGame, btnReprendre, btnJeuQuitter, pnlPausebtnrecommencer, btnValider);
            pnlMenuPause.Parent = this;
            grille_manager.CenterPanel(this.ClientSize.Width, this.ClientSize.Height);
            player = new Players();
            Other_option = new Other(btnValider, this, Btn_Pret);
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
                grille_manager.Piece_Rezise(true);
                var (largeurCase, hauteurCase, _, _) = grille_manager.CalculerTaillesEtPositions();
                Initialisation_Pion.PositionnerTousLesPions(PnlGrilleGame, largeurCase, hauteurCase);
            }
        }

        private void FicJeu_Load(object sender, EventArgs e)
        {
            Debug_config();
            grille_manager.Piece_Init();
            Other_option.bValider_position();
            Btn_Pret.Visible = false;
            player.Initialisation_Jeu(grille_manager, Btn_Pret, this);
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            var (largeurCase, hauteurCase, _, _) = grille_manager.CalculerTaillesEtPositions();
            Initialisation_Pion.PositionnerTousLesPions(PnlGrilleGame, largeurCase, hauteurCase);
            Btn_Pret.Visible = false;
            Player_Game();
        }

        private void Player_Game()
        {
            action_joue = false;
            grille_manager.TerminerPlacement();
            player.ChangerJoueur();
            grille_manager.Player_Grille_Change(player.CurrentPlayer);
            ActiverModeJeuPourJoueur(player.CurrentPlayer);
        }

        private void ActiverModeJeuPourJoueur(Player joueur)
        {
            foreach (Control ctrl in grille_manager.PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo info)
                {
                    pb.Click -= Pion_Jeu_Click;
                    if ((joueur == Player.Player_Blue && info.IsBlue) ||
                        (joueur == Player.Player_Red && !info.IsBlue))
                    {
                        pb.Click += Pion_Jeu_Click;
                    }
                }
            }
        }

        // Handler de clic sur un pion
        private void Pion_Jeu_Click(object sender, EventArgs e)
        {
            if (action_joue) return;

            var clickedPiece = sender as PictureBox;
            if (clickedPiece == null) return;

            var clickedInfo = clickedPiece.Tag as PieceInfo;
            if (clickedInfo == null) return;

            if (pionSelectionne == null)
            {
                pionSelectionne = clickedPiece;
                pionSelectionne.BackColor = Color.Yellow;
            }
            else
            {
                var selectedInfo = pionSelectionne.Tag as PieceInfo;
                if (pionSelectionne == clickedPiece)
                {
                    pionSelectionne.BackColor = clickedInfo.IsBlue ? Color.LightBlue : Color.LightCoral;
                    pionSelectionne = null;
                    return;
                }
                /*
                // Si c'est un adversaire adjacent, on attaque
                if (selectedInfo.IsBlue != clickedInfo.IsBlue)
                {
                    int dx = Math.Abs(selectedInfo.PositionGrille.X - clickedInfo.PositionGrille.X);
                    int dy = Math.Abs(selectedInfo.PositionGrille.Y - clickedInfo.PositionGrille.Y);
                    if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1))
                    {
                        bool victoire = ResoudreAffrontement(selectedInfo, clickedInfo);
                        if (victoire)
                        {
                            // Supprimer le pion adverse
                            grille_manager.PnlGrilleGame.Controls.Remove(clickedPiece);
                            if (clickedInfo.IsBlue)
                                player.PositionsPionsBleus.Remove(clickedInfo.PositionGrille);
                            else
                                player.PositionsPionsRouges.Remove(clickedInfo.PositionGrille);

                            // Déplacer le pion attaquant
                            DeplacerPion(selectedInfo, pionSelectionne, clickedInfo.PositionGrille);
                        }
                        else
                        {
                            // Supprimer le pion attaquant
                            grille_manager.PnlGrilleGame.Controls.Remove(pionSelectionne);
                            if (selectedInfo.IsBlue)
                                player.PositionsPionsBleus.Remove(selectedInfo.PositionGrille);
                            else
                                player.PositionsPionsRouges.Remove(selectedInfo.PositionGrille);
                        }
                        pionSelectionne = null;
                        action_joue = true;
                        return;
                    
                }*/

                // Sinon, comportement normal (désélection)
                pionSelectionne.BackColor = selectedInfo.IsBlue ? Color.LightBlue : Color.LightCoral;
                pionSelectionne = null;
            }
        }

        // Handler de clic sur la grille (case vide ou occupée)
        private void PnlGrilleGame_MouseDown(object sender, MouseEventArgs e)
        {
            if (action_joue) return;
            if (pionSelectionne == null) return;
            var info = pionSelectionne.Tag as PieceInfo;
            if (info == null) return;

            var (largeurCase, hauteurCase, _, _) = grille_manager.CalculerTaillesEtPositions();
            int col = e.X / largeurCase + 1;
            int row = e.Y / hauteurCase + 1;
            Point destination = new Point(col, row);

            // Si on clique sur la case actuelle du pion sélectionné, on le désélectionne
            if (info.PositionGrille.Equals(destination))
            {
                pionSelectionne.BackColor = info.IsBlue ? Color.LightBlue : Color.LightCoral;
                pionSelectionne = null;
                return;
            }

            // Empêche le déplacement des bombes et du drapeau
            if (info.Nom == "Bombe" || info.Nom == "Drapeau")
                return;

            bool deplacementValide = false;

            if (info.Nom == "Éclaireur")
            {
                // L'éclaireur peut avancer de plusieurs cases en ligne droite si le chemin est libre
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
                // Toutes les autres pièces (y compris l'Espion) ne peuvent avancer que d'une case
                int dx = Math.Abs(info.PositionGrille.X - destination.X);
                int dy = Math.Abs(info.PositionGrille.Y - destination.Y);
                deplacementValide = ((dx == 1 && dy == 0) || (dx == 0 && dy == 1));
            }


            if (!deplacementValide)
                return;

            // Vérifie si la case est occupée
            PictureBox pbCible = null;
            PieceInfo cibleInfo = null;
            foreach (Control ctrl in grille_manager.PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo ci)
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
                if (cibleInfo.IsBlue == info.IsBlue)
                {
                    MessageBox.Show("Case occupée par un de vos pions !");
                }
                else
                {
                    // Lancement de l'affrontement
                    byte victoire = ResoudreAffrontement(info, cibleInfo);
                    if (victoire == 1)
                    {
                        // Supprimer le pion adverse
                        grille_manager.PnlGrilleGame.Controls.Remove(pbCible);
                        if (cibleInfo.IsBlue)
                            player.PositionsPionsBleus.Remove(cibleInfo.PositionGrille);
                        else
                            player.PositionsPionsRouges.Remove(cibleInfo.PositionGrille);

                        // Déplacer le pion attaquant
                        DeplacerPion(info, pionSelectionne, destination);
                    }
                    if(victoire == 2)
                    {
                        // Supprimer le pion attaquant
                        grille_manager.PnlGrilleGame.Controls.Remove(pionSelectionne);
                        if (info.IsBlue)
                            player.PositionsPionsBleus.Remove(info.PositionGrille);
                        else
                            player.PositionsPionsRouges.Remove(info.PositionGrille);
                    }
                    if (victoire == 4)
                    {
                        // Le drapeau a été touché, fin de la partie
                        MessageBox.Show("Drapeau touché ! Fin de la partie !");
                        // Ici, tu peux ajouter la logique pour terminer le jeu
                    }
                    
                        if (victoire == 3)
                        {
                            // Supprimer les deux pions en cas d'égalité
                            grille_manager.PnlGrilleGame.Controls.Remove(pbCible);
                            grille_manager.PnlGrilleGame.Controls.Remove(pionSelectionne);

                            if (cibleInfo.IsBlue)
                                player.PositionsPionsBleus.Remove(cibleInfo.PositionGrille);
                            else
                                player.PositionsPionsRouges.Remove(cibleInfo.PositionGrille);

                            if (info.IsBlue)
                                player.PositionsPionsBleus.Remove(info.PositionGrille);
                            else
                                player.PositionsPionsRouges.Remove(info.PositionGrille);

                            MessageBox.Show("Égalité ! Les deux pions sont retirés.");
                        }

                }
                pionSelectionne = null;
                action_joue = true;
                return;
            }

            // Case libre : déplacer le pion
            DeplacerPion(info, pionSelectionne, destination);

            pionSelectionne = null;
            action_joue = true;
        }

        // Déplacement d'un pion sur la grille et mise à jour des dictionnaires
        private void DeplacerPion(PieceInfo info, PictureBox pb, Point destination)
        {
            if (info.IsBlue)
            {
                player.PositionsPionsBleus.Remove(info.PositionGrille);
                player.PositionsPionsBleus[destination] = info.Nom;
            }
            else
            {
                player.PositionsPionsRouges.Remove(info.PositionGrille);
                player.PositionsPionsRouges[destination] = info.Nom;
            }

            info.PositionGrille = destination;

            var (largeurCase, hauteurCase, _, _) = grille_manager.CalculerTaillesEtPositions();
            int tailleX = (int)(largeurCase * 0.8);
            int tailleY = (int)(hauteurCase * 0.8);
            pb.Location = new Point(
                (destination.X - 1) * largeurCase + (largeurCase - tailleX) / 2,
                (destination.Y - 1) * hauteurCase + (hauteurCase - tailleY) / 2
            );
            pb.BackColor = info.IsBlue ? Color.LightBlue : Color.LightCoral;
        }

        // Fonction d'affrontement à adapter selon tes règles
        private byte ResoudreAffrontement(PieceInfo attaquant, PieceInfo defenseur)
        {
            int forceAttaquant = GetForce(attaquant.Nom);
            int forceDefenseur = GetForce(defenseur.Nom);
            if(forceAttaquant == 3 && forceDefenseur == 11) // exepction bombe demineur 
            {
                Debug.WriteLine("bombe déminée");
                MessageBox.Show("bombe deminée");
                return 1; // le demineur gagne
            }
            if(forceAttaquant == 1 && forceDefenseur == 10) // si l'espion attaque le 10 il gagne si il se défend il perd 
            {
                Debug.WriteLine("Attaquant gagne ! (Espion attaque Maréchal)");
                MessageBox.Show("Attaquant gagne ! (Espion attaque Maréchal)");
                return 1; // Attaquant gagne
            }

            if (forceDefenseur == 0) // Drapeau
            {
                Debug.WriteLine("Attaquant gagne ! (Drapeau touché)");
                MessageBox.Show("Attaquant gagne ! (Drapeau touché)");
                return 4; // Attaquant gagne
            }

            if (forceAttaquant > forceDefenseur)
            {
                Debug.WriteLine("Attaquant gagne !");
                return 1;
            } // Attaquant gagne}
            else if (forceAttaquant < forceDefenseur)
            {
                Debug.WriteLine("Défenseur gagne !");
                return 2;
            }
            else
                Debug.WriteLine("égalité !");
            return 3; // Défenseur gagne ou égalité (défenseur gagne)
        }

        // Mapping force
        private int GetForce(string nom)
        {
            switch (nom)
            {
                case "Maréchal": return 10;
                case "Général": return 9;
                case "Colonel": return 8;
                case "Commandant": return 7;
                case "Capitaine": return 6;
                case "Lieutenant": return 5;
                case "Sergent": return 4;
                case "Démineur": return 3;
                case "Éclaireur": return 2;
                case "Espion": return 1;
                case "Bombe": return 11; // Bombe spéciale
                case "Drapeau": return 0;
                default: return 0;
            }
        }

        // Vérifie si une case est occupée (utile uniquement pour déplacement sur case vide)
        private bool CaseOccupee(Point pos)
        {
            foreach (Control ctrl in grille_manager.PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo info)
                {
                    if (info.PositionGrille.Equals(pos))
                        return true;
                }
            }
            return false;
        }

        private void FicJeu_SizeChanged(object sender, EventArgs e)
        {
            if (Other_option != null)
            {
                Other_option.bValider_position();
            }
        }

        private void Btn_Pret_Click(object sender, EventArgs e)
        {
        }
    }
}
