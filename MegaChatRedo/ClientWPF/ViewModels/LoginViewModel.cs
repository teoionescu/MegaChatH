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
        public string Adress { get; set; } = "192.168.0.5";
        public string Port { get; set; } = "8888";
        public ICommand DoLogin { get; set; }
        private string _errMsg;
        public string ErrMsg
        {
            get { return _errMsg; }
            set { _errMsg = value; OnPropertyChanged(); }
        }

        public LoginViewModel()
        {
            DoLogin = new RelayCommand(OnDoLogin);
        }

        public void OnDoLogin()
        {
            var result = DIContainer.GetInstance<IChatClient>().Connect(Adress, int.Parse(Port), Name);
            if (result != null)
            {
                ErrMsg = result;
                return;
            }
            DIContainer.GetInstance<INavigationManager>().ShowDashboard();
        }
    }
}
