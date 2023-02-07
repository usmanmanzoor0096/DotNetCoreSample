namespace AuthService.Common.Models.ConfigModels
{
    public class JwtConfig
    {
        public string JWTKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInMinutes { get; set; }
        public int Expiration { get; set; }

    }
}
