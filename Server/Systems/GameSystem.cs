namespace Server.Systems;

internal interface IGameSystem
{
    public void Run(GameState gameState, float deltaTime);
}
