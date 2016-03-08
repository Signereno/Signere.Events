using System;
using System.Threading.Tasks;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Client
{
    public static class EventClientFluent
    {
        public static EventClient SubScribeToDocumentSignedEvent(this EventClient eventClient,
            Func<DocumentSignedEvent, Task> DocumentSignedEventFunc)
        {
            eventClient.SubScribeToDocumentSignedEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static EventClient SubScribeToDocumentCancledEvent(this EventClient eventClient,
           Func<DocumentCancledEvent, Task> DocumentSignedEventFunc)
        {
            eventClient.SubScribeToDocumentCancledEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static EventClient SubScribeToDocumentPartialSignedEvent(this EventClient eventClient,
           Func<DocumentPartialSignedEvent, Task> DocumentSignedEventFunc)
        {
            eventClient.SubScribeToDocumentPartialSignedEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static EventClient SubScribeToDocumentPadesSavedEvent(this EventClient eventClient,
           Func<DocumentPartialSignedEvent,byte[], Task> DocumentSignedEventFunc)
        {
            eventClient.SubScribeToDocumentPadesSavedEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static EventClient SubScribeToDocumentSDOSavedEvent(this EventClient eventClient,
         Func<DocumentSDOSavedEvent, byte[], Task> DocumentSignedEventFunc)
        {
            eventClient.SubScribeToDocumentSDOSavedEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static void Start(this EventClient eventClient)
        {
            eventClient.Start();
        }
    }
}