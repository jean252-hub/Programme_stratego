using Stratego_Jean_Gazon.LogiqueJeu;
using Stratego_Jean_Gazon.Reseau;
using Stratego_Jean_Gazon.Stratego_Jean_Gazon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu : Form
    {
        public static bool IsServeur { get; set; }
        public GameTransitionManager transitionManager;

        private Players player;
        private MenuESC menuEsc;

        // ✅ Champs correctement utilisés
        public static LogiqueServeur Logique_Serveur;
        public static LogiqueClient Logique_Client;

        private Grille_Manager grille_manager;
        private Grille_GameEngine grilleGameEngine;
        private Other Other_option;

        private PictureBox pionSelectionne = null;
        private bool action_joue = false;
        private bool initialisation_terminee = false;
        int premier_tour = 0;


        public ImageList ImageListPions => ImgListPerso;

        public FicJeu()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();

            SetStyle(
                ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint,
                true);

            // ✅ CORRECTION CRITIQUE
            Logique_Serveur = new LogiqueServeur();
            Logique_Client = new LogiqueClient();

            grille_manager = new Grille_Manager(
                PnlGrilleGame,
                pnlMenuPause,
                ptLac1,
                ptLac2,
                ImgListPerso);

            grilleGameEngine = new Grille_GameEngine();

            menuEsc = new MenuESC(
                this,
                pnlMenuPause,
                PnlGrilleGame,
                btnReprendre,
                btnJeuQuitter,
                pnlPausebtnrecommencer,
                btnValider);

            pnlMenuPause.Parent = this;

            transitionManager = new GameTransitionManager(
                this,
                Properties.Resources.Image_Transition);

            grille_manager.CenterPanel(ClientSize.Width, ClientSize.Height);
            grille_manager.RecalculerTaillesEtPositions();

            player = new Players();
            Other_option = new Other(btnValider, this, Btn_Pret);

            PositionnerTitreFenetre();
        }

        private async void FicJeu_Load(object sender, EventArgs e)
        {
            Debug_config();

            grille_manager.démarrer_connection(IsServeur);
            grille_manager.Piece_Init();
            Other_option.bValider_position();

            Btn_Pret.Visible = false;

            player.Initialisation_Jeu(grille_manager, Btn_Pret, this);

            if (IsServeur)
                await transitionManager.ShowPlacement(Player.Player_Blue);
            else
                await transitionManager.ShowPlacement(Player.Player_Red);
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

        private async Task btnValider_Click(object sender, EventArgs e)
        {
            if (!initialisation_terminee)
            {
                if (IsServeur)
                {
                    await grille_manager.ReceptionPositionPions();
                    await transitionManager.ShowPlacement(
                        IsServeur ? Player.Player_Red : Player.Player_Blue);
                    await grille_manager.EnvoyerPositionPionsBleu();
                    grille_manager.afficher_dictionnaire();
                }
                else
                {
                    await grille_manager.EnvoyerPositionsPions();
                    await grille_manager.ReceptionPositionPionsBleu();
                    grille_manager.afficher_dictionnaire();
                }

                initialisation_terminee = true;
            }

            if (IsServeur)
            {
                var (pion, pb, depart, destination) = await grille_manager.Recevoir_DeplacementAsync(IsServeur);
                MessageBox.Show($"Déplacement reçu du client : {depart} -> {destination} {pion} {pb}");

                if (pion != null && pb != null)
                {
                    if (PnlGrilleGame.InvokeRequired)
                    {
                        PnlGrilleGame.Invoke((Action)(() =>
                        {
                            grille_manager.DeplacerPion(pion, pb, destination);
                            PnlGrilleGame.Refresh();
                            MessageBox.Show("Déplacement appliqué sur le serveur.");
                        }));
                    }
                    else
                    {
                        grille_manager.DeplacerPion(pion, pb, destination);
                        PnlGrilleGame.Refresh();
                    }
                }
            }
            else
            {
                if (premier_tour >= 2)
                {
                    //MessageBox.Show("dans le deplcement");
                    var (pion, pb, depart, destination) = await grille_manager.Recevoir_DeplacementAsync(IsServeur);
                    MessageBox.Show($"Déplacement reçu du serveur : {depart} -> {destination} {pion} {pb}");

                    if (pion != null && pb != null)
                    {
                        if (PnlGrilleGame.InvokeRequired)
                        {
                            PnlGrilleGame.Invoke((Action)(() =>
                            {
                                grille_manager.DeplacerPion(pion, pb, destination);
                                PnlGrilleGame.Refresh();
                                MessageBox.Show("Déplacement appliqué sur le client.");
                            }));
                        }
                        else
                        {
                            grille_manager.DeplacerPion(pion, pb, destination);
                            PnlGrilleGame.Refresh();
                        }
                    }
                    
                }

            }
            premier_tour++;
            grille_manager.RecalculerTaillesEtPositions();
            var (largeurCase, hauteurCase, _, _) = grille_manager.GetTaillesEtPositions();
            Initialisation_Pion.PositionnerTousLesPions(PnlGrilleGame, largeurCase, hauteurCase);
            grille_manager.afficher_dictionnaire();
            Btn_Pret.Visible = false;
            Player_Game();
        }

        private async void Player_Game()
        {
            action_joue = false;

            await transitionManager.ShowChangeTurn(player.CurrentPlayer);

            // ✅ Logique serveur protégée
            /*if (IsServeur && Logique_Serveur != null)
            {
                Logique_Serveur.Gestion_Operation(grille_manager);
            }*/

            grille_manager.TerminerPlacement();
            grille_manager.Player_Grille_Change(player.CurrentPlayer);
            ActiverModeJeuPourJoueur(player.CurrentPlayer);
           // grille_manager.afficher_dictionnaire();
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

            PictureBox clickedPiece = sender as PictureBox;
            if (clickedPiece == null) return;

            personnage_base clickedInfo = clickedPiece.Tag as personnage_base;
            if (clickedInfo == null) return;

            if (pionSelectionne == null)
            {
                pionSelectionne = clickedPiece;
                pionSelectionne.BackColor = Color.Yellow;
            }
            else
            {
                personnage_base selectedInfo = pionSelectionne.Tag as personnage_base;

                pionSelectionne.BackColor =
                    selectedInfo.Couleur ? Color.LightBlue : Color.LightCoral;

                pionSelectionne = null;
            }
        }

        private void PositionnerTitreFenetre()
        {
            Titre_Fenetre.Location = new Point(
                ClientSize.Width / 2 - Titre_Fenetre.Width / 2,
                3);
        }

        private void BtnRetourMenu_Click(object sender, EventArgs e)
        {
            FermerLog();
            Close();
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

        private void btnValider_Click_Sync(object sender, EventArgs e)
        {
            _ = btnValider_Click(sender, e);
        }

        private void PnlGrilleGame_Paint(object sender, PaintEventArgs e)
        {
            if (grille_manager == null)
                return;

            grille_manager.PnlGrilleGame_Dessine(sender, e);
        }

        private void PnlGrilleGame_SizeChanged(object sender, EventArgs e)
        {
            UpdateStyles();

            if (grille_manager == null)
                return;

            grille_manager.CenterPanel(ClientSize.Width, ClientSize.Height);
            grille_manager.RecalculerTaillesEtPositions();
            grille_manager.Piece_Rezise(true);
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
            await grille_manager.Envoyer_DeplacementAsync(info, destination, pionSelectionne, IsServeur);
            
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


        private void FicJeu_SizeChanged(object sender, EventArgs e)
        {
            if (Other_option != null)
                Other_option.bValider_position();

            if (grille_manager != null)
                grille_manager.RecalculerTaillesEtPositions();

            PositionnerTitreFenetre();
        }

        private void Btn_Pret_Click(object sender, EventArgs e)
        {
            // Stub: logique à réimplémenter si nécessaire.
        }

        private Image GetPionImageFromImageList(string nom)
        {
            if (ImgListPerso.Images.ContainsKey(nom))
                return ImgListPerso.Images[nom];

            foreach (string key in ImgListPerso.Images.Keys)
            {
                if (string.Equals(key, nom, StringComparison.InvariantCultureIgnoreCase))
                    return ImgListPerso.Images[key];
            }

            return null;
        }

        private bool CaseOccupee(Point pos)
        {
            if (grille_manager == null)
                return false;

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
    }
}
