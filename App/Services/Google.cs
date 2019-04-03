using System.Threading;
using Microsoft.AspNetCore.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using GMaster.Common.Google;

namespace GMaster.Services
{
    public class Google : Service
    {
        public Google(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string OAuth2(string code)
        {
            //authenticate Google OAuth 2.0 via server-side
            var oAuthFlow = new OAuthFlow();
            var tokens = oAuthFlow.Flow.ExchangeCodeForTokenAsync(
                "gmaster", code, 
                Settings.Google.OAuth2.redirectURI, CancellationToken.None).Result;

            //get user account information from Google
            var token = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;
            var credential = GoogleCredential.FromAccessToken(token);

            var oauthService = new Oauth2Service(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Gmaster Account Auth"
                }
            );

            var googleUser = oauthService.Userinfo.Get().Execute();

            //check if user is in the database
            var user = Query.Users.GetByEmail(googleUser.Email);
            var userId = 0;
            var developerKey = "";
            if(user != null)
            {
                //user exists
                userId = user.userId;
                Query.Users.UpdateRefreshToken(userId, refreshToken);

                //get developer key
                var devkeyInfo = Query.DeveloperKeys.ForUser(userId);
                if(devkeyInfo == null)
                {
                    //developer key didn't exist
                    developerKey = Query.DeveloperKeys.Create(userId);
                }
                else
                {
                    //developer key exists
                    developerKey = devkeyInfo.devkey;
                }
            }
            else
            {
                //create new user
                userId = Query.Users.CreateUser(new Query.Models.User()
                {
                    email = googleUser.Email,
                    name = googleUser.Name,
                    gender = googleUser.Gender == "male",
                    locale = googleUser.Locale,
                    refreshToken = refreshToken
                });

                //create a developer key for new user
                developerKey = Query.DeveloperKeys.Create(userId);
            }

            //save refresh token in database
            return developerKey;
        }
    }
}
