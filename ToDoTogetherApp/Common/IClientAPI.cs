using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTogetherApp.Models;

namespace ToDoTogetherApp.Common
{
    public interface IClientAPI
    {
        IMobileServiceTable<Assignment> AssignmentsTable { get; }
        IMobileServiceTable<User> UsersTable { get; }
        IMobileServiceTable<Project> ProjectsTable { get; }
        IMobileServiceTable<TaskItem> TaskItemsTable { get; }

        // returns all projects that the user owns
        Task<ICollection<Project>> GetProjectsByOwnerAsync(string ownerId);

        // returns all projects that the user collaborates in
        Task<ICollection<Project>> GetProjectsByContributorAsync(string contributorId);

        Task AddNewProjectAsync(Project p);
        Task AddNewTaskItemAsync(TaskItem t);
        Task CompleteTaskItemByIdAsync(string taskId, bool complete);
        Task DeleteProjectByIdAsync(string projectId);
        Task<User> AddCollaboratorToProjectAsync(string projectId, string userEmail);
    }
}
