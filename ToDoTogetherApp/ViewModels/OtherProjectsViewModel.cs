using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoTogetherApp.Common;
using ToDoTogetherApp.Models;

namespace ToDoTogetherApp.ViewModels
{
    public class OtherProjectsViewModel
    {
        ClientAPI api = new ClientAPI(App.MobileService.MobileAppUri.ToString());

        public ObservableCollection<Project> Projects { get; set; }
        private User currentUser { get; set; }

        public OtherProjectsViewModel()
        {
            Projects = new ObservableCollection<Project>();
        }

        public async Task GetProjectsAsync()
        {
            if (currentUser == null)
            {
                currentUser = await App.MobileService.InvokeApiAsync<User>("Facebook", HttpMethod.Get, null);
            }
            var myProjects = await api.GetProjectsByContributorAsync(currentUser.Id);

            foreach (var p in myProjects)
            {
                Projects.Add(p);
            }
        }
    }
}
