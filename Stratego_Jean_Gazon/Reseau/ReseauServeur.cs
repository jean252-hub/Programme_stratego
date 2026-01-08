using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace Stratego_Jean_Gazon.Reseau
{
    public class ReseauServeur
    {
        // Valeurs globales par défaut que l'UI peut modifier
        public static string DefaultAdresseIP { get; set; } = "127.0.0.1";
        public static int DefaultPort { get; set; } = 11000;

         public string AdresseIP { get; set; }
        public int Port { get; set; }

        public async Task DemarrerServeurAsync()
        {
            // Ouvrir une console si elle n'existe pas (utile pour déboguer)
            try { AllocConsole(); } catch { /* Ignorer si impossible */ }

            // Si les propriétés d'instance ne sont pas définies, utiliser les valeurs par défaut
            var ipToUse = string.IsNullOrWhiteSpace(AdresseIP) ? DefaultAdresseIP : AdresseIP;
            var portToUse = Port == 0 ? DefaultPort : Port;

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ipToUse), portToUse);
            Socket listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipEndPoint);
            listener.Listen(100);
            var handler = await listener.AcceptAsync();
            Console.WriteLine("serveur ok");
            while (true)
            {
                var buffer_Receive_commande = new byte[1024];
                var received = await handler.ReceiveAsync(new ArraySegment<byte>(buffer_Receive_commande), SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer_Receive_commande, 0, received);

                var eom = "<|EOM|>";
                if (response.IndexOf(eom) > -1 /* is end of message */)
                {
                    Console.WriteLine(
                        $"Socket server received message: \"{response.Replace(eom, "")}\"");

                    var ackMessage = "<|ACK|>";
                    var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await handler.SendAsync(new ArraySegment<byte>(echoBytes), SocketFlags.None);
                    Console.WriteLine(
                        $"Socket server sent acknowledgment: \"{ackMessage}\"");

                    break;
                }
            }

            Console.WriteLine("Serveur démarré.");
           
        }

        // Permet d'ouvrir une console pour voir les Console.WriteLine depuis une appli WinForms
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
