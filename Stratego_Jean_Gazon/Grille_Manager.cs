using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Dictionary<Point, string> PositionsPionsBleus { get; private set; } = new Dictionary<Point, string>();
        public Dictionary<Point, string> PositionsPionsRouges { get; private set; } = new Dictionary<Point, string>();

        // Propriétés centralisées pour les tailles et positions
        private int largeurCase;
        private int hauteurCase;
        private int taillePictureBoxX;
        private int taillePictureBoxY;
        private Point basGauche;
        private Point hautGauche;

        public Grille_Manager(Panel panel_Grille, Panel panel_Pause, PictureBox picture1, PictureBox picture2, ImageList Imglistperso)
        {
            PnlGrilleGame = panel_Grille;
            pnlMenuPause = panel_Pause;
            ptLac1 = picture1;
            ptLac2 = picture2;
            ImageList = Imglistperso;
            RecalculerTaillesEtPositions();
        }

        public void RecalculerTaillesEtPositions()
        {
            largeurCase = PnlGrilleGame.Width / 10;
            hauteurCase = PnlGrilleGame.Height / 10;
            taillePictureBoxX = (int)(largeurCase * 0.8);
            taillePictureBoxY = (int)(hauteurCase * 0.8);
            basGauche = new Point((largeurCase * 0) + (largeurCase / 2), (hauteurCase * 6) + (hauteurCase / 2));
            hautGauche = new Point((largeurCase * 0) + (largeurCase / 2), (hauteurCase * 0) + (hauteurCase / 2));
        }

        public (int largeurCase, int hauteurCase, Point basGauche, Point hautGauche) GetTaillesEtPositions()
        {
            return (largeurCase, hauteurCase, basGauche, hautGauche);
        }

        public void AjouterPion(Point position, string typePion, bool isBlue)
        {
            if (isBlue)
                PositionsPionsBleus[position] = typePion;
            else
                PositionsPionsRouges[position] = typePion;
        }

        public void SupprimerPion(Point position, bool isBlue)
        {
            if (isBlue)
                PositionsPionsBleus.Remove(position);
            else
                PositionsPionsRouges.Remove(position);
        }

        public void DeplacerPion(PieceInfo info, PictureBox pb, Point destination)
        {
            if (info.IsBlue)
            {
                PositionsPionsBleus.Remove(info.PositionGrille);
                PositionsPionsBleus[destination] = info.Nom;
            }
            else
            {
                PositionsPionsRouges.Remove(info.PositionGrille);
                PositionsPionsRouges[destination] = info.Nom;
            }

            info.PositionGrille = destination;

            pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);
            pb.Location = CalculerPositionGraphique(destination.X, destination.Y);
            pb.BackColor = info.IsBlue ? Color.LightBlue : Color.LightCoral;
        }

        public Dictionary<Point, string> ObtenirPositionsBleues() => PositionsPionsBleus;
        public Dictionary<Point, string> ObtenirPositionsRouges() => PositionsPionsRouges;

        public void PnlGrilleGame_Dessine(object sender, PaintEventArgs e)
        {
            for (int i = 1; i < 10; i++)
            {
                e.Graphics.DrawLine(Pens.Black, 0, i * hauteurCase, PnlGrilleGame.Width, i * hauteurCase);
            }

            for (int j = 1; j < 10; j++)
            {
                e.Graphics.DrawLine(Pens.Black, j * largeurCase, 0, j * largeurCase, PnlGrilleGame.Height);
            }

            Place_ptlac(2, 4, ptLac1);
            Place_ptlac(6, 4, ptLac2);
        }

        private void Place_ptlac(int positionx, int positiony, PictureBox picturebox)
        {
            int x = positionx * largeurCase;
            int y = positiony * hauteurCase;
            int width = 2 * largeurCase;
            int height = 2 * hauteurCase;
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

            int xPosition = startX;
            int yPosition = startY;

            int colonne = 1;
            int ligne = isBlue ? 7 : 1;
            int pionParLigne = 10;

            foreach (var (nom, count, ImgIndex) in personnages)
            {
                for (int i = 0; i < count; i++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Name = $"{nom}_{equipe}{i + 1}",
                        Size = new Size(taillePictureBoxX, taillePictureBoxY),
                        BackColor = isBlue ? Color.LightBlue : Color.LightCoral,
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

                    pictureBox.Location = CalculerPositionGraphique(colonne, ligne);

                    PnlGrilleGame.Controls.Add(pictureBox);

                    if (isBlue)
                        PositionsPionsBleus[new Point(colonne, ligne)] = nom;
                    else
                        PositionsPionsRouges[new Point(colonne, ligne)] = nom;

                    colonne++;
                    if (colonne > pionParLigne)
                    {
                        colonne = 1;
                        ligne++;
                    }
                }
            }
        }

        private Point CalculerPositionGraphique(int col, int row)
        {
            return new Point(
                (col - 1) * largeurCase + (largeurCase - taillePictureBoxX) / 2,
                (row - 1) * hauteurCase + (hauteurCase - taillePictureBoxY) / 2
            );
        }

        public void Piece_Init()
        {
            PositionsPionsBleus.Clear();
            PositionsPionsRouges.Clear();
            RecalculerTaillesEtPositions();
            CreateStrategoPictureBoxes(basGauche.X, basGauche.Y, true);
            CreateStrategoPictureBoxes(hautGauche.X, hautGauche.Y, false);
        }

        public void Piece_Rezise(bool isblue)
        {
            RecalculerTaillesEtPositions();
            Piece_Adaptation(isblue);
            Piece_Adaptation(!isblue);
        }

       private void Piece_Adaptation(bool isBlueTeam)
        {
            foreach (Control PictureBox_piece in PnlGrilleGame.Controls)
            {
                if (PictureBox_piece is PictureBox pb && pb.Tag is PieceInfo info && info.IsBlue == isBlueTeam)
                {
                    int col = info.PositionGrille.X;
                    int row = info.PositionGrille.Y;

                    pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);
                    pb.Location = CalculerPositionGraphique(col, row);
                }
            }
        }
        public void Player_Grille_Change(Player Current_Player)
        {
            if (Current_Player == Player.Player_Blue)
            {
                Cacher_Piece(true);
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
        }

        public void TerminerPlacement()
        {
            initialisationPion?.SupprimerEvents();
            initialisationPion = null;
        }
    }
}
