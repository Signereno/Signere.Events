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

        public static EventClient UseTestEnvironment(this EventClient eventClient, bool testEnvironment=true)
        {
            eventClient.TestEnvironment = testEnvironment;
            return eventClient;
        }
        /// <summary>
        /// Do not use only for Signere interla developers
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="testEnvironment"></param>
        /// <returns></returns>
        public static EventClient UseDevEnvironment(this EventClient eventClient, string apiUrl)
        {
            eventClient.APIURL = apiUrl;
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
        /// <summary>
        /// Start the eventlistner, Important to call or else the eventclient will not start listening
        /// </summary>
        /// <param name="eventClient"></param>
        public static void Start(this EventClient eventClient)
        {
            eventClient.Start();
        }
    }
}