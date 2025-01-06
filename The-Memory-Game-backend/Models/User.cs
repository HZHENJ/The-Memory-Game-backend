namespace TheMemoryGameBackend.Models
{
    public class User
    {
        public int Id { get; set; } // key
        public string Password { get; set; } // password
        public string Username { get; set; } // username
        public int Type { get; set; } // type 0 for free user, 1 for paid user
        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
