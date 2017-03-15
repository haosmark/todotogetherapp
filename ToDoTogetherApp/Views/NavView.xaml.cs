using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class NavView : Page
    {
        NavViewModel vm;
        public NavView()
        {
            this.InitializeComponent();
            vm = new NavViewModel();
            hamburgerMenuControl.ItemsSource = vm.MainMenuItems;
            hamburgerMenuControl.OptionsItemsSource = vm.OptionalMenuItems;
            contentFrame.Navigate(typeof(MyProjectsView));
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            contentFrame.Navigate(menuItem.PageType);
        }

        private async void OnOptionalMenuItemClick(object sender, ItemClickEventArgs e)
        {
            await vm.LogoutAsync();
            var menuItem = e.ClickedItem as MenuItem;
            Frame.Navigate(menuItem.PageType);
        }
    }
}
