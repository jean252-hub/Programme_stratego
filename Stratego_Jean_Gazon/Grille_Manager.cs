using Stratego_Jean_Gazon.LogiqueJeu;
using Stratego_Jean_Gazon.Reseau;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Globalization;

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
        ReseauClient client = new ReseauClient();
        ReseauServeur serveur = new ReseauServeur();

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
            if (FicJeu.IsServeur == true)
            {
                CreateStrategoPictureBoxes(basGauche.X, basGauche.Y, true);
                CreateStrategoPictureBoxes(hautGauche.X, hautGauche.Y, false);
            }
            else
            {
                CreateStrategoPictureBoxes(hautGauche.X, hautGauche.Y, true);
                CreateStrategoPictureBoxes(basGauche.X, basGauche.Y, false);
            }
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
            DebugAfficherDictionnaire("PositionsPionsBleus", PositionsPionsBleus);
            //DebugAfficherDictionnaire("PositionsPionsRouges", PositionsPionsRouges);
        }

        private static void DebugAfficherDictionnaire(string nom, Dictionary<Point, personnage_base> dictionnaire)
        {
            Debug.WriteLine($"--- {nom} (Count={dictionnaire.Count}) ---");

            foreach (KeyValuePair<Point, personnage_base> kvp in dictionnaire)
            {
                Point position = kvp.Key;
                personnage_base pion = kvp.Value;

                string descriptionPion = pion == null
                    ? "null"
                    : $"Grade={pion.Grade}, Couleur={(pion.Couleur ? "Bleu" : "Rouge")}";

                Debug.WriteLine($"Pos=({position.X},{position.Y}) -> {descriptionPion}");
            }
        }
        //envoi des positions des pions rouges au serveur
        public async Task EnvoyerPositionsPions()
        {
            RebuildDictionnairesDepuisUI();
            await client.Envoyer_Initialisation_Pions(PositionsPionsRouges);

        }

        //reception des positions des pions rouges du serveur
        public async Task ReceptionPositionPions()
        {

            PositionsPionsRouges = await serveur.ReceptionInitialisationRouge();
            AppliquerInitialisationAdverseSansId(PositionsPionsRouges, false);
            Debug.WriteLine("Positions des pions rouges reçues du serveur.");
            DebugAfficherDictionnaire("PositionsPionsRouges", PositionsPionsRouges);
        }

        //reception des positions des pions bleus du client
        public async Task ReceptionPositionPionsBleu()
        {
            PositionsPionsBleus = await client.ReceptionInitialisationBleu();
            AppliquerInitialisationAdverseSansId(PositionsPionsBleus, true);
            Debug.WriteLine("Positions des pions bleus reçues du client.");
            DebugAfficherDictionnaire("PositionsPionsBleus", PositionsPionsBleus);
        }

        //envoi des positions des pions bleus au serveur
        public async Task EnvoyerPositionPionsBleu()
        {
            RebuildDictionnairesDepuisUI();
            await serveur.EnvoyerPositionBleu(PositionsPionsBleus);

        }
        public async void démarrer_connection(bool IsServeur)
        {
            if (IsServeur == true)
            {

                await serveur.DemarrerServeurAsync();
            }
            else
            {

                await client.DemarrerClientAsync();
            }
        }
        public void afficher_dictionnaire()
        {
            Debug.WriteLine("------------- teste pour le client regarder les rouge");

            DebugAfficherDictionnaire("PositionsPionsRouges", PositionsPionsRouges);
            DebugAfficherDictionnaire("PositionsPionsBleus", PositionsPionsBleus);
        }
        public async Task Envoyer_DeplacementAsync(personnage_base info, Point nouvellePosition, PictureBox pionSelectionne, bool isServeur)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (pionSelectionne == null)
                throw new ArgumentNullException(nameof(pionSelectionne));

            if (isServeur)
                await serveur.Envoyer_Deplacement(info, pionSelectionne, nouvellePosition);
            else
                await client.Envoyer_Deplacement(info, pionSelectionne, nouvellePosition);
        }
        public async Task<(personnage_base pion, PictureBox pb, Point depart, Point destination)> Recevoir_DeplacementAsync(bool isServeur)
        {
            if (isServeur)
            {
                var (positionDepart, positionArrivee) = await serveur.Reception_Deplacement();

                var dico = PositionsPionsRouges;

                PictureBox pb = null;
                personnage_base pionUi = null;

                foreach (Control ctrl in PnlGrilleGame.Controls)
                {
                    if (ctrl is PictureBox pic && pic.Tag is personnage_base tagPion)
                    {
                        if (tagPion.PositionGrille.Equals(positionDepart))
                        {
                            pb = pic;
                            pionUi = tagPion;
                            break;
                        }
                    }
                }

                if (pionUi != null)
                {
                    dico.Remove(positionDepart);
                    dico[positionArrivee] = pionUi;
                }

                return (pionUi, pb, positionDepart, positionArrivee);
            }
            else
            {
                var (positionDepart, positionArrivee) = await client.Recevoir_Deplacement();

                var dico = PositionsPionsBleus;

                PictureBox pb = null;
                personnage_base pionUi = null;

                foreach (Control ctrl in PnlGrilleGame.Controls)
                {
                    if (ctrl is PictureBox pic && pic.Tag is personnage_base tagPion)
                    {
                        if (tagPion.PositionGrille.Equals(positionDepart))
                        {
                            pb = pic;
                            pionUi = tagPion;
                            break;
                        }
                    }
                }

                if (pionUi != null)
                {
                    dico.Remove(positionDepart);
                    dico[positionArrivee] = pionUi;
                }

                return (pionUi, pb, positionDepart, positionArrivee);
            }
        }
        public void AppliquerInitialisationAdverseSansId(Dictionary<Point, personnage_base> positionsRecues, bool couleurEquipeRecue)
        {
            if (positionsRecues == null) throw new ArgumentNullException(nameof(positionsRecues));
            // Liste des PictureBox adverses disponibles, groupées par grade
            var disponiblesParGrade = new Dictionary<string, List<PictureBox>>(StringComparer.OrdinalIgnoreCase);

            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion && pion.Couleur == couleurEquipeRecue)
                {
                    if (!disponiblesParGrade.TryGetValue(pion.Grade, out var list))
                    {
                        list = new List<PictureBox>();
                        disponiblesParGrade.Add(pion.Grade, list);
                    }

                    list.Add(pb);
                }
            }

            // Affecter les positions reçues aux Tag des PB correspondantes (par grade)
            foreach (var kvp in positionsRecues)
            {
                var destination = kvp.Key;
                var pionRecu = kvp.Value;
                if (pionRecu == null)
                    continue;

                if (!disponiblesParGrade.TryGetValue(pionRecu.Grade, out var list) || list.Count == 0)
                    continue;

                var pb = list[0];
                list.RemoveAt(0);

                var pionUi = (personnage_base)pb.Tag;
                pionUi.PositionGrille = destination;

                pb.Location = CalculerPositionGraphique(destination.X, destination.Y);
            }

            // Rebuild pour cohérence interne
            RebuildDictionnairesDepuisUI();
        }

        public void RebuildDictionnairesDepuisUI()
        {
            var bleus = new Dictionary<Point, personnage_base>();
            var rouges = new Dictionary<Point, personnage_base>();

            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    if (pion.Couleur)
                        bleus[pion.PositionGrille] = pion;
                    else
                        rouges[pion.PositionGrille] = pion;
                }
            }

            PositionsPionsBleus = bleus;
            PositionsPionsRouges = rouges;
        }

        public void AppliquerCombat(Point positionAttaquant, Point positionDefenseur, char resultat)
        {
            PictureBox pbA = null;
            personnage_base pionA = null;

            PictureBox pbD = null;
            personnage_base pionD = null;

            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    if (pion.PositionGrille.Equals(positionAttaquant))
                    {
                        pbA = pb;
                        pionA = pion;
                    }
                    else if (pion.PositionGrille.Equals(positionDefenseur))
                    {
                        pbD = pb;
                        pionD = pion;
                    }
                }
            }

            if (resultat == 'A')
            {
                if (pbD != null && pionD != null)
                {
                    PnlGrilleGame.Controls.Remove(pbD);
                    SupprimerPion(pionD.PositionGrille, pionD.Couleur);
                }

                if (pbA != null && pionA != null)
                    DeplacerPion(pionA, pbA, positionDefenseur);

                return;
            }

            if (resultat == 'D')
            {
                if (pbA != null && pionA != null)
                {
                    PnlGrilleGame.Controls.Remove(pbA);
                    SupprimerPion(pionA.PositionGrille, pionA.Couleur);
                }

                return;
            }

            if (resultat == 'E')
            {
                if (pbD != null && pionD != null)
                {
                    PnlGrilleGame.Controls.Remove(pbD);
                    SupprimerPion(pionD.PositionGrille, pionD.Couleur);
                }

                if (pbA != null && pionA != null)
                {
                    PnlGrilleGame.Controls.Remove(pbA);
                    SupprimerPion(pionA.PositionGrille, pionA.Couleur);
                }

                return;
            }

            if (resultat == 'F')
            {
                // À minima : supprimer le défenseur (drapeau) et déplacer l'attaquant
                if (pbD != null && pionD != null)
                {
                    PnlGrilleGame.Controls.Remove(pbD);
                    SupprimerPion(pionD.PositionGrille, pionD.Couleur);
                }

                if (pbA != null && pionA != null)
                    DeplacerPion(pionA, pbA, positionDefenseur);

                // l'écran de fin de partie est géré par FicJeu
                return;
            }

            throw new InvalidOperationException("Résultat de combat inconnu : " + resultat);
        }
        public async Task Envoyer_CombatAsync(Point positionAttaquant, Point positionDefenseur, char resultatCombat, bool isServeur)
        {
            // Exemple d'implémentation basique, à adapter selon votre logique réseau
            if (isServeur && serveur != null)
            {
                await serveur.Envoyer_Combat(positionAttaquant, positionDefenseur, resultatCombat);
            }
            else if (!isServeur && client != null)
            {
                await client.Envoyer_Combat(positionAttaquant, positionDefenseur, resultatCombat);
            }
        }
        public async Task<(Point positionAttaquant, Point positionDefenseur, char resultatCombat)> Recevoir_CombatAsync(bool isServeur)
        {
            if (isServeur)
            {
                var (positionDepart, positionArrivee, resultatCombat) = await serveur.Reception_Combat();

                var dico = PositionsPionsRouges;

                PictureBox pb = null;
                personnage_base pionUi = null;

                foreach (Control ctrl in PnlGrilleGame.Controls)
                {
                    if (ctrl is PictureBox pic && pic.Tag is personnage_base tagPion)
                    {
                        if (tagPion.PositionGrille.Equals(positionDepart))
                        {
                            pb = pic;
                            pionUi = tagPion;
                            break;
                        }
                    }
                }

                if (pionUi != null)
                {
                    dico.Remove(positionDepart);
                    dico[positionArrivee] = pionUi;
                }

                return (positionDepart, positionArrivee, resultatCombat);
            }
            else
            {
                var (positionDepart, positionArrivee, resultatCombat) = await client.Recevoir_Combat();

                var dico = PositionsPionsBleus;

                PictureBox pb = null;
                personnage_base pionUi = null;

                foreach (Control ctrl in PnlGrilleGame.Controls)
                {
                    if (ctrl is PictureBox pic && pic.Tag is personnage_base tagPion)
                    {
                        if (tagPion.PositionGrille.Equals(positionDepart))
                        {
                            pb = pic;
                            pionUi = tagPion;
                            break;
                        }
                    }
                }

                if (pionUi != null)
                {
                    dico.Remove(positionDepart);
                    dico[positionArrivee] = pionUi;
                }

                return (positionDepart, positionArrivee, resultatCombat);
            }
        }
        public async Task<string> RecevoirMessageAsync(bool isServeur)
        {
            if (isServeur)
                return await serveur.RecevoirMessageAsync();

            return await client.RecevoirMessageAsync();
        }

        public static (Point depart, Point destination) ParserDeplacement(string message)
        {
            if (!message.StartsWith("DEPLACEMENT|", StringComparison.Ordinal))
                throw new Exception("DEPLACEMENT attendu");

            string data = message.Replace("DEPLACEMENT|", "");
            string[] positions = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (positions.Length != 2)
                throw new Exception("Format du déplacement invalide");

            string[] depart = positions[0].Split(';');
            string[] arrivee = positions[1].Split(';');

            return (
                new Point(int.Parse(depart[0]), int.Parse(depart[1])),
                new Point(int.Parse(arrivee[0]), int.Parse(arrivee[1]))
            );
        }

        public static (Point attaquant, Point defenseur, char resultat) ParserCombat(string message)
        {
            if (!message.StartsWith("COMBAT|", StringComparison.Ordinal))
                throw new Exception("COMBAT attendu");

            string data = message.Replace("COMBAT|", "");
            string[] parts = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                throw new Exception("Format COMBAT invalide");

            string[] a = parts[0].Split(';');
            string[] d = parts[1].Split(';');

            return (
                new Point(int.Parse(a[0]), int.Parse(a[1])),
                new Point(int.Parse(d[0]), int.Parse(d[1])),
                parts[2][0]
            );
        }

        public static bool TryParserDeplacement(string message, out Point depart, out Point destination)
        {
            depart = default(Point);
            destination = default(Point);

            if (!message.StartsWith("DEPLACEMENT|", StringComparison.Ordinal))
                return false;

            string data = message.Replace("DEPLACEMENT|", "");
            string[] positions = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (positions.Length != 2)
                return false;

            string[] d = positions[0].Split(';');
            string[] a = positions[1].Split(';');
            if (d.Length < 2 || a.Length < 2)
                return false;

            depart = new Point(int.Parse(d[0]), int.Parse(d[1]));
            destination = new Point(int.Parse(a[0]), int.Parse(a[1]));
            return true;
        }

        public static bool TryParserCombat(string message, out Point attaquant, out Point defenseur, out char resultat)
        {
            attaquant = default(Point);
            defenseur = default(Point);
            resultat = '\0';

            if (!message.StartsWith("COMBAT|", StringComparison.Ordinal))
                return false;

            string data = message.Replace("COMBAT|", "");
            string[] parts = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                return false;

            string[] a = parts[0].Split(';');
            string[] d = parts[1].Split(';');

            attaquant = new Point(int.Parse(a[0]), int.Parse(a[1]));
            defenseur = new Point(int.Parse(d[0]), int.Parse(d[1]));
            resultat = parts[2][0];
            return true;
        }

        public void AppliquerDeplacement(Point depart, Point destination)
        {
            PictureBox pb = null;
            personnage_base pion = null;

            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pic && pic.Tag is personnage_base tagPion)
                {
                    if (tagPion.PositionGrille.Equals(depart))
                    {
                        pb = pic;
                        pion = tagPion;
                        break;
                    }
                }
            }

            if (pb != null && pion != null)
                DeplacerPion(pion, pb, destination);
        }
        public void SauvegarderPartie(string filePath, Player joueurCourant)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            RebuildDictionnairesDepuisUI();

            var sb = new StringBuilder();

            sb.AppendLine("TURN=" + (joueurCourant == Player.Player_Blue ? "Blue" : "Red"));

            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    string team = pion.Couleur ? "B" : "R";
                    sb.AppendLine(string.Format(
                        CultureInfo.InvariantCulture,
                        "{0};{1};{2};{3}",
                        team,
                        pion.PositionGrille.X,
                        pion.PositionGrille.Y,
                        pion.Grade));
                }
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public void ChargerPartie(string filePath, out Player joueurCourant)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            // IMPORTANT: repartir d'une grille propre pour pouvoir restaurer aussi les pièces "mortes"
            Piece_Init();

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            joueurCourant = Player.Player_Blue;

            var positionsBleues = new Dictionary<Point, personnage_base>();
            var positionsRouges = new Dictionary<Point, personnage_base>();

            var positionsBleuesSet = new HashSet<Point>();
            var positionsRougesSet = new HashSet<Point>();

            foreach (string raw in lines)
            {
                string line = raw.Trim();
                if (line.Length == 0)
                    continue;

                if (line.StartsWith("TURN=", StringComparison.OrdinalIgnoreCase))
                {
                    string v = line.Substring("TURN=".Length).Trim();
                    joueurCourant = string.Equals(v, "Red", StringComparison.OrdinalIgnoreCase)
                        ? Player.Player_Red
                        : Player.Player_Blue;
                    continue;
                }

                string[] parts = line.Split(';');
                if (parts.Length < 4)
                    continue;

                string team = parts[0];
                int x = int.Parse(parts[1], CultureInfo.InvariantCulture);
                int y = int.Parse(parts[2], CultureInfo.InvariantCulture);
                string grade = parts[3];

                var pos = new Point(x, y);

                personnage_base tmp = CreerPersonnageTemporaire(
                    team.Equals("B", StringComparison.OrdinalIgnoreCase),
                    grade,
                    pos);

                if (team.Equals("B", StringComparison.OrdinalIgnoreCase))
                {
                    positionsBleues[pos] = tmp;
                    positionsBleuesSet.Add(pos);
                }
                else
                {
                    positionsRouges[pos] = tmp;
                    positionsRougesSet.Add(pos);
                }
            }

            // Appliquer les positions aux Tag existants
            AppliquerInitialisationAdverseSansId(positionsBleues, true);
            AppliquerInitialisationAdverseSansId(positionsRouges, false);

            // Supprimer les pièces absentes de la sauvegarde (mortes)
            SupprimerPiecesAbsentes(positionsBleuesSet, true);
            SupprimerPiecesAbsentes(positionsRougesSet, false);

            // Rebuild final
            RebuildDictionnairesDepuisUI();
            EnvoyerDataSave();
        }
        private async void EnvoyerDataSave()
        {
            //await EnvoyerPositionsPions();
            //envoyer position bleu n'envoie enfaite que le dictionnaire on peut envoyer le rouge ou le bleu 
            await serveur.EnvoyerPositionBleu(PositionsPionsBleus);
            await serveur.EnvoyerPositionBleu(PositionsPionsRouges);
            AppliquerInitialisationAdverseSansId(PositionsPionsRouges, false);
            AppliquerInitialisationAdverseSansId(PositionsPionsBleus, true);
        }
        public async void recevirDataSave()
        {
            PositionsPionsBleus = await client.ReceptionInitialisationBleu();
            PositionsPionsRouges = await client.ReceptionInitialisationRouge();
        }


        private static personnage_base CreerPersonnageTemporaire(bool couleur, string grade, Point position)
        {
            // Suffit pour que .Grade et .PositionGrille soient cohérents.
            // Le type exact n'est pas critique pour un mapping "par grade" au chargement.
            // On réutilise votre logique de création existante : côté bleu/rouge, le grade peut varier accentué/non accentué.
            switch (grade)
            {
                case "Drapeau": return new Drapeau(couleur, position);
                case "Bombe": return new Bombe(couleur, position);
                case "Espion": return new Espion(couleur, position);
                case "Eclaireur":
                case "Éclaireur": return new Eclaireur(couleur, position);
                case "Demineur":
                case "Démineur": return new Demineur(couleur, position);
                case "Sergent": return new Sergent(couleur, position);
                case "Lieutenant": return new Lieutenant(couleur, position);
                case "Capitaine": return new Capitaine(couleur, position);
                case "Commandant": return new Commandant(couleur, position);
                case "Colonel": return new Colonel(couleur, position);
                case "General":
                case "Général": return new Marechal(couleur, position);
                case "Marechal":
                case "Maréchal": return new Marechal(couleur, position);
                default: throw new Exception("Grade inconnu dans la sauvegarde : " + grade);
            }
        }

        private void SupprimerPiecesAbsentes(HashSet<Point> positionsExistantes, bool couleurEquipe)
        {
            var toRemove = new List<PictureBox>();

            foreach (Control ctrl in PnlGrilleGame.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion && pion.Couleur == couleurEquipe)
                {
                    if (!positionsExistantes.Contains(pion.PositionGrille))
                        toRemove.Add(pb);
                }
            }

            foreach (PictureBox pb in toRemove)
            {
                if (pb.Tag is personnage_base pion)
                    SupprimerPion(pion.PositionGrille, pion.Couleur);

                PnlGrilleGame.Controls.Remove(pb);
            }
        }
    } }
