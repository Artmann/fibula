using Communication.Data;

namespace Server
{
    [Serializable]
    public class GameState
    {
        public List<Character> characters = new();
    }
}
