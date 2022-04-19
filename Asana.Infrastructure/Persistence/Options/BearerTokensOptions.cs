namespace Asana.Infrastructure.Persistence.Options
{
   public class BearerTokensOptions
    {
        public const string JWtBearer = "JWtBearer";

        public string Key { set; get; }

        public string Issuer { set; get; }

        public string Audience { set; get; }

        public int AccessTokenExpirationSeconds { set; get; }

        public int RefreshTokenExpirationSeconds { set; get; }
    }
}
