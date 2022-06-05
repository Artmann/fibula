using Communication.Commands;
using Communication.Data;
using Newtonsoft.Json;
using System.Text;
using WatsonWebsocket;

namespace Fibula.Client;

public class Client
{
    public delegate void GameStateUpdateDelegate(GameState gameState);
    public GameStateUpdateDelegate OnGameStateUpdated;

    public delegate void LogMessageDelegate(string message);
    public LogMessageDelegate OnLogMessage;

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
        Log("Starting client.");
        webSocketClient.Start();
    }

    public void Move(float x, float y)
    {
        SendCommand(new MovementCommand(x, y));
    }

    private void Log(string message)
    {
        OnLogMessage?.Invoke(message);
    }

    private void SendCommand(Command command)
    {
        var json = JsonConvert.SerializeObject(command);

        webSocketClient.SendAsync(json);
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs args)
    {
        var data = Encoding.UTF8.GetString(args.Data);

        Log(data);

        if (data == null)
        {
            throw new Exception("Empty message.");
        }
        
        var parts = data.Split(";", 2);

        var message = parts[0];
        var json = parts[1];

        Log($"Received message: {message}");

        if (message == "game-state")
        {
            var gameState = JsonConvert.DeserializeObject<GameState>(json);

            if (gameState == null)
            {
                throw new Exception("Game state is null.");
            }

            OnGameStateUpdated?.Invoke(gameState);
        }
        
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
