using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToDoTogetherApp.Models;
using ToDoTogetherApp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoTogetherApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectDetailsView : Page
    {
        public ProjectDetailsViewModel vm;
        public ProjectDetailsView()
        {
            this.InitializeComponent();
            vm = new ProjectDetailsViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var project = e.Parameter as Project;
            vm.SelectedProject = project;
            vm.TaskItems = new ObservableCollection<TaskItem>(project.Tasks);
            vm.Collaborators = new ObservableCollection<User>(project.Collaborators);
        }

        private async void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            await vm.DeleteProjectAsync();
            Frame.Navigate(typeof(MyProjectsView));
        }

        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            await vm.AddTaskAsync(TaskNameBox.Text);
            TaskNameBox.Text = "";
        }

        private async void AddCollaboratorButton_Click(object sender, RoutedEventArgs e)
        {
            await vm.AddCollaboratorAsync(CollaboratorsEmailBox.Text);
            CollaboratorsEmailBox.Text = "";
        }
    }
}
