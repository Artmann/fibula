using Communication.Commands;
using Newtonsoft.Json;
using System.Text;
using WatsonWebsocket;

namespace Client;

public class Client
{
    private readonly WatsonWsClient webSocketClient;

    public Client(string host = "localhost", int port = 9009)
    {
        webSocketClient = new WatsonWsClient(host, port, false);

        webSocketClient.ServerConnected += ServerConnected;
        webSocketClient.ServerDisconnected += ServerDisconnected;
        webSocketClient.MessageReceived += MessageReceived;
    }

    public void Start()
    {
        webSocketClient.Start();
    }

    public void Move(float x, float y)
    {
        SendCommand(new MovementCommand(x, y));
    }

    private void SendCommand(Command command)
    {
        var json = JsonConvert.SerializeObject(command);

        webSocketClient.SendAsync(json);
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs args)
    {
        Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(args.Data));
    }

    private void ServerConnected(object sender, EventArgs args)
    {
        Console.WriteLine("Server connected");
    }

    private void ServerDisconnected(object sender, EventArgs args)
    {
        Console.WriteLine("Server disconnected");
    }
}
