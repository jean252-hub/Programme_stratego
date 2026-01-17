using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;


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

    }
}
