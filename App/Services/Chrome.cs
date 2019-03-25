using System.Threading;
using Microsoft.AspNetCore.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using GMaster.Common.Google;

namespace GMaster.Services
{
    public class Chrome : Service
    {
        public Chrome(HttpContext context) : base(context)
        {
        }

        public string OAuth2(string code)
        {
            //authenticate Google OAuth 2.0 via server-side
            var oAuthFlow = new OAuthFlow();
            var tokens = oAuthFlow.Flow.ExchangeCodeForTokenAsync(
                "gmaster", code, 
                Settings.Google.OAuth2.redirectURI, CancellationToken.None).Result;

            //get user account information from Google
            var token = tokens.AccessToken;
            var credential = GoogleCredential.FromAccessToken(token);

            var oauthService = new Oauth2Service(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Gmaster Account Auth"
                }
            );

            var user = oauthService.Userinfo.Get().Execute();

            //check if user is in the database

            //save refresh token in database
            var refreshToken = tokens.RefreshToken;
            return Success();
        }
    }
}
