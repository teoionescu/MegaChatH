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
        void ShowDashboard();
        void ReturnToLogin();
    }

    class NavigationManager : INavigationManager
    {
        private MainWindow window;
        private LoginView loginview;
        private LoginViewModel loginviewmodel;
        private DashboardView dashboardview;
        private DashboardViewModel dashboardviewmodel;

        public void Start()
        {
            window = (MainWindow)Application.Current.MainWindow;
            loginview = new LoginView();
            loginviewmodel = new LoginViewModel();
            loginview.DataContext = loginviewmodel;
            window.ContentControl.Content = loginview;
        }

        public void ShowDashboard()
        {
            if (dashboardview == null)
            {
                dashboardview = new DashboardView();
                dashboardviewmodel = new DashboardViewModel {Name = loginviewmodel.Name};
                dashboardview.DataContext = dashboardviewmodel;
            }
            else
            {
                dashboardviewmodel.Name = loginviewmodel.Name;
                dashboardviewmodel.OnNavigation();
            }
            window.ContentControl.Content = dashboardview;
        }

        public void ReturnToLogin()
        {
            window.ContentControl.Content = loginview;
        }
    }
}
