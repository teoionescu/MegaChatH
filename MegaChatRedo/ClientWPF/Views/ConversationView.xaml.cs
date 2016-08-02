using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientLibrary;
using ClientWPF.ViewModels;
using CommonLibrary;

namespace ClientWPF.Views
{
    /// <summary>
    /// Interaction logic for ConversationView.xaml
    /// </summary>
    public partial class ConversationView : UserControl
    {
        public ConversationView()
        {
            InitializeComponent();
            DIContainer.GetInstance<IChatClient>().MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(MessageBase obj)
        {
            if (obj is ChatMessage)
            {
                UiInvoker.Run(() => _scrollViewer.ScrollToEnd());
            }
        }
    }
}
