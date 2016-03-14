using System;
using System.Threading.Tasks;
using Rebus.Logging;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Client
{
    public static class EventClientFluent
    {
        public static EventClient SubscribeToDocumentSignedEvent(this EventClient eventClient,
            Func<DocumentSignedEvent, Task> DocumentSignedEventFunc)
        {
            eventClient.SubscribeToDocumentSignedEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        /// <summary>
        /// Use this if you are connected to the Signere.no test environment and not the production environment. If in doubt, contact support at support@signere.no
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="testEnvironment"></param>
        /// <returns></returns>
        public static EventClient UseTestEnvironment(this EventClient eventClient, bool testEnvironment=true)
        {
            eventClient.TestEnvironment = testEnvironment;
            return eventClient;
        }

        /// <summary>
        /// Plugin - a logger which is compatible with Rebus. Read more here: https://github.com/rebus-org/Rebus/wiki/Logging
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static EventClient UseRebusCompatibleLogger(this EventClient eventClient,object loggerFactory)
        {
            if (loggerFactory as IRebusLoggerFactory != null)
                eventClient.RebusLoggerFactory =(IRebusLoggerFactory) loggerFactory;
            if(loggerFactory!=null)
                eventClient.LogToConsole = false;
            return eventClient;
        }

        /// <summary>
        /// Sets up a console logger in Rebus. You can only have one logger, so do not combine this with another logger
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="logToConsole"></param>
        /// <returns></returns>
        public static EventClient LogToConsole(this EventClient eventClient, bool logToConsole=true)
        {
            eventClient.LogToConsole = logToConsole;
            if(logToConsole)
                eventClient.RebusLoggerFactory = null;
            return eventClient;
        }

        /// <summary>
        /// Do not use - only for Signere internal developers
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="testEnvironment"></param>
        /// <returns></returns>
        public static EventClient UseDevEnvironment(this EventClient eventClient, string apiUrl)
        {
            eventClient.APIURL = apiUrl;
            return eventClient;
        }

        public static EventClient SubscribeToDocumentCanceledEvent(this EventClient eventClient,
           Func<DocumentCanceledEvent, Task> DocumentSignedEventFunc)
        {
            eventClient.SubscribeToDocumentCanceledEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static EventClient SubscribeToDocumentPartiallySignedEvent(this EventClient eventClient,
           Func<DocumentPartiallySignedEvent, Task> DocumentSignedEventFunc)
        {
            eventClient.SubscribeToDocumentPartiallySignedEvent(DocumentSignedEventFunc);
            return eventClient;
        }

        public static EventClient SubscribeToDocumentPadesSavedEvent(this EventClient eventClient,
           Func<DocumentPadesSavedEvent,byte[], Task> DocumentPadesSavedEventFunc)
        {
            eventClient.SubscribeToDocumentPadesSavedEvent(DocumentPadesSavedEventFunc);
            return eventClient;
        }
        /// <summary>
        /// Subscribe to the DocumentSigned event. This is fired when all the signers on a document have signed
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="DocumentSignedEventFunc"></param>
        /// <returns></returns>
        public static EventClient SubscribeToDocumentSDOSavedEvent(this EventClient eventClient,
         Func<DocumentSDOSavedEvent, byte[], Task> DocumentSignedEventFunc)
        {
            eventClient.SubscribeToDocumentSDOSavedEvent(DocumentSignedEventFunc);
            return eventClient;
        }
        /// <summary>
        /// Start the event listener. It is important to call this function; or else the EventClient will not start listening
        /// </summary>
        /// <param name="eventClient"></param>
        public static EventClient Start(this EventClient eventClient,Unipluss.Sign.Events.Client.LogLevel logLevel=LogLevel.Debug)
        {
            var internalLogLevel = (Rebus.Logging.LogLevel) Enum.Parse(typeof (Rebus.Logging.LogLevel), logLevel.ToString());
            eventClient.Start(internalLogLevel);
            return eventClient;
        }
    }
}