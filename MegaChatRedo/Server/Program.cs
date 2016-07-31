using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Properties;
using ServerLibrary;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var srv=new ChatServer(8888);
            Task.Run(() => srv.Run())
                .ContinueWith(t => Console.WriteLine("Error " + t.Exception), TaskContinuationOptions.OnlyOnFaulted);*/

            Type s = typeof(ChatServer);
            Task.Run(() => (ChatServer)Activator.CreateInstance(s, args: Settings.Default.ListenPort))
            .ContinueWith(t => Console.WriteLine("Error " + t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("Press any key to stop server...");
            Console.ReadKey();
        }
    }
}
