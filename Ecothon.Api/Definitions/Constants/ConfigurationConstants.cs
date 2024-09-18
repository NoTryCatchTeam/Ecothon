namespace Ecothon.Api.Definitions.Constants;

public class ConfigurationConstants
{
    public class Secrets
    {
        public class Jwt
        {
            public const string SECRET = "Jwt:Secret";

            public const string ISSUER = "Jwt:Issuer";

            public const string AUDIENCE = "Jwt:Audience";

            public const string ACCESS_TOKEN_EXPIRES_IN_MINS = "Jwt:AccessTokenExpiresInMins";

            public const string REFRESH_TOKEN_EXPIRES_IN_MINS = "Jwt:RefreshTokenExpiresInMins";
        }

        public class Db
        {
            public const string DB_USERNAME = "Db:Username";

            public const string DB_PASSWORD = "Db:Password";

            public const string DB_HOST = "Db:Host";

            public const string DB_DB_NAME = "Db:DbName";
        }
    }

    public class AppSettings
    {
        public class Db
        {
            public const string CONNECTION_STRING = "Db:ConnectionString";
        }
    }
}
