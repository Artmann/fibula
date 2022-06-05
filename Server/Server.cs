using Newtonsoft.Json;
using System.Text;
using WatsonWebsocket;

namespace Server
{
    public class Server
    {
        private readonly WatsonWsServer webSocketServer;

        private readonly List<string> clients = new();

        public Server()
        {
            webSocketServer = new WatsonWsServer("localhost", 9009, false);

            webSocketServer.ClientConnected += ClientConnected;
            webSocketServer.ClientDisconnected += ClientDisconnected;
            webSocketServer.MessageReceived += MessageReceived;
        }

        public void Start()
        {
            Console.WriteLine("Server is running.");
            
            webSocketServer.Start();
        }

        public void OnGameStateChanged(GameState gameState)
        {
            var json = JsonConvert.SerializeObject(gameState);

            Broadcast(json);
        }

        private void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.IpPort);

            clients.Add(args.IpPort);
        }

        private void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.IpPort);
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message received from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        private void Broadcast(string json)
        {
            foreach (var client in clients) {
                webSocketServer.SendAsync(client, json);
            }
        }
    }
}
