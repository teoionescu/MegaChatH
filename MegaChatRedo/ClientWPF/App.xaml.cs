using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ClientLibrary;
using ClientWPF.ViewModels;
using ClientWPF.Workflow;

namespace ClientWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DIContainerInitializer.Run();
            base.OnStartup(e);
        }
    }

    public class DIContainerInitializer
    {
        public static void Run()
        {
            DIContainer.AddInstance<INavigationManager>(new NavigationManager());
            DIContainer.AddInstance<IChatClient>(new Connection());
        }
    }
}
