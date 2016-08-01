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
using CommonLibrary;

namespace ClientWPF.ViewModels
{
    public class DashboardViewModel : BaseModel
    {
        public IChatClient ChatClient;
        public string Name { get; set; }
        public ObservableCollection<ConversationViewModel> Conversations { get; }
        private ConversationViewModel _currentConversation;
        public ConversationViewModel CurrentConversation
        {
            get { return _currentConversation; }
            set { _currentConversation = value; OnPropertyChanged(); }
        }

        public DashboardViewModel()
        {
            Conversations = new ObservableCollection<ConversationViewModel>();
            ChatClient = DIContainer.GetInstance<IChatClient>();
            ChatClient.MessageReceived += OnMessageReceived;

            ChatClient.SendMessage(new ListMessage());
            var rTimer = new Timer(3000);
            rTimer.Elapsed += OnRefreshList;
            rTimer.AutoReset = true;
            rTimer.Enabled = true;
        }

        private void OnRefreshList(object sender, ElapsedEventArgs e)
        {
            ChatClient.SendMessage(new ListMessage());
        }

        private void OnMessageReceived(MessageBase obj)
        {
            if (obj is ChatMessage)
            {
                var msg = (ChatMessage) obj;
                if (Conversations.Any(conv => conv.OtherUser == msg.Source)) return;
                var crt = new ConversationViewModel(ChatClient, msg.Source, Name);
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
                        var crt = new ConversationViewModel(ChatClient, x, Name);
                        UiInvoker.Run(() => { Conversations.Add(crt); });
                    }
                }
                var rem = Conversations.Where(x => msg.Online.All(s => s != x.OtherUser)).ToList();
                var _lock = new object();
                lock (_lock)
                {
                    foreach (var x in rem)
                    {
                        UiInvoker.Run(() => { Conversations.Remove(x); });
                    }
                }
            }
        }
    }
}
