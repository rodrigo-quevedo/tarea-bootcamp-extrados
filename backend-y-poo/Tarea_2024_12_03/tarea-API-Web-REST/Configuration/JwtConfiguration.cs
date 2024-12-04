namespace Configuration
{
    public class JwtConfiguration
    {
        public string secret {  get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
    }
}
