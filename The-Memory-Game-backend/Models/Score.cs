namespace TheMemoryGameBackend.Models
{
    public class Score
    {
        public int Id { get; set; } // key
        public int UserId { get; set; } // foreign key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public TimeSpan ElapsedTime { get; set; }
        public User User { get; set; } // One-to-Many relationship
    }
}