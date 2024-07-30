namespace TheProjectTascamon.Models
{
    public class TypeClass
    {
        public GameType Name { get; set; }
    }

    public enum GameType
    {
        Fire,
        Water,
        Grass,
        // Other types...
    }
}