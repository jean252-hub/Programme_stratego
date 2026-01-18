using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon.Reseau
{
    public class ReseauServeur
    {
        public static string DefaultAdresseIP { get; set; } = "127.0.0.1";
        public static int DefaultPort { get; set; } = 11000;

        public string AdresseIP { get; set; }
        public int Port { get; set; }

        private Socket _clientHandler;

        public async Task DemarrerServeurAsync()
        {
            try { AllocConsole(); } catch { Console.WriteLine("echec du serveur"); }

            var ipToUse = string.IsNullOrWhiteSpace(AdresseIP) ? DefaultAdresseIP : AdresseIP;
            var portToUse = Port == 0 ? DefaultPort : Port;

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ipToUse), portToUse);
            Socket listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(1);

            Console.WriteLine("Serveur en attente...");
            Debug.WriteLine("Serveur en attente...");
            _clientHandler = await listener.AcceptAsync();
            Console.WriteLine("Client connecté");
        }
        public async Task EnvoyerPositionBleu(
       Dictionary<Point, personnage_base> pionsBleus)
        {
            if (_clientHandler == null || !_clientHandler.Connected)
                throw new InvalidOperationException("Client non connecté");

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append("INIT_PIONS|");

            foreach (var kvp in pionsBleus)
            {
                Point position = kvp.Key;
                personnage_base perso = kvp.Value;

                
                messageBuilder.Append(
                    $"{position.X};{position.Y};{perso.GetType().Name}|");
            }

            messageBuilder.Append("<|EOM|>");

            byte[] messageBytes =
                Encoding.UTF8.GetBytes(messageBuilder.ToString());

            await _clientHandler.SendAsync(
                new ArraySegment<byte>(messageBytes),
                SocketFlags.None);

            Console.WriteLine("Initialisation des pions bleus envoyée");
        }
        public async Task<(Point positionDepart, Point positionArrivee)> Reception_Deplacement()
        {
            Debug.WriteLine("Réception d'un déplacement démarrée...");
            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientHandler.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }

            string message = messageComplet.ToString().Replace("<|EOM|>", "");
            Debug.WriteLine("Message complet reçu : " + message);

            if (!message.StartsWith("DEPLACEMENT|"))
            {
                Debug.WriteLine("Message invalide reçu : " + message);
                throw new Exception("DEPLACEMENT attendu");
            }

            string data = message.Replace("DEPLACEMENT|", "");
            string[] positions = data.Split(
                new char[] { '|' },
                StringSplitOptions.RemoveEmptyEntries);

            if (positions.Length != 2)
                throw new Exception("Format du déplacement invalide");

            string[] depart = positions[0].Split(';');
            string[] arrivee = positions[1].Split(';');

            Point positionDepart = new Point(int.Parse(depart[0]), int.Parse(depart[1]));
            Point positionArrivee = new Point(int.Parse(arrivee[0]), int.Parse(arrivee[1]));

            // Envoyer un ACK au client
            //byte[] ack = Encoding.UTF8.GetBytes("<|ACK|>");
            //await _clientHandler.SendAsync(new ArraySegment<byte>(ack), SocketFlags.None);

            Debug.WriteLine($"Déplacement reçu : ({positionDepart.X},{positionDepart.Y}) -> ({positionArrivee.X},{positionArrivee.Y})");
            return (positionDepart, positionArrivee);
        }

        public async Task Envoyer_Deplacement(
    personnage_base info,
    PictureBox pionSelectionne,
    Point nouvellePosition)
        {
            if (_clientHandler == null)
                throw new InvalidOperationException("Aucun client connecté");

            Point positionDepart = info.PositionGrille;

            string message = $"DEPLACEMENT|{positionDepart.X};{positionDepart.Y}|{nouvellePosition.X};{nouvellePosition.Y}|<|EOM|>";

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _clientHandler.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

            Console.WriteLine($"Déplacement envoyé au client : ({positionDepart.X},{positionDepart.Y}) -> ({nouvellePosition.X},{nouvellePosition.Y})");
           // Log($"Déplacement envoyé au client : ({positionDepart.X},{positionDepart.Y}) -> ({nouvellePosition.X},{nouvellePosition.Y})");
        }



        public async Task<Dictionary<Point, personnage_base>> ReceptionInitialisationRouge()
        {
            Debug.WriteLine("Réception de l'initialisation des pions rouges demarrage");
            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientHandler.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }
            Debug.WriteLine("Réception de l'initialisation des pions rouges fin de la boucle");
            Debug.WriteLine("Message complet reçu : " + messageComplet.ToString());

            string message = messageComplet.ToString().Replace("<|EOM|>", "");

            if (!message.StartsWith("INIT_PIONS|")) { Debug.WriteLine("Message invalide reçu"); }
           
                //throw new Exception("INIT_PIONS attendu");

            Dictionary<Point, personnage_base> pionsRouges =
                new Dictionary<Point, personnage_base>();

            string data = message.Replace("INIT_PIONS|", "");

            string[] pions = data.Split(
                new char[] { '|' },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string pion in pions)
            {
                string[] infos = pion.Split(';');

                int x = int.Parse(infos[0]);
                int y = int.Parse(infos[1]);
                string grade = infos[2];

                Point position = new Point(x, y);

                personnage_base perso = CreerPersonnageRouge(grade, position);

                pionsRouges.Add(position, perso);
                Debug.WriteLine($"Pion rouge ajouté : {grade} en ({x},{y})");
            }

            byte[] ack = Encoding.UTF8.GetBytes("<|ACK|>");
            await _clientHandler.SendAsync(new ArraySegment<byte>(ack), SocketFlags.None);
            Console.WriteLine("Initialisation des pions rouges reçue");
            Debug.WriteLine("Réception de l'initialisation des pions rouges fin");
            return pionsRouges;
        
        }


        private personnage_base CreerPersonnageRouge(string grade, Point position)
        {
            switch (grade)
            {
                case "Drapeau":
                    return new Drapeau(false, position);

                case "Bombe":
                    return new Bombe(false, position);

                case "Espion":
                    return new Espion(false, position);

                case "Eclaireur":
                case "Éclaireur":
                    return new Eclaireur(false, position);

                case "Demineur":
                case "Démineur":
                    return new Demineur(false, position);

                case "Sergent":
                    return new Sergent(false, position);

                case "Lieutenant":
                    return new Lieutenant(false, position);

                case "Capitaine":
                    return new Capitaine(false, position);

                case "Commandant":
                    return new Commandant(false, position);

                case "Colonel":
                    return new Colonel(false, position);

                case "General":
                case "Général":
                    return new General(false, position);

                case "Marechal":
                case "Maréchal":
                    return new Marechal(false, position);

                default:
                    throw new Exception($"Grade inconnu reçu : '{grade}'");
            }
        }


        public async Task Envoyer_Combat(Point attaquant, Point defenseur, char resultat)
        {
            if (_clientHandler == null || !_clientHandler.Connected)
                throw new InvalidOperationException("Aucun client connecté");

            string message = $"COMBAT|{attaquant.X};{attaquant.Y}|{defenseur.X};{defenseur.Y}|{resultat}|<|EOM|>";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            Debug.WriteLine("Envoi du message de combat : " + message);

            await _clientHandler.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
        }

        public async Task<(Point attaquant, Point defenseur, char resultat)> Reception_Combat()
        {
            if (_clientHandler == null || !_clientHandler.Connected)
                throw new InvalidOperationException("Aucun client connecté");

            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientHandler.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));
                Debug.WriteLine("Message partiel reçu : " + messageComplet.ToString());

                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }

            string message = messageComplet.ToString().Replace("<|EOM|>", "");

            if (!message.StartsWith("COMBAT|", StringComparison.Ordinal))
                throw new Exception("COMBAT attendu");

            string data = message.Replace("COMBAT|", "");
            string[] parts = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
                throw new Exception("Format COMBAT invalide");

            string[] a = parts[0].Split(';');
            string[] d = parts[1].Split(';');

            Point attaquant = new Point(int.Parse(a[0]), int.Parse(a[1]));
            Point defenseur = new Point(int.Parse(d[0]), int.Parse(d[1]));
            char resultat = parts[2][0];

            return (attaquant, defenseur, resultat);
        }

        public async Task<string> RecevoirMessageAsync()
        {
            if (_clientHandler == null || !_clientHandler.Connected)
                throw new InvalidOperationException("Client non connecté");

            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientHandler.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                int eomIndex = messageComplet.ToString().IndexOf("<|EOM|>", StringComparison.Ordinal);
                if (eomIndex >= 0)
                    return messageComplet.ToString().Substring(0, eomIndex);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
