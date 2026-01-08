using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Stratego_Jean_Gazon.Reseau
{
    public class ReseauClient
    {
        // Valeurs globales par défaut que l'UI peut modifier
        public static string DefaultAdresseIP { get; set; } = "127.0.0.1";
        public static int DefaultPort { get; set; } = 11000;

        public string AdresseIP { get; set; }
        public int Port { get; set; }

        public async Task DemarrerClientAsync()
        {
            // Si les propriétés d'instance ne sont pas définies, utiliser les valeurs par défaut
            var ipToUse = string.IsNullOrWhiteSpace(AdresseIP) ? DefaultAdresseIP : AdresseIP;
            var portToUse = Port == 0 ? DefaultPort : Port;

            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipToUse), portToUse);

            using (var client = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connecter sans bloquer l'UI en effectuant le Connect sur un thread de pool
                await Task.Run(() => client.Connect(ipEndPoint));

                while (true)
                {
                    // Envoyer le message (Send synchrone utilisé ici pour compatibilité .NET Framework 4.8)
                    var message = "Hi friends 👋!<|EOM|>";
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    await Task.Run(() => client.Send(messageBytes));
                    Console.WriteLine($"Socket client sent message: \"{message}\"");

                    // Recevoir l'ack
                    var buffer = new byte[1_024];
                    var received = await Task.Run(() => client.Receive(buffer));
                    var response = Encoding.UTF8.GetString(buffer, 0, received);
                    if (response == "<|ACK|>")
                    {
                        Console.WriteLine($"Socket client received acknowledgment: \"{response}\"");
                        break;
                    }
                }

                client.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
