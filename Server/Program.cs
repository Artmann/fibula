using Communication;
using Server.Systems;
using SimpleInjector;

var container = new Container();

// Server
container.Register<Server.Server>();

// Game
container.Register<Server.Game>();

container.Register<CharacterMovementSystem>();

container.Register<SpeedCalculator>();

container.Verify();

var game = container.GetInstance<Server.Game>();

var server = container.GetInstance<Server.Server>();

game.GameStateChanged += server.OnGameStateChanged;

server.Start();

game.Start();

