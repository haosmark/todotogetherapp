using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTogetherApp.Common;
using ToDoTogetherApp.Models;

namespace ToDoTogetherApp.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public ProjectDetailsViewModel()
        {
            SelectedProject = new Project();
        }

        ClientAPI api = new ClientAPI(App.MobileService.MobileAppUri.ToString());
        public Project SelectedProject { get; set; }
        public ObservableCollection<TaskItem> TaskItems { get; set; }
        public ObservableCollection<User> Collaborators { get; set; }

        public async Task AddTaskAsync(string taskName)
        {
            TaskItem t = new TaskItem
            {
                Name = taskName,
                ProjectId = SelectedProject.Id,
                Complete = false
            };
            await api.AddNewTaskItemAsync(t);
            TaskItems.Add(t);
        }

        public async Task AddCollaboratorAsync(string email)
        {
            User user = await api.AddCollaboratorToProjectAsync(SelectedProject.Id, email);
            Collaborators.Add(user);
        }

        public async Task CompleteTask(string id, bool complete)
        {
            await api.CompleteTaskItemByIdAsync(id, complete);
        }

        public async Task DeleteProjectAsync()
        {
            await api.DeleteProjectByIdAsync(SelectedProject.Id);
        }
    }
}
