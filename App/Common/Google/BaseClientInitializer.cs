using Google.Apis.Services;

namespace GMaster.Common.Google
{
    public class BaseClientInitializer : BaseClientService.Initializer
    {
        public BaseClientInitializer(Datasilk.User User)
        {
            ApplicationName = "Gmaster";
            HttpClientInitializer = Credential.GetGoogleCredential(User.credentialUserId);
        }

        public BaseClientInitializer(string userId, string code)
        {
            ApplicationName = "Gmaster";
            HttpClientInitializer = Credential.GetGoogleCredentialFromCode(userId, code);
        }
    }
}
