using Communication;

namespace Server.Systems;

public class CharacterMovementSystem : IGameSystem
{
    private readonly SpeedCalculator speedCalculator;

    public CharacterMovementSystem(SpeedCalculator speedCalculator)
    {
        this.speedCalculator = speedCalculator;
    }

    public void Run(GameState gameState, float deltaTime)
    {
        foreach (var character in gameState.characters)
        {
            var speed = speedCalculator.GetSpeed(character) * deltaTime;
            
            var movementX = character.movementX * speed;
            var movementY = character.movementY * speed;

            character.x += movementX;
            character.y += movementY;
        }
    }
}
