namespace Communication.Data
{
    [Serializable]
    public class Character
    {
        public readonly string id;

        public float x;
        public float y;

        public float movementX;
        public float movementY;

        public Character(string id) {
            this.id = id;
        }
    }
}
