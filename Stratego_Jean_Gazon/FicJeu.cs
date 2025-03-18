﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu : Form
    {
        int[] PositionPnlClicG = new int [2] ;
        int[] positioncase = new int [2] ;
        private void Debug_config()
        {

            Debug.Listeners.Add(new TextWriterTraceListener("Positioncase.txt"));
            Debug.AutoFlush = true;
           
        }
        
        private const string WINFORM_EVENTS = "WinForm Events";
        public FicJeu()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            
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
            Place_ptlac(2,4,largeurCase, hauteurCase,ptLac1);
            Place_ptlac(6, 4, largeurCase, hauteurCase, ptLac2);
        }
        

       

        private void Place_ptlac(int positionx, int positiony,int largeurcase,int hauteurcase,PictureBox picturebox)
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
           
        }

        private void PnlGrilleGame_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                PositionPnlClicG[0] = e.X / (PnlGrilleGame.Width / 10) + 1;
                PositionPnlClicG[1] = e.Y / (PnlGrilleGame.Height / 10) + 1;

                
                Debug.Write("ligne" + PositionPnlClicG[0] + " colonne " + PositionPnlClicG[1]+" \n");

                Debug.WriteLine(e.X.ToString() + " " + e.Y.ToString());



            }
        }

        private void CreateStrategoPictureBoxes( int startX, int startY,bool isBlue)
        {
            string equipe = "rouge"; 
            if (isBlue) { equipe =  "bleu"; }

            // Liste des personnages avec leur nombre d'occurrences
            var personnages = new (string nom, int count)[]
            {
            ("Maréchal", 1),
            ("Général", 1),
            ("Colonel", 1),
            ("Commandant", 1),
            ("Capitaine", 1),
            ("Lieutenant", 2),
            ("Sergent", 3),
            ("Démineur", 6),
            ("Adjudant", 2),
            ("Éclaireur", 4),
            ("Espion", 1),
            ("Bombe", 6),
            ("Drapeau", 1)
            };

            // Position initiale pour les PictureBox
            int xPosition = startX;
            int yPosition = startY;

            // Définition de la couleur en fonction de l'équipe
            Color equipeColor = (equipe == "Bleu") ? Color.LightBlue : Color.LightCoral;

            // Boucle pour créer les PictureBox pour chaque personnage
            foreach (var (nom, count) in personnages)
            {
                for (int i = 0; i < count; i++)
                {
                    // Création d'une nouvelle PictureBox
                    PictureBox pictureBox = new PictureBox();

                    // Nom unique pour chaque PictureBox
                    pictureBox.Name = $"{nom}_{equipe}{i + 1}";

                    // Définition des propriétés de la PictureBox
                    pictureBox.Size = new System.Drawing.Size(10, 10); // Taille réduite pour mieux s'afficher
                    pictureBox.Location = new System.Drawing.Point(xPosition, yPosition);
                    pictureBox.BackColor = equipeColor; // Couleur selon l'équipe
                    pictureBox.BringToFront();

                    // Ajout de la PictureBox au formulaire
                    this.Controls.Add(pictureBox);

                    // Mise à jour de la position (on déplace chaque PictureBox sur l'axe X)
                    xPosition += 90;

                    // Si la position dépasse la largeur du formulaire, passer à la ligne suivante
                    if (xPosition > this.Width - 100)
                    {
                        xPosition = startX;
                        yPosition += 90;
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
            int[] Case1BasG = new int[2];
            int[] Case1hautG = new int[2];
            Case1BasG[0] = PnlGrilleGame.Width/20;// avoir le millieu de  la case
            Case1BasG[1]=PnlGrilleGame.Height/20;
            CreateStrategoPictureBoxes(Case1BasG[0], Case1BasG[1], true);

        }
    }
}
