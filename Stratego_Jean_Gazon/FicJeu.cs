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

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu : Form
    {
        private MenuESC menuEsc;
        int[] PositionPnlClicG = new int[2];
        int[] positioncase = new int[2];
        private void Debug_config()
        {
            string logFilePath = "Positioncase.txt";

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
            menuEsc = new MenuESC(this, pnlMenuPause, PnlGrilleGame, btnReprendre, btnJeuQuitter,pnlPausebtnrecommencer);
            pnlMenuPause.Parent = this;
            CenterPanel();
        }

        private void PnlGrilleGame_Paint(object sender, PaintEventArgs e)
        {
            // Calculer la taille des cases
            int largeurCase = PnlGrilleGame.Width / 10;  // 10 cases en largeur
            int hauteurCase = PnlGrilleGame.Height / 10; // 10 cases en hauteur
            // Dessiner les lignes horizontales
            for (int i = 1; i < 10; i++) // Nous commençons à 1 car la première ligne est au-dessus de 0
            {
                e.Graphics.DrawLine(Pens.Black, 0, i * hauteurCase, PnlGrilleGame.Width, i * hauteurCase);
            }

            // Dessiner les lignes verticales
            for (int j = 1; j < 10; j++) // Idem pour les colonnes
            {
                e.Graphics.DrawLine(Pens.Black, j * largeurCase, 0, j * largeurCase, PnlGrilleGame.Height);
            }
            Place_ptlac(2, 4, largeurCase, hauteurCase, ptLac1);
            Place_ptlac(6, 4, largeurCase, hauteurCase, ptLac2);

        }

        private void CenterPanel()
        {
            pnlMenuPause.Left = (this.ClientSize.Width - pnlMenuPause.Width) / 2;
            pnlMenuPause.Top = (this.ClientSize.Height - pnlMenuPause.Height) / 2;
        }



        private void Place_ptlac(int positionx, int positiony, int largeurcase, int hauteurcase, PictureBox picturebox)
        {
            int x = positionx * largeurcase; // Position X pour les colonnes 5 et 6
            int y = positiony * hauteurcase; // Position Y pour les lignes 3 et 4
            int width = 2 * largeurcase; // Largeur pour couvrir les colonnes 5 et 6
            int height = 2 * hauteurcase; // Hauteur pour couvrir les lignes 3 et 4
            picturebox.SetBounds(x, y, width, height);

        }

        private void PnlGrilleGame_SizeChanged(object sender, EventArgs e)
        {
            this.UpdateStyles();
            Variable_Resize();
            CenterPanel();

            

        }

        private void PnlGrilleGame_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PositionPnlClicG[0] = e.X / (PnlGrilleGame.Width / 10) + 1;
                PositionPnlClicG[1] = e.Y / (PnlGrilleGame.Height / 10) + 1;
                     
            }
        }

        private void CreateStrategoPictureBoxes(int startX, int startY, bool isBlue)
        {
            string equipe = isBlue ? "bleu" : "rouge";
            var personnages = new (string nom, int count)[]
            {
        ("Maréchal", 1), ("Général", 1), ("Colonel", 1),
        ("Commandant", 1), ("Capitaine", 1), ("Lieutenant", 2),
        ("Sergent", 3), ("Démineur", 6), ("Adjudant", 2),
        ("Éclaireur", 4), ("Espion", 1), ("Bombe", 6),
        ("Drapeau", 1)
            };

            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;
            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);
            Color equipeColor = isBlue ? Color.LightBlue : Color.LightCoral;

            int xPosition = startX;
            int yPosition = startY;

            foreach (var (nom, count) in personnages)
            {
                for (int i = 0; i < count; i++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Name = $"{nom}_{equipe}{i + 1}",
                        Size = new Size(taillePictureBoxX, taillePictureBoxY),
                        BackColor = equipeColor
                    };

                    // Centrer la PictureBox dans la case
                    pictureBox.Location = new Point(
                        xPosition - (taillePictureBoxX / 2),
                        yPosition - (taillePictureBoxY / 2)
                    );
                    pictureBox.Tag = isBlue;

                    PnlGrilleGame.Controls.Add(pictureBox);

                    xPosition += largeurCase;
                    if (xPosition >= PnlGrilleGame.Width)
                    {
                        xPosition = startX;
                        yPosition += hauteurCase;
                    }
                }
            }
        }
        private void FicJeu_Load(object sender, EventArgs e)
        {
            Debug_config();
            Piece_Init();


        }
        private void Piece_Init()
        {
            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;

            // Calcul du point de départ pour la première pièce (équipe bleue)
            int Case1BasG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1BasG_Y = (hauteurCase * 7) + (hauteurCase / 2);

            // Calcul du point de départ pour la deuxième pièce (équipe rouge)
            int Case1hautG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1hautG_Y = (hauteurCase * 0) + (hauteurCase / 2);
            CreateStrategoPictureBoxes(Case1BasG_X, Case1BasG_Y, true);
            CreateStrategoPictureBoxes(Case1hautG_X, Case1hautG_Y, false);
        }

        private void Variable_Resize()
        {
            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;

            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);

            int Case1BasG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1BasG_Y = (hauteurCase * 7) + (hauteurCase / 2);

            int Case1hautG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1hautG_Y = (hauteurCase * 0) + (hauteurCase / 2);
            Resizepiece(Case1BasG_X, Case1BasG_Y, largeurCase, hauteurCase, taillePictureBoxX, taillePictureBoxY,true);

            Resizepiece(Case1hautG_X, Case1hautG_Y, largeurCase, hauteurCase, taillePictureBoxX, taillePictureBoxY,false);
           
        }

        private void Resizepiece(int LocationX, int LocationY, int largeurCase, int hauteurCase, int taillePictureBoxX, int taillePictureBoxY, bool isBlueTeam)
        {
            int PositionX = LocationX;
            int PositionY = LocationY;

            int compteur = 0;

            foreach (Control PictureBox_piece in PnlGrilleGame.Controls)
            {
                if (PictureBox_piece is PictureBox pb && pb.Tag is bool pieceBlue && pieceBlue == isBlueTeam) // Filtrer selon la couleur
                {
                    compteur++;
                    ;
                    


                    // Définir la taille correcte
                    pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);

                    // Centrer la PictureBox dans la case
                    pb.Location = new Point(
                        PositionX - (taillePictureBoxX / 2),
                        PositionY - (taillePictureBoxY / 2)
                    );

                    // Déplacer vers la case suivante
                    PositionX += largeurCase;

                    // Si on dépasse la largeur du panel, passer à la ligne suivante
                    if (PositionX >= PnlGrilleGame.Width)
                    {
                        PositionX = LocationX;
                        PositionY += hauteurCase;
                    }
                }
            }
        }
        // Gestionnaire d'événements pour le bouton "Reprendre"
    }
}
