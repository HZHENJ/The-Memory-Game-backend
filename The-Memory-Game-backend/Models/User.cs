namespace TheMemoryGameBackend.Models
{
    public class User
    {
        public int Id { get; set; } // 主键
        public string Email { get; set; } // 用户邮箱
        public string Password { get; set; } // 用户密码
    }
}
