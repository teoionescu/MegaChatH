using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ServerLibrary
{
    public class Listener : IDisposable
    {
        private readonly Person person;
        private Task internalprocess;
        private bool isactive = true;

        public Listener(Person person)
        {
            this.person = person;
        }

        public void Start()
        {
            internalprocess = Task.Run(ListenLoop);
        }

        private async Task ListenLoop()
        {
            while(isactive)
            {
                try
                {
                    var rd = await person.Reader.ReadLineAsync();
                    var comm = Serializer.Deserialize(rd);
                    ExecuteCommand(comm);
                }
                catch (Exception e)
                {
                    person.Dispose();
                    return;
                }
            }
        }

        private void ExecuteCommand(MessageBase commandData)
        {
            if (commandData is ChatMessage)
            {
                var commandDataAsChatMessage = (ChatMessage) commandData;
                var dest = commandDataAsChatMessage.Destination;
                if (Person.Map.ContainsKey(dest))
                {
                    commandDataAsChatMessage.Source = person.Name;
                    Person.Map[dest].PersonalSender.SendMessage(commandDataAsChatMessage);
                }
            }
            if (commandData is ListMessage)
            {
                var commandDataAsListMessage = (ListMessage)commandData;
                commandDataAsListMessage.Online = Person.Map.Keys.ToArray();
                person.PersonalSender.SendMessage(commandDataAsListMessage);
            }
        }

        public void Dispose()
        {
            isactive = false;
            /*if (!(Task.CurrentId.HasValue && internalprocess.Id == Task.CurrentId.Value)) internalprocess.Wait();*/
        }
    }
}
