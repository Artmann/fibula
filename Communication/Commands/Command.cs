namespace Communication.Commands;

public enum CommandName
{
    Move = 1
}

[Serializable]
public abstract class Command
{
    public abstract CommandName GetName();
}
