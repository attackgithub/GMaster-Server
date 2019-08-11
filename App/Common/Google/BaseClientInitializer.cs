using Google.Apis.Services;

namespace GMaster.Common.Google
{
    public class BaseClientInitializer : BaseClientService.Initializer
    {
        public BaseClientInitializer(string credentialUserId)
        {
            ApplicationName = "Gmaster";
            HttpClientInitializer = Credential.GetUserCredential(credentialUserId);
        }

        public BaseClientInitializer(string credentialUserId, string token)
        {
            ApplicationName = "Gmaster";
            HttpClientInitializer = Credential.GetUserCredentialFromToken(credentialUserId, token);
        }
    }
}
