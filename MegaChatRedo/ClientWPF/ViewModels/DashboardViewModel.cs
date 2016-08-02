using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ClientLibrary;
using ClientWPF.Workflow;
using CommonLibrary;

namespace ClientWPF.ViewModels
{
    public class DashboardViewModel : BaseModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ConversationViewModel> Conversations { get; }
        private ConversationViewModel currentConversation;
        public ConversationViewModel CurrentConversation
        {
            get { return currentConversation; }
            set { currentConversation = value; OnPropertyChanged(); }
        }
        public ICommand DoLogout { get; set; }
        private Timer rTimer;

        public DashboardViewModel()
        {
            Conversations = new ObservableCollection<ConversationViewModel>();
            DIContainer.GetInstance<IChatClient>().MessageReceived += OnMessageReceived;
            DoLogout = new RelayCommand(OnDoLogout);
            OnNavigation();
        }

        public void OnRefreshList(object sender, ElapsedEventArgs e)
        {
            DIContainer.GetInstance<IChatClient>().SendMessage(new ListMessage());
        }

        private void OnDoLogout()
        {
            rTimer.Dispose();
            rTimer = null;
            DIContainer.GetInstance<IChatClient>().Disconnect();
            DIContainer.GetInstance<INavigationManager>().ReturnToLogin();
        }

        public void OnMessageReceived(MessageBase obj)
        {
            if (obj is ChatMessage)
            {
                var msg = (ChatMessage) obj;
                if (Conversations.Any(conv => conv.OtherUser == msg.Source)) return;
                var crt = new ConversationViewModel(msg.Source, null);
                UiInvoker.Run(() => { Conversations.Add(crt); });
                CurrentConversation = crt;
                CurrentConversation.OnMessageReceived(obj);
            }
            if (obj is ListMessage)
            {
                var msg = (ListMessage)obj;
                foreach (var x in msg.Online)
                {
                    if (Conversations.All(conv => conv.OtherUser != x))
                    {
                        var crt = new ConversationViewModel(x, null);
                        UiInvoker.Run(() => { Conversations.Add(crt); });
                    }
                }
                var rem = Conversations.Where(x => msg.Online.All(s => s != x.OtherUser)).ToList();
                var _lock = new object();
                lock (_lock)
                {
                    foreach (var x in Conversations)
                    {
                        //UiInvoker.Run(() => { Conversations.Remove(x); });
                        if (rem.Contains(x)) x.Online = false;
                        else x.Online = true;
                    }
                }
            }
        }

        public void OnNavigation()
        {
            DIContainer.GetInstance<IChatClient>().SendMessage(new ListMessage());
            rTimer = new Timer(3000);
            rTimer.Elapsed += OnRefreshList;
            rTimer.AutoReset = true;
            rTimer.Enabled = true;
        }
    }
}
