using System.Threading;
using Google.Apis.Auth.OAuth2;

namespace GMaster.Common.Google
{
    public static class Credential
    {
        public static GoogleCredential GetGoogleCredential(string userId)
        {
            var oAuthFlow = new OAuthFlow();
            var tokens = oAuthFlow.Flow.LoadTokenAsync(userId, CancellationToken.None).Result;
            var credential = GoogleCredential.FromAccessToken(tokens.AccessToken);
            return credential;
        }

        public static GoogleCredential GetGoogleCredentialFromCode(string userId, string code)
        {
            var oAuthFlow = new OAuthFlow();
            var tokens = oAuthFlow.Flow.ExchangeCodeForTokenAsync(userId, code,
                Settings.Google.OAuth2.redirectURI, CancellationToken.None).Result;

            //get user account information from Google
            var token = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;
            return GoogleCredential.FromAccessToken(token);
        }
    }

    
}
