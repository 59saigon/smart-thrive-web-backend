namespace SWD.SmartThrive.API.RequestModel
{
    public class UserVerifyEmail
    {
        public string Email { get; set; }
        public bool Email_verified { get; set; }

        public string GoogleToken { get; set; }
    }
}
