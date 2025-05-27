using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public class PieceInfo
    {
        public bool IsBlue { get; set; }
        public Image OriginalImage { get; set; }
        public string Nom { get; set; } // Type du pion (ex: "Maréchal")
        public Point PositionGrille { get; set; } // Position logique sur la grille (colonne, ligne)
    }

    public class Grille_Manager
    {
        public Panel PnlGrilleGame;
        private Panel pnlMenuPause;
        private PictureBox ptLac1;
        private PictureBox ptLac2;
        private ImageList ImageList;
        private Initialisation_Pion initialisationPion;

        public Grille_Manager(Panel panel_Grille, Panel panel_Pause, PictureBox picture1, PictureBox picture2, ImageList Imglistperso)
        {
            PnlGrilleGame = panel_Grille;
            pnlMenuPause = panel_Pause;
            ptLac1 = picture1;
            ptLac2 = picture2;
            ImageList = Imglistperso;
        }

        public void PnlGrilleGame_Dessine(object sender, PaintEventArgs e)
        {
            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;

            for (int i = 1; i < 10; i++)
            {
                e.Graphics.DrawLine(Pens.Black, 0, i * hauteurCase, PnlGrilleGame.Width, i * hauteurCase);
            }

            for (int j = 1; j < 10; j++)
            {
                e.Graphics.DrawLine(Pens.Black, j * largeurCase, 0, j * largeurCase, PnlGrilleGame.Height);
            }

            Place_ptlac(2, 4, largeurCase, hauteurCase, ptLac1);
            Place_ptlac(6, 4, largeurCase, hauteurCase, ptLac2);
        }

        private void Place_ptlac(int positionx, int positiony, int largeurcase, int hauteurcase, PictureBox picturebox)
        {
            int x = positionx * largeurcase;
            int y = positiony * hauteurcase;
            int width = 2 * largeurcase;
            int height = 2 * hauteurcase;
            picturebox.SetBounds(x, y, width, height);
        }

        public void CenterPanel(int ClientSizeW, int ClientSizeH)
        {
            pnlMenuPause.Left = (ClientSizeW - pnlMenuPause.Width) / 2;
            pnlMenuPause.Top = (ClientSizeH - pnlMenuPause.Height) / 2;
        }

        private void CreateStrategoPictureBoxes(int startX, int startY, bool isBlue)
        {
            string equipe = isBlue ? "bleu" : "rouge";
            var personnages = new (string nom, int count, int ImgIndex)[]
            {
                ("Maréchal", 1,10), ("Général", 1,9), ("Colonel", 2,8),
                ("Commandant", 3,7), ("Capitaine", 4,6), ("Lieutenant", 4,5),
                ("Sergent", 4,4), ("Démineur", 5,3),
                ("Éclaireur", 8,2), ("Espion", 1,11), ("Bombe", 6,1),
                ("Drapeau", 1,0)
            };

            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;
            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);
            Color equipeColor = isBlue ? Color.LightBlue : Color.LightCoral;

            int xPosition = startX;
            int yPosition = startY;

            // Pour le placement logique sur la grille
            int colonne = 1;
            int ligne = isBlue ? 7 : 1; // Les bleus en bas, les rouges en haut (adapter si besoin)
            int pionParLigne = 10;

            foreach (var (nom, count, ImgIndex) in personnages)
            {
                for (int i = 0; i < count; i++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Name = $"{nom}_{equipe}{i + 1}",
                        Size = new Size(taillePictureBoxX, taillePictureBoxY),
                        BackColor = equipeColor,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = ImageList.Images[ImgIndex],
                        Tag = new PieceInfo
                        {
                            IsBlue = isBlue,
                            OriginalImage = ImageList.Images[ImgIndex],
                            Nom = nom,
                            PositionGrille = new Point(colonne, ligne)
                        }
                    };

                    pictureBox.Location = new Point(
                        xPosition - (taillePictureBoxX / 2),
                        yPosition - (taillePictureBoxY / 2)
                    );

                    PnlGrilleGame.Controls.Add(pictureBox);

                    // Placement logique
                    colonne++;
                    if (colonne > pionParLigne)
                    {
                        colonne = 1;
                        ligne++;
                    }

                    // Placement graphique
                    xPosition += largeurCase;
                    if (xPosition >= PnlGrilleGame.Width)
                    {
                        xPosition = startX;
                        yPosition += hauteurCase;
                    }
                }
            }
        }

        public (int largeurCase, int hauteurCase, Point basGauche, Point hautGauche) CalculerTaillesEtPositions()
        {
            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;

            Point basGauche = new Point((largeurCase * 0) + (largeurCase / 2), (hauteurCase * 6) + (hauteurCase / 2));
            Point hautGauche = new Point((largeurCase * 0) + (largeurCase / 2), (hauteurCase * 0) + (hauteurCase / 2));

            return (largeurCase, hauteurCase, basGauche, hautGauche);
        }

        public void Piece_Init()
        {
            var (largeurCase, hauteurCase, basGauche, hautGauche) = CalculerTaillesEtPositions();

            CreateStrategoPictureBoxes(basGauche.X, basGauche.Y, true);
            CreateStrategoPictureBoxes(hautGauche.X, hautGauche.Y, false);
        }

        public void Piece_Rezise(bool isblue)
        {
            var (largeurCase, hauteurCase, basGauche, hautGauche) = CalculerTaillesEtPositions();

            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);

            Piece_Adaptation(basGauche.X, basGauche.Y, largeurCase, hauteurCase, taillePictureBoxX, taillePictureBoxY, isblue);
            Piece_Adaptation(hautGauche.X, hautGauche.Y, largeurCase, hauteurCase, taillePictureBoxX, taillePictureBoxY, !isblue);
        }

        private void Piece_Adaptation(int LocationX, int LocationY, int largeurCase, int hauteurCase, int taillePictureBoxX, int taillePictureBoxY, bool isBlueTeam)
        {
            foreach (Control PictureBox_piece in PnlGrilleGame.Controls)
            {
                if (PictureBox_piece is PictureBox pb && pb.Tag is PieceInfo info && info.IsBlue == isBlueTeam)
                {
                    // Utilise la position logique du pion pour le placer correctement
                    int col = info.PositionGrille.X;
                    int row = info.PositionGrille.Y;

                    pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);
                    pb.Location = new Point(
                        (col - 1) * largeurCase + (largeurCase - taillePictureBoxX) / 2,
                        (row - 1) * hauteurCase + (hauteurCase - taillePictureBoxY) / 2
                    );
                }
            }
        }


        public void Player_Grille_Change(Player Current_Player)
        {
            if (Current_Player == Player.Player_Blue)
            {
                Cacher_Piece(true);
                Debug.WriteLine("Cacher les pions rouges");
                Piece_Rezise(true);
            }
            else
            {
                Cacher_Piece(false);
                Piece_Rezise(false);
            }
        }

        public void Cacher_Piece(bool isblue)
        {
            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo info)
                {
                    if (info.IsBlue != isblue)
                    {
                        pb.Image = null;
                    }
                    else
                    {
                        pb.Image = info.OriginalImage;
                    }
                }
            }
        }

        public void ActiverPlacement(Player joueur)
        {
            initialisationPion = new Initialisation_Pion(PnlGrilleGame, joueur);
            Debug.WriteLine(joueur + "grille");
        }

        public void TerminerPlacement()
        {
            initialisationPion?.SupprimerEvents();
            initialisationPion = null;
        }
    }
}
