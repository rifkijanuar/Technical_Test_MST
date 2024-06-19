namespace Technical_Test_MST_Back_End.Models
{
    public class Users : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email {  get; set; }
        public string Fullname { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiredToken { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
    }
}
