namespace Knet.Phone.Client.ArcGIS.Extensions
{
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    public static class IdentityManagerExtensions
    {
        /// <summary>
        /// Generates credentials for the user to the ArcGIS Online and registers ChallengeMethod for the IdentityManager.
        /// </summary>
        /// <param name="identityManager">IdentityManager used.</param>
        /// <param name="username">The Username.</param>
        /// <param name="password">The password.</param>
        /// <param name="tokenValidity">Token validity in minutes.</param>
        /// <returns>Returns <see cref="IdentityManager.Credential"/> as a <see cref="Task"/></returns>
        public static Task<IdentityManager.Credential> LoginAsync(
            this IdentityManager identityManager, string username, string password, int tokenValidity = 5)
        {
            var tcs = new TaskCompletionSource<IdentityManager.Credential>();

            IdentityManager.Current.TokenValidity = tokenValidity;
            IdentityManager.Current.ChallengeMethod +=
                (url, handler, options) =>
                IdentityManager.Current.GenerateCredentialAsync(url, username, password, handler, options);

            IdentityManager.Current.GetCredentialAsync(
                UrlResources.ArcGISPortalGenerateTokenUrl,
                true,
                (credential, exception) =>
                    {
                        if (exception != null)
                        {
                            tcs.SetException(exception);
                            return;
                        }

                        credential.AutoRefresh = true;

                        tcs.SetResult(credential);
                    });

            return tcs.Task;
        }
    }
}