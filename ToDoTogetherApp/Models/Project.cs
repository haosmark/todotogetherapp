using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTogetherApp.Models
{
    public class Project
    {
        public Project()
        {
            Tasks = new List<TaskItem>();
            Collaborators = new List<User>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<TaskItem> Tasks { get; set; }
        public ICollection<User> Collaborators { get; set; }
    }
}
