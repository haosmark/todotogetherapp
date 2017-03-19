using System;
using System.Collections.Generic;
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
    public sealed partial class OtherProjectsView : Page
    {
        public OtherProjectsView()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }

        private OtherProjectsViewModel vm = new OtherProjectsViewModel();
  
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await vm.GetProjectsAsync();
        }

        private void ProjectsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var project = e.ClickedItem as Project;
            Frame.Navigate(typeof(ProjectDetailsView), project);
        }
    }
}
