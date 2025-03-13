namespace Configuration.Jwt
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string jwt_secret { get; set; }
        public string refreshToken_secret { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }

        public JwtConfiguration() { }

        public JwtConfiguration(string jwt_secret, string refreshToken_secret, string issuer, string audience)
        {
            this.jwt_secret = jwt_secret;
            this.refreshToken_secret = refreshToken_secret;
            this.issuer = issuer;
            this.audience = audience;
        }
    }
}
