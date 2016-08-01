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
        public IChatClient ChatClient;
        public ICommand SendMessage { get; set; }
        public string Name { get; set; }
        public string OtherUser { get; set; }
        public ObservableCollection<MessageModel> Messages { get; } = new ObservableCollection<MessageModel>();
        private string _currentMessageBody;
        public string CurrentMessageBody
        {
            get { return _currentMessageBody; }
            set { _currentMessageBody = value; OnPropertyChanged(); }
        }


        public ConversationViewModel(IChatClient chatClient, string otherUser, string name)
        {
            ChatClient = chatClient;
            Name = name;
            OtherUser = otherUser;
            ChatClient.MessageReceived += OnMessageReceived;
            SendMessage = new RelayCommand(OnSendMessage);
        }

        public void OnSendMessage()
        {
            ChatClient.SendMessage(new ChatMessage
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
