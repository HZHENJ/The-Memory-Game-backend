namespace TheMemoryGameBackend.Models 
{
    public class ScoreRequest
    {
        public int UserId { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }
    
}