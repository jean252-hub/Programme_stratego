using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Stratego_Jean_Gazon.Reseau
{
    public class ReseauClient
    {
        private static bool _logInitialise = false;
        private static string _logFilePath = "log_reseau_client.txt";

        // Valeurs globales modifiables par l'UI
        public static string DefaultAdresseIP { get; set; } = "127.0.0.1";
        public static int DefaultPort { get; set; } = 11000;

        public string AdresseIP { get; set; }
        public int Port { get; set; }

        // Socket persistant
        private Socket _clientSocket;

        // =========================
        // Connexion au serveur
        // =========================
        public async Task DemarrerClientAsync()
        {
            Log("Tentative de connexion");
            var ipToUse = string.IsNullOrWhiteSpace(AdresseIP) ? DefaultAdresseIP : AdresseIP;
            var portToUse = Port == 0 ? DefaultPort : Port;

            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipToUse), portToUse);

            _clientSocket = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            await Task.Run(() => _clientSocket.Connect(ipEndPoint));

            Log("Client connecté au serveur");

            // Handshake
           // await EnvoyerAsync("HELLO<|EOM|>");

            string response = await RecevoirAsync();
            if (response != "<|ACK|>")
                throw new Exception("ACK non reçu du serveur");
        }

        // =========================
        // Envoi message générique
        // =========================
        private async Task EnvoyerAsync(string message)
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            byte[] data = Encoding.UTF8.GetBytes(message);
            await Task.Run(() => _clientSocket.Send(data));
        }

        // =========================
        // Réception message
        // =========================
        private async Task<string> RecevoirAsync()
        {
            var buffer = new byte[1024];
            int received = await Task.Run(() => _clientSocket.Receive(buffer));
            return Encoding.UTF8.GetString(buffer, 0, received);
        }

        // =========================
        // ENVOI INITIALISATION PIONS
        // =========================
        public async Task Envoyer_Deplacement(
    personnage_base info,
    PictureBox pionSelectionne,
    Point nouvellePosition)
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            // On envoie les positions de départ et d'arrivée
            Point positionDepart = info.PositionGrille;

            string message = $"DEPLACEMENT|{positionDepart.X};{positionDepart.Y}|{nouvellePosition.X};{nouvellePosition.Y}|<|EOM|>";

            await EnvoyerAsync(message);

            Console.WriteLine($"Déplacement envoyé : ({positionDepart.X},{positionDepart.Y}) -> ({nouvellePosition.X},{nouvellePosition.Y})");
            Log($"Déplacement envoyé de ({positionDepart.X},{positionDepart.Y}) à ({nouvellePosition.X},{nouvellePosition.Y})");
        }
        public async Task<(Point positionDepart, Point positionArrivee)> Recevoir_Deplacement()
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            Debug.WriteLine("Réception d'un déplacement du serveur...");

            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }

            string message = messageComplet.ToString().Replace("<|EOM|>", "");
            Debug.WriteLine("Message complet reçu : " + message);

            if (!message.StartsWith("DEPLACEMENT|"))
                throw new Exception("DEPLACEMENT attendu");

            string data = message.Replace("DEPLACEMENT|", "");
            string[] positions = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (positions.Length != 2)
                throw new Exception("Format du déplacement invalide");

            string[] depart = positions[0].Split(';');
            string[] arrivee = positions[1].Split(';');

            Point positionDepart = new Point(int.Parse(depart[0]), int.Parse(depart[1]));
            Point positionArrivee = new Point(int.Parse(arrivee[0]), int.Parse(arrivee[1]));

            // Envoyer un ACK au serveur
            //byte[] ack = Encoding.UTF8.GetBytes("<|ACK|>");
            //await _clientSocket.SendAsync(new ArraySegment<byte>(ack), SocketFlags.None);

            Debug.WriteLine($"Déplacement reçu du serveur : ({positionDepart.X},{positionDepart.Y}) -> ({positionArrivee.X},{positionArrivee.Y})");
            return (positionDepart, positionArrivee);
        }


        public async Task Envoyer_Initialisation_Pions(
            Dictionary<Point, personnage_base> PionsRouge)
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            var sb = new StringBuilder();
            sb.Append("INIT_PIONS|");

            foreach (var pion in PionsRouge)
            {
                Point position = pion.Key;
                personnage_base perso = pion.Value;

                sb.Append($"{position.X};{position.Y};{perso.GetType().Name}|");
            }

            sb.Append("<|EOM|>");

            await EnvoyerAsync(sb.ToString());

            Console.WriteLine("Initialisation des pions envoyée");
            Log("Initialisation des Pions envoyer !");

        }


        // =========================
        // Fermeture propre
        // =========================
        public void FermerConnexion()
        {
            if (_clientSocket != null)
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
                _clientSocket = null;

                Console.WriteLine("Connexion fermée");
            }
        }
        private void Log(string message)
        {
            if (!_logInitialise)
            {
                File.WriteAllText(_logFilePath, string.Empty);

                Debug.Listeners.Add(new TextWriterTraceListener(_logFilePath));
                Debug.AutoFlush = true;

                Debug.WriteLine("===== NOUVEAU LOG RESEAU =====");
                Debug.WriteLine($"Timestamp: {DateTime.Now}");
                Debug.WriteLine("=============================");

                _logInitialise = true;
            }

            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public async Task<Dictionary<Point, personnage_base>> ReceptionInitialisationBleu()
        {
            Debug.WriteLine("Réception de l'initialisation des pions bleus démarrage");

            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            // Lire le message complet
            while (true)
            {
                int received = await _clientSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }

            Debug.WriteLine("Message complet reçu : " + messageComplet.ToString());

            // Nettoyer le message
            string message = messageComplet.ToString().Replace("<|EOM|>", "");

            if (!message.StartsWith("INIT_PIONS|"))
            {
                Debug.WriteLine("Message invalide reçu");
                return null; // ou throw new Exception("INIT_PIONS attendu");
            }

            Dictionary<Point, personnage_base> pionsBleus = new Dictionary<Point, personnage_base>();

            string data = message.Replace("INIT_PIONS|", "");

            string[] pions = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pion in pions)
            {
                string[] infos = pion.Split(';');
                if (infos.Length != 3)
                {
                    Debug.WriteLine("Format pion invalide : " + pion);
                    continue;
                }

                int x = int.Parse(infos[0]);
                int y = int.Parse(infos[1]);
                string classe = infos[2]; // nom de la classe reçue par le serveur

                Point position = new Point(x, y);

                personnage_base perso = CreerPersonnageBleu(classe, position);

                pionsBleus.Add(position, perso);

                Debug.WriteLine($"Pion bleu ajouté : {classe} en ({x},{y})");
            }

            // ACK
            //byte[] ack = Encoding.UTF8.GetBytes("<|ACK|>");
            //await _clientSocket.SendAsync(new ArraySegment<byte>(ack), SocketFlags.None);

            Console.WriteLine("Initialisation des pions bleus reçue");
            Debug.WriteLine("Réception de l'initialisation des pions bleus fin");

            return pionsBleus;
        }

        // Factory pour créer les personnages côté client (bleu)
        private personnage_base CreerPersonnage(string grade, bool couleur, Point position)
{
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
        case "Général": return new General(couleur, position);

        case "Marechal":
        case "Maréchal": return new Marechal(couleur, position);

        default:
            throw new Exception("Grade inconnu : " + grade);
    }
}

private personnage_base CreerPersonnageBleu(string grade, Point position)
{
    return CreerPersonnage(grade, true, position);
}

private personnage_base CreerPersonnageRouge(string grade, Point position)
{
    return CreerPersonnage(grade, false, position);
}

        public async Task Envoyer_Combat(Point attaquant, Point defenseur, char resultat)
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            string message = $"COMBAT|{attaquant.X};{attaquant.Y}|{defenseur.X};{defenseur.Y}|{resultat}|<|EOM|>";
            Debug.WriteLine("Envoi du message de combat : " + message);
            await EnvoyerAsync(message);
        }

        public async Task<(Point attaquant, Point defenseur, char resultat)> Recevoir_Combat()
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }
            Debug.WriteLine("Message complet reçu : " + messageComplet.ToString());

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
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            byte[] buffer = new byte[1024];
            StringBuilder messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    SocketFlags.None);

                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));

                int eomIndex = messageComplet.ToString().IndexOf("<|EOM|>", StringComparison.Ordinal);
                if (eomIndex >= 0)
                {
                    // Ne garder que le premier message (le reste serait pour une boucle continue plus tard)
                    return messageComplet.ToString().Substring(0, eomIndex);
                }
            }
        }

        public async Task<Dictionary<Point, personnage_base>> ReceptionInitialisationRouge()
        {
            if (_clientSocket == null || !_clientSocket.Connected)
                throw new InvalidOperationException("Socket non connecté");

            byte[] buffer = new byte[1024];
            var messageComplet = new StringBuilder();

            while (true)
            {
                int received = await _clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                messageComplet.Append(Encoding.UTF8.GetString(buffer, 0, received));
                if (messageComplet.ToString().Contains("<|EOM|>"))
                    break;
            }

            string message = messageComplet.ToString().Replace("<|EOM|>", "");
            if (!message.StartsWith("INIT_PIONS|"))
                throw new Exception("INIT_PIONS attendu");

            var pionsRouges = new Dictionary<Point, personnage_base>();
            string data = message.Replace("INIT_PIONS|", "");
            string[] pions = data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pion in pions)
            {
                string[] infos = pion.Split(';');
                if (infos.Length != 3)
                    continue;

                int x = int.Parse(infos[0]);
                int y = int.Parse(infos[1]);
                string grade = infos[2];

                Point position = new Point(x, y);
                pionsRouges.Add(position, CreerPersonnageRouge(grade, position));
            }

            return pionsRouges;
        }
    }

}
