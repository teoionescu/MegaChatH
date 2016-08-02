using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientLibrary;
using CommonLibrary;

namespace ClientWPF.ViewModels
{
    public class ConversationViewModel : BaseModel
    {
        public ICommand SendMessage { get; set; }
        public string Name { get; set; }
        public string OtherUser { get; set; }
        private bool _online = true;
        public bool Online
        {
            get { return _online; }
            set { _online = value; OnPropertyChanged(); }
        }
        public ObservableCollection<MessageModel> Messages { get; } = new ObservableCollection<MessageModel>();
        private string currentMessageBody;
        

        public string CurrentMessageBody
        {
            get { return currentMessageBody; }
            set { currentMessageBody = value; OnPropertyChanged(); }
        }


        public ConversationViewModel(string otherUser, string name)
        {
            Name = name;
            OtherUser = otherUser;
            DIContainer.GetInstance<IChatClient>().MessageReceived += OnMessageReceived;
            SendMessage = new RelayCommand(OnSendMessage);
        }

        public void OnSendMessage()
        {
            DIContainer.GetInstance<IChatClient>().SendMessage(new ChatMessage
            {
                Source = Name,
                Destination = OtherUser,
                Body = CurrentMessageBody
            });
            Messages.Add(new MessageModel
            {
                From = Name,
                To = OtherUser,
                Body = CurrentMessageBody,
                IsInbound = false
            });
            CurrentMessageBody = null;
        }

        public void OnMessageReceived(MessageBase obj)
        {
            if (obj is ChatMessage)
            {
                var msg = (ChatMessage) obj;
                if (msg.Source != OtherUser) return;
                UiInvoker.Run(() =>
                    Messages.Add(new MessageModel
                    {
                        From = OtherUser,
                        To = Name,
                        Body = msg.Body,
                        IsInbound = true
                    }));
            }
        }
    }
}
