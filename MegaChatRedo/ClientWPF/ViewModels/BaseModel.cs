using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClientWPF.Annotations;

namespace ClientWPF.ViewModels
{
    public class BaseModel : INotifyPropertyChanged
    {
        /*public BaseModel()
        {
            if (IsDesignTest) PopulateTestData();
        }
        public virtual void PopulateTestData() { }
        public bool IsDesignTest => DesignerProperties.GetIsInDesignMode(Application.Current.MainWindow);*/

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action method;
        private readonly Func<bool> canExec;
        public RelayCommand(Action method, Func<bool> canExec = null)
        {
            this.method = method;
            this.canExec = canExec ?? (() => true);
        }
        public bool CanExecute(object parameter)
        {
            return canExec();
        }
        public void Execute(object parameter)
        {
            method?.Invoke();
        }
        public event EventHandler CanExecuteChanged;
    }

    public class DIContainer
    {
        private static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();
        public static T GetInstance<T>()
        {
            return (T)instances[typeof(T)];
        }
        public static void AddInstance<T>(T inst)
        {
            instances.Add(typeof(T), inst);
        }
    }

    public class UiInvoker
    {
        public static void Run(Action action)
        {
            //Application.Current.MainWindow.Dispatcher.Invoke(action);
            Application.Current.Dispatcher.Invoke(action);
        }
    }

    public class MessageModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public bool IsInbound { get; set; }
    }
}
