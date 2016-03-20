using System;
using System.Threading.Tasks;
using Rebus.Logging;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Client
{
    public static class EventClientFluent
    {
        /// <summary>
        /// Subscribe to the Document Signed saved event. This is fired when all the signers have signed the document.
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="@event"></param>
        /// <returns></returns>
        public static EventClient SubscribeToDocumentSignedEvent(this EventClient eventClient,
            Func<DocumentSignedEvent, Task> @event)
        {
            if(@event!=null)
                eventClient.SubscribeToDocumentSignedEvent(@event);
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

        /// <summary>
        /// Subscribe to the SubscribeToDocumentCanceled event. This is fired when the document is cancled either by the sender or the receiver
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="@event"></param>
        /// <returns></returns>
        public static EventClient SubscribeToDocumentCanceledEvent(this EventClient eventClient,
           Func<DocumentCanceledEvent, Task> @event)
        {
            if(@event!=null)
                eventClient.SubscribeToDocumentCanceledEvent(@event);
            return eventClient;
        }

        /// <summary>
        /// Subscribe to the DocumentSignedPartiallySigned event. This is fired when the document is signed, but when it's not the last signer.
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="@event"></param>
        /// <returns></returns>
        public static EventClient SubscribeToDocumentPartiallySignedEvent(this EventClient eventClient,
           Func<DocumentPartiallySignedEvent, Task> @event)
        {
            if(@event!=null)
            eventClient.SubscribeToDocumentPartiallySignedEvent(@event);
            return eventClient;
        }

        /// <summary>
        /// Subscribe to the PADES saved event. This is fired when the PADES file (PDF document signing) is created and saved to storage
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="@event"></param>
        /// <returns></returns>
        public static EventClient SubscribeToDocumentPadesSavedEvent(this EventClient eventClient,
           Func<DocumentPadesSavedEvent,byte[], Task> @event)
        {
            if (@event != null)
                eventClient.SubscribeToDocumentPadesSavedEvent(@event);
            return eventClient;
        }
        /// <summary>
        /// Subscribe to the SDO saved event. This is fired when all the signers on a document have signed and the SDO file have been saved to storage
        /// </summary>
        /// <param name="eventClient"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        public static EventClient SubscribeToDocumentSDOSavedEvent(this EventClient eventClient,
         Func<DocumentSDOSavedEvent, byte[], Task> @event)
        {
            if (@event != null)
                eventClient.SubscribeToDocumentSDOSavedEvent(@event);
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