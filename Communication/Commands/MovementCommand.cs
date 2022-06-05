namespace Communication.Commands;

public class MovementCommand: Command
{
    private readonly float x;
    private readonly float y;
    
    public MovementCommand(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public override CommandName GetName()
    {
        return CommandName.Move;
    }
}
