using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Client
{
    public class EventClient
    {
        public IBus Bus;
       
        private  Func<DocumentSignedEvent,Task> DocumentSignedEventFunc;
        private Func<DocumentCancledEvent, Task> DocumentCancledEventFunc;
        private Func<DocumentPartialSignedEvent, Task> DocumentPartialSignedEventFunc;
        private BuiltinHandlerActivator adapter;
        private readonly string _connectionstring;
        private readonly string _queuename;
        private readonly string _apikey;
        private readonly bool _secondaryKey;

        internal EventClient( BuiltinHandlerActivator adapter,string connectionstring,string queuename,string apikey,bool secondaryKey)
        {
            this.adapter = adapter;
            _connectionstring = connectionstring;
            _queuename = queuename;
            _apikey = apikey;
            _secondaryKey = secondaryKey;
        }

        internal void SubScribeToDocumentPadesSavedEvent(Func<DocumentPadesSavedEvent,byte[], Task> func)
        {
            adapter.Handle<DocumentPadesSavedEvent>(async (bus, context, msg) => {
               byte[] data=await DownloadPades(msg.DocumentId);
               await func(msg, data);
            });
        }

        internal void SubScribeToDocumentSDOSavedEvent(Func<DocumentSDOSavedEvent, byte[], Task> func)
        {
            adapter.Handle<DocumentSDOSavedEvent>(async (bus, context, msg) => {
                byte[] data = await DownloadSDO(msg.DocumentId);
                await func(msg, data);
            });
        }

        private Task<byte[]> DownloadSDO(Guid documentId)
        {
            throw new NotImplementedException();
        }

        private Task<byte[]> DownloadPades(Guid documentId)
        {
            throw new NotImplementedException();
        }

        internal void SubScribeToDocumentSignedEvent(Func<DocumentSignedEvent, Task> func)
        {
            adapter.Handle(func);
        }

        internal void SubScribeToDocumentCancledEvent(Func<DocumentCancledEvent, Task> func)
        {
            adapter.Handle(func);
        }

        internal void SubScribeToDocumentPartialSignedEvent(Func<DocumentPartialSignedEvent, Task> func)
        {
            adapter.Handle(func);
        }

        public static EventClient SetupWithPrimaryApiKey(string azureServiceBusConnectionString, Guid DocumentProvider,string ApiKey)
        {
            BuiltinHandlerActivator adapter = new BuiltinHandlerActivator();
           
           
            return new EventClient(adapter,azureServiceBusConnectionString,DocumentProvider.ToString("n"),ApiKey,false);
        }

        public static EventClient SetupWithSecondaryApiKey(string azureServiceBusConnectionString, Guid DocumentProvider, string ApiKey)
        {
            BuiltinHandlerActivator adapter = new BuiltinHandlerActivator();


            return new EventClient(adapter, azureServiceBusConnectionString, DocumentProvider.ToString("n"), ApiKey, true);
        }

        internal void Start()
        {
            this.Bus = Configure.With(adapter)
                .Transport(x => x.UseAzureServiceBus(_connectionstring, _queuename))
                //.Logging(x=>x.)
                .Start();
        }
    }
}