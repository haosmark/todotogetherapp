using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace ToDoTogetherApp.ViewModels
{
    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }
    }

    class NavViewModel
    {
        public List<MenuItem> MainMenuItems = new List<MenuItem>();
        public List<MenuItem> OptionalMenuItems = new List<MenuItem>();

        public NavViewModel()
        {
            if (MainMenuItems.Count == 0 && OptionalMenuItems.Count == 0)
            {
                MainMenuItems.Add(new MenuItem()
                {
                    Icon = Symbol.Home,
                    Name = "My Projects",
                    PageType = typeof(Views.MyProjectsView)
                });
                MainMenuItems.Add(new MenuItem()
                {
                    Icon = Symbol.ViewAll,
                    Name = "Other Projects",
                    PageType = typeof(Views.OtherProjectsView)
                });

                OptionalMenuItems.Add(new MenuItem()
                {
                    Icon = Symbol.Back,
                    Name = "Logout",
                    PageType = typeof(Views.LoginView)
                });
            }
        }

        private PasswordVault vault = new PasswordVault();
        private PasswordCredential credential;
        public async Task LogoutAsync()
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
                vault.Remove(credential);
                await App.MobileService.LogoutAsync();
            }
        }
    }
}
