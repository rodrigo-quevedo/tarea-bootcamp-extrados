namespace Configuration
{
    public class JwtConfiguration
    {
        public string jwt_secret {  get; set; }
        public string refreshToken_secret { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
    }
}
