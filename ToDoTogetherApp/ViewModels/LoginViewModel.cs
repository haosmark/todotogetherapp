using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace ToDoTogetherApp.ViewModels
{
    public class LoginViewModel
    {
        private MobileServiceUser user;
        private PasswordVault vault = new PasswordVault();
        private PasswordCredential credential;

        public bool IsAuthenticated()
        {
            var provider = MobileServiceAuthenticationProvider.Facebook;
            try
            {
                credential = vault.FindAllByResource(provider.ToString()).FirstOrDefault();
            }
            catch (Exception e)
            {

            }

            if (credential != null)
            {
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                App.MobileService.CurrentUser = user;
                return true;
            }

            return false;
        }

        public async Task<bool> AuthenticateAsync()
        {
            var provider = MobileServiceAuthenticationProvider.Facebook;

            try
            {
                user = await App.MobileService.LoginAsync(provider);
                credential = new PasswordCredential(provider.ToString(), user.UserId, user.MobileServiceAuthenticationToken);
                vault.Add(credential);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
