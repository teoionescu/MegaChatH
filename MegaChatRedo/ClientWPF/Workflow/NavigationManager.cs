using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClientLibrary;
using ClientWPF.ViewModels;
using ClientWPF.Views;

namespace ClientWPF.Workflow
{
    public interface INavigationManager
    {
        void Start();
        void ShowDashboard(LoginViewModel lvm);
    }

    class NavigationManager : INavigationManager
    {
        private MainWindow window;

        public void Start()
        {
            window = (MainWindow)Application.Current.MainWindow;
            var view = new LoginView();
            view.DataContext = new LoginViewModel();
            window.ContentControl.Content = view;
        }

        public void ShowDashboard(LoginViewModel lvm)
        {
            var view = new DashboardView();
            var vm = new DashboardViewModel {Name = lvm.Name};
            view.DataContext = vm;
            window.ContentControl.Content = view;
        }
    }
}
