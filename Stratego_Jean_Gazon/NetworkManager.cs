csharp Network/NetworkManager.cs
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Stratego_Jean_Gazon.Network
{
    public class NetworkManager : IDisposable
    {
        private TcpListener listener;
        private TcpClient client;
        private StreamWriter writer;
        private CancellationTokenSource cts;
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        public bool IsConnected => client != null && client.Connected;

        // messageType, payload (deserialized concrete type)
        public event Action<MessageType, object> MessageReceived;

        public NetworkManager()
        {
        }

        public async Task StartServerAsync(int port)
        {
            cts = new CancellationTokenSource();
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            var tcp = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
            StartClientLoop(tcp, cts.Token);
        }

        public async Task ConnectToServerAsync(string host, int port)
        {
            cts = new CancellationTokenSource();
            client = new TcpClient();
            await client.ConnectAsync(host, port).ConfigureAwait(false);
            StartClientLoop(client, cts.Token);
        }

        private void StartClientLoop(TcpClient tcp, CancellationToken token)
        {
            client = tcp;
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            // read loop
            Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        var line = await reader.ReadLineAsync().ConfigureAwait(false);
                        if (line == null) break;
                        try
                        {
                            var env = serializer.Deserialize<JsonEnvelope>(line);
                            if (Enum.TryParse(env.Type, out MessageType mt))
                            {
                                switch (mt)
                                {
                                    case MessageType.Move:
                                        var mv = serializer.Deserialize<MoveMessage>(env.PayloadJson);
                                        MessageReceived?.Invoke(mt, mv);
                                        break;
                                    case MessageType.PlacementDone:
                                        MessageReceived?.Invoke(mt, null);
                                        break;
                                    case MessageType.SyncState:
                                        MessageReceived?.Invoke(mt, env.PayloadJson);
                                        break;
                                }
                            }
                        }
                        catch { /* ignore parse errors for now */ }
                    }
                }
                catch { }
                finally
                {
                    Dispose();
                }
            }, token);
        }

        public Task SendAsync<T>(T payload, MessageType type)
        {
            if (writer == null) return Task.CompletedTask;
            var payloadJson = serializer.Serialize(payload);
            var envelope = new JsonEnvelope { Type = type.ToString(), PayloadJson = payloadJson };
            var full = serializer.Serialize(envelope);
            try
            {
                lock (writer)
                {
                    writer.WriteLine(full);
                }
            }
            catch { /* swallow for now */ }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            try { cts?.Cancel(); } catch { }
            try { writer?.Close(); } catch { }
            try { client?.Close(); } catch { }
            try { listener?.Stop(); } catch { }
            cts = null;
            client = null;
            listener = null;
            writer = null;
        }
    }
}