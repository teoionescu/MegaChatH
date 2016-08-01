using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientLibrary;
using ClientWPF.Workflow;

namespace ClientWPF.ViewModels
{
    public class LoginViewModel : BaseModel
    {
        public string Name { get; set; }
        public ICommand DoLogin { get; set; }

        public LoginViewModel()
        {
            DoLogin = new RelayCommand(OnDoLogin);
        }

        private void OnDoLogin()
        {
            DIContainer.GetInstance<IChatClient>().Connect("localhost",8888,Name);
            DIContainer.GetInstance<INavigationManager>().ShowDashboard(this);
        }
    }
}
