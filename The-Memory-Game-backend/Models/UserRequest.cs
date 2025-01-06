namespace TheMemoryGameBackend.Models
{
    public class UserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRequestWithId
    {
        public int Id { get; set; }
    }

    public class RegisterRequest
    {
        public string Username {get; set;}
        public string Password {get; set;}
    }

    public class LoginRequest 
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}