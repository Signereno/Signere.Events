using System;
using System.Threading.Tasks;
using Rebus.Logging;
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

        /// <summary>
        /// Use this if you are connected to the Signere.no test environment and not the production environment. If in doubt contact support.
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
        /// Plugin a logger compatible with Rebus read more here: https://github.com/rebus-org/Rebus/wiki/Logging
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
        /// Sets up a console logger in rebus, you can only have one logger, do not combine this with another logger
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="logtoConsole"></param>
        /// <returns></returns>
        public static EventClient LogToConsole(this EventClient eventClient, bool logtoConsole=true)
        {
            eventClient.LogToConsole = logtoConsole;
            if(logtoConsole)
                eventClient.RebusLoggerFactory = null;
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
           Func<DocumentPadesSavedEvent,byte[], Task> DocumentPadesSavedEventFunc)
        {
            eventClient.SubScribeToDocumentPadesSavedEvent(DocumentPadesSavedEventFunc);
            return eventClient;
        }
        /// <summary>
        /// Subscribe to the DocumentSigned event this is fired when all the signers on a document have signed
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="DocumentSignedEventFunc"></param>
        /// <returns></returns>
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
        public static EventClient Start(this EventClient eventClient,Unipluss.Sign.Events.Client.LogLevel logLevel=LogLevel.Debug)
        {
            var internalLogLevel = (Rebus.Logging.LogLevel) Enum.Parse(typeof (Rebus.Logging.LogLevel), logLevel.ToString());
            eventClient.Start(internalLogLevel);
            return eventClient;
        }
    }
}