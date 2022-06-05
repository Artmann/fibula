using Communication.Data;
using Server.Systems;
using System.Diagnostics;

namespace Server;

public class Game
{
    public delegate void OnGameStateUpdated(GameState gameState);
    public OnGameStateUpdated GameStateChanged;

    private readonly List<IGameSystem> systems = new();

    private readonly CharacterMovementSystem characterMovementSystem;

    private bool IsRunning;

    public Game(CharacterMovementSystem characterMovementSystem)
    {
        this.characterMovementSystem = characterMovementSystem;
    }

    public void Start()
    {
        Console.WriteLine("Boostraping Server.");

        Bootstrap();

        IsRunning = true;

        var gameState = new GameState();

        gameState.characters.Add(new Character("character-1") { x = -5f });
        gameState.characters.Add(new Character("character-2") { x = 5f });

        const int ticksPerSecond = 32;

        var timePerFrame = 1f / ticksPerSecond;

        var stopwatch = new Stopwatch();

        var timings = new List<float>();

        Console.WriteLine("Game is running.");

        Console.WriteLine($"Running at {ticksPerSecond} ticks ({timePerFrame}s).");

        while (IsRunning)
        {
            stopwatch.Restart();

            foreach (var system in systems)
            {
                system.Run(gameState, timePerFrame);
            }

            GameStateChanged.Invoke(gameState);

            stopwatch.Stop();
            
            var elapsed = stopwatch.ElapsedMilliseconds / 1000f;
            var timeLeft = timePerFrame - elapsed;

            timings.Add(elapsed);

            if (timeLeft > 0)
            {
                Thread.Sleep((int)Math.Round(timeLeft * 1000));
            }

            if (timings.Count > 5f / timePerFrame)
            {
                var average = timings.Average();
                
                Console.WriteLine($"Average Loop Time: {average}s.");
                
                timings.Clear();
            }
        }
    }

    public void Stop()
    {
        IsRunning = false;
    }

    private void Bootstrap()
    {
        systems.Add(characterMovementSystem);
    }
}
