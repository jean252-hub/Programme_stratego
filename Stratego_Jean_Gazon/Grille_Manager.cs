using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public class Grille_Manager
    {
        public Panel PnlGrilleGame;
        private Panel pnlMenuPause;
        private PictureBox ptLac1;
        private PictureBox ptLac2;
        private ImageList ImageList;
        private Initialisation_Pion initialisationPion;

        public Dictionary<Point, personnage_base> PositionsPionsBleus { get; private set; } = new Dictionary<Point, personnage_base>();
        public Dictionary<Point, personnage_base> PositionsPionsRouges { get; private set; } = new Dictionary<Point, personnage_base>();

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

        public void AjouterPion(Point position, personnage_base pion, bool isBlue)
        {
            if (isBlue)
                PositionsPionsBleus[position] = pion;
            else
                PositionsPionsRouges[position] = pion;
        }

        public void SupprimerPion(Point position, bool isBlue)
        {
            if (isBlue)
                PositionsPionsBleus.Remove(position);
            else
                PositionsPionsRouges.Remove(position);
        }

        public void DeplacerPion(personnage_base pion, PictureBox pb, Point destination)
        {
            if (pion.Couleur)
            {
                PositionsPionsBleus.Remove(pion.PositionGrille);
                PositionsPionsBleus[destination] = pion;
            }
            else
            {
                PositionsPionsRouges.Remove(pion.PositionGrille);
                PositionsPionsRouges[destination] = pion;
            }

            pion.PositionGrille = destination;

            pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);
            pb.Location = CalculerPositionGraphique(destination.X, destination.Y);
            pb.BackColor = pion.Couleur ? Color.LightBlue : Color.LightCoral;
        }

        public Dictionary<Point, personnage_base> ObtenirPositionsBleues() => PositionsPionsBleus;
        public Dictionary<Point, personnage_base> ObtenirPositionsRouges() => PositionsPionsRouges;

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

        private void CreateStrategoPictureBoxes(int startX, int startY, bool isBlue) // permet de créer les pions de la grille
        {
            string equipe = isBlue ? "bleu" : "rouge"; // 
            var personnages = new (string nom, int count, int ImgIndex)[] // on definit les personnages que l'on veut avoir sur la grille et leur nombre dans une variable personnage 
            // celle ci sert juste a créer le bon nombre de maéchale génral... 
            {
                ("Maréchal", 1,10), ("Général", 1,9), ("Colonel", 2,8),
                ("Commandant", 3,7), ("Capitaine", 4,6), ("Lieutenant", 4,5),
                ("Sergent", 4,4), ("Démineur", 5,3),
                ("Éclaireur", 8,2), ("Espion", 1,11), ("Bombe", 6,1),
                ("Drapeau", 1,0)
            };

            int colonne = 1; 
            int ligne = isBlue ? 7 : 1; // permet de savoir ou la boucle pour créer les pictures boxes va commencer, si c'est l'équipe bleu on commence en bas sinon on commence en haut
            int pionParLigne = 10;

            foreach (var (nom, count, ImgIndex) in personnages)// création des personnage  à l'aide d'une boucle pour parcourir les personnages
            {
                for (int i = 0; i < count; i++) // permet de créer le nombre de personnage voulu en fonction du type (exemple 1 maréchal, 2 colonel...)
                {
                    Point position = new Point(colonne, ligne);// on donne tout les caractéristique de l'objet personnage 
                    personnage_base pion = null;
                    switch (nom)
                    {
                        case "Maréchal": pion = new Marechal(isBlue, position); break;
                        case "Général": pion = new General(isBlue, position); break;
                        case "Colonel": pion = new Colonel(isBlue, position); break;
                        case "Commandant": pion = new Commandant(isBlue, position); break;
                        case "Capitaine": pion = new Capitaine(isBlue, position); break;
                        case "Lieutenant": pion = new Lieutenant(isBlue, position); break;
                        case "Sergent": pion = new Sergent(isBlue, position); break;
                        case "Démineur": pion = new Demineur(isBlue, position); break;
                        case "Éclaireur": pion = new Eclaireur(isBlue, position); break;
                        case "Espion": pion = new Espion(isBlue, position); break;
                        case "Bombe": pion = new Bombe(isBlue, position); break;
                        case "Drapeau": pion = new Drapeau(isBlue, position); break;
                    }

                    PictureBox pictureBox = new PictureBox // on crée un picturebox pour chaque personnage
                    {
                        Name = $"{nom}_{equipe}{i + 1}",
                        Size = new Size(taillePictureBoxX, taillePictureBoxY),
                        BackColor = isBlue ? Color.LightBlue : Color.LightCoral,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = ImageList.Images[ImgIndex],
                        Tag = pion // on associe chaque personnage à sa picturebox
                    };

                    pictureBox.Location = CalculerPositionGraphique(colonne, ligne);

                    PnlGrilleGame.Controls.Add(pictureBox);

                    if (isBlue)
                        PositionsPionsBleus[position] = pion;
                    else
                        PositionsPionsRouges[position] = pion;

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
                if (PictureBox_piece is PictureBox pb && pb.Tag is personnage_base pion && pion.Couleur == isBlueTeam)
                {
                    int col = pion.PositionGrille.X;
                    int row = pion.PositionGrille.Y;

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
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    if (pion.Couleur != isblue)
                    {
                        pb.Image = null;
                    }
                    else
                    {
                        int imgIndex = GetImageIndexFromGrade(pion.Grade);
                        if (imgIndex >= 0 && imgIndex < ImageList.Images.Count)
                            pb.Image = ImageList.Images[imgIndex];
                    }
                }
            }
        }

        private int GetImageIndexFromGrade(string grade) // donne l'index de l'image en fonction du grade du personnage
        {
            switch (grade)
            {
                case "Drapeau": return 0;
                case "Bombe": return 1;
                case "Éclaireur": return 2;
                case "Démineur": return 3;
                case "Sergent": return 4;
                case "Lieutenant": return 5;
                case "Capitaine": return 6;
                case "Commandant": return 7;
                case "Colonel": return 8;
                case "Général": return 9;
                case "Maréchal": return 10;
                case "Espion": return 11;
                default: return -1;
            }
        }

        public void ActiverPlacement(Player joueur)
        {
            initialisationPion = new Initialisation_Pion(PnlGrilleGame, joueur);  // appelle de la classe initialisation pion pour pouvoir positionner ses pions en début de partie 
        }

        public void TerminerPlacement()
        {
            initialisationPion?.SupprimerEvents();
            initialisationPion = null;
        }
    }
}
