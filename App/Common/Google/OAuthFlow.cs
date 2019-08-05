using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;

namespace GMaster.Common.Google
{
    public class OAuthFlow
    {
        public IAuthorizationCodeFlow Flow { get; } = new GoogleAuthorizationCodeFlow(
            new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = Settings.Google.OAuth2.clientId,
                    ClientSecret = Settings.Google.OAuth2.secret
                },
                Scopes = new string[]{ //authorization scopes for Google API access
                    "https://www.googleapis.com/auth/plus.me",
                    "https://www.googleapis.com/auth/gmail.send",
                    "https://www.googleapis.com/auth/gmail.labels"
                },
                DataStore = new FileDataStore("Content/Drive.Api.Auth.Store")
            }
        );
    }
}
