using System;
using System.Threading.Tasks;
using Unipluss.Sign.Events.Client;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Test
{
    public class EventClientTest
    {
        public EventClientTest()
        {
            EventClient
                .SetupWithPrimaryApiKey("",Guid.NewGuid(),"")
                .SubScribeToDocumentSignedEvent(DocumentSignedEvent)
                .SubScribeToDocumentCancledEvent(DocumentCancledEvent)
                .SubScribeToDocumentPartialSignedEvent(DocumentPartialSignedEvent)
                .SubScribeToDocumentPadesSavedEvent(DocumentPadesSavedEvent)
                .Start();

        }

        private async Task DocumentPadesSavedEvent(DocumentPartialSignedEvent arg1, byte[] arg2)
        {
            
        }

        private async Task DocumentPartialSignedEvent(DocumentPartialSignedEvent arg)
        {
        }

        private async  Task DocumentCancledEvent(DocumentCancledEvent arg)
        {
        }

        private  Task DocumentSignedEvent(DocumentSignedEvent arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(true);
        }
    }
}
