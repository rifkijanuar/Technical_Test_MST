namespace Technical_Test_MST_Back_End.Models.Request
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
