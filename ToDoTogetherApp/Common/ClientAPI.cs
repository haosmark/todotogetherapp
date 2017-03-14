using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using ToDoTogetherApp.Models;

namespace ToDoTogetherApp.Common
{
    public class ClientAPI : IClientAPI
    {
        public IMobileServiceTable<Assignment> AssignmentsTable { get { return assignmentsTable; } }
        public IMobileServiceTable<User> UsersTable { get { return usersTable; } }
        public IMobileServiceTable<Project> ProjectsTable { get { return projectsTable; } }
        public IMobileServiceTable<TaskItem> TaskItemsTable { get { return taskItemsTable; } }

        private IMobileServiceTable<Assignment> assignmentsTable;
        private IMobileServiceTable<User> usersTable;
        private IMobileServiceTable<Project> projectsTable;
        private IMobileServiceTable<TaskItem> taskItemsTable;

        private MobileServiceClient client;

        public ClientAPI(string url)
        {
            client = new MobileServiceClient(url);
            assignmentsTable = client.GetTable<Assignment>();
            projectsTable = client.GetTable<Project>();
            taskItemsTable = client.GetTable<TaskItem>();
            usersTable = client.GetTable<User>();
        }


        public async Task<User> AddCollaboratorToProjectAsync(string projectId, string userEmail)
        {
            User user;
            var users = await usersTable.Where(u => u.Email == userEmail).ToCollectionAsync();
            if (users.Count == 0)
            {
                user = new User() { Email = userEmail };
                await usersTable.InsertAsync(user);
            }
            else
            {
                user = users.FirstOrDefault();
            }

            await assignmentsTable.InsertAsync(new Assignment { ProjectId = projectId, UserId = user.Id });
            return user;
        }

        public async Task AddNewProjectAsync(Project p)
        {
            // Because of relational database, we can only store the individual values in the database
            // All other properties must be null for table insert
            p.OwnerId = p.Owner.Id;
            p.Owner = null;
            p.Tasks = null;
            p.Collaborators = null;
            await projectsTable.InsertAsync(p);
            // New project starts with 0 tasks and 0 users
            p.Tasks = new List<TaskItem>();
            p.Collaborators = new List<User>();
        }

        public async Task AddNewTaskItemAsync(TaskItem t)
        {
            await taskItemsTable.InsertAsync(t);
        }

        public async Task CompleteTaskItemByIdAsync(string taskId, bool complete)
        {
            TaskItem t = await taskItemsTable.LookupAsync(taskId);

            if (t == null)
            {
                throw new KeyNotFoundException($"TaskID {taskId} not found.");
            }

            t.Complete = complete;
        }

        public async Task DeleteProjectByIdAsync(string projectId)
        {
            var p = await projectsTable.LookupAsync(projectId);

            if (p == null)
            {
                throw new KeyNotFoundException($"ProjectID {projectId} not found");
            }

            await projectsTable.DeleteAsync(p);
        }

        public async Task<ICollection<Project>> GetProjectsByContributorAsync(string contributorId)
        {
            var assignments = await assignmentsTable.Where(x => x.UserId == contributorId).ToCollectionAsync();
            ICollection<Project> projects = new List<Project>();
            foreach (var a in assignments)
            {
                var p = await projectsTable.LookupAsync(a.ProjectId);
                projects.Add(p);
            }
            foreach (var p in projects)
            {
                await populateProjectsAsync(p);
            }

            return projects;
        }

        public async Task<ICollection<Project>> GetProjectsByOwnerAsync(string ownerId)
        {
            var projects = await projectsTable.Where(x => x.OwnerId == ownerId).OrderBy(x => x.Name).ToCollectionAsync();
            foreach (var p in projects)
            {
                await populateProjectsAsync(p);
            }

            return projects;
        }

        // helper method
        private async Task populateProjectsAsync(Project p)
        {
            p.Owner = await usersTable.LookupAsync(p.OwnerId);

            foreach (var a in await assignmentsTable.Where(x => x.ProjectId == p.Id).ToCollectionAsync())
            {
                p.Collaborators.Add(await usersTable.LookupAsync(a.UserId));
            }

            foreach (var t in await taskItemsTable.Where(x => x.ProjectId == p.Id).OrderBy(x => x.Name).ToCollectionAsync())
            {
                p.Tasks.Add(t);
            }
        }
    }
}
