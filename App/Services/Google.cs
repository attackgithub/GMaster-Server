using System;
using Microsoft.AspNetCore.Http;
using Google.Apis.Oauth2.v2;
using GMaster.Common.Google;
using Utility.Serialization;

namespace GMaster.Services
{
    public class Google : Service
    {
        public Google(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string OAuth2(string code)
        {
            //generate new developer key to use within Gmail Chrome Extension
            var developerKey = Utility.Strings.Generate.NewId(10);

            //generate new userId for Google Credentials
            var credentialUserId = Utility.Strings.Generate.NewId(10);
            try
            {
                //authenticate Google OAuth 2.0 via server-side
                var oauthService = new Oauth2Service(new BaseClientInitializer(credentialUserId, code));

                var googleUser = oauthService.Userinfo.Get().Execute();

                //check if user is in the database
                var user = Query.Users.GetByEmail(googleUser.Email);
                var userId = 0;
                if (user != null)
                {
                    //user exists
                    userId = user.userId;
                    if(user.googleUserId == "")
                    {
                        Query.Users.UpdateGoogleUserId(userId, googleUser.Id);
                    }

                    //update Google Credentials userId
                    Query.Users.UpdateCredentialUserId(userId, credentialUserId);
                }
                else
                {
                    //create new user
                    userId = Query.Users.CreateUser(new Query.Models.User()
                    {
                        email = googleUser.Email,
                        name = googleUser.Name != null && googleUser.Name != "" ? googleUser.Name : googleUser.Email.Split('@')[0],
                        gender = googleUser.Gender == "male",
                        locale = googleUser.Locale,
                        credentialUserId = credentialUserId,
                        googleUserId = googleUser.Id
                    });
                }

                //update developer key
                Query.DeveloperKeys.Update(userId, developerKey);

                //log API request
                Common.Log.Api(context, Query.Models.LogApi.Names.GoogleOAuth2, userId);
                
                return Serializer.WriteObjectToString(new { devkey = developerKey, userId });
            }
            catch (Exception)
            {
                //log API call
                Common.Log.Api(context, Query.Models.LogApi.Names.GoogleOAuth2, 0, null, null, false);
                return Error("incorrect Google Sign-In code");
            }
            
        }
    }
}
