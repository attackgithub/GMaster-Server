using System.Threading;
using Google.Apis.Auth.OAuth2;

namespace GMaster.Common.Google
{
    public static class Credential
    {
        public static UserCredential GetUserCredential(string credentialUserId)
        {
            var oAuthFlow = new OAuthFlow();
            var tokens = oAuthFlow.Flow.LoadTokenAsync(credentialUserId, CancellationToken.None).Result;
            return new UserCredential(oAuthFlow.Flow, credentialUserId, tokens);
        }

        public static UserCredential GetUserCredentialFromToken(string credentialUserId, string token)
        {
            var oAuthFlow = new OAuthFlow();
            var tokens = oAuthFlow.Flow.ExchangeCodeForTokenAsync(credentialUserId, token,
                Settings.Google.OAuth2.redirectURI, CancellationToken.None).Result;
            return new UserCredential(oAuthFlow.Flow, credentialUserId, tokens);
        }
    }

    
}
