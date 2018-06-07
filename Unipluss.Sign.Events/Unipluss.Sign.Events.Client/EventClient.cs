using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.AzureServiceBus.Config;
using Rebus.Bus;
using Rebus.Compression;
using Rebus.Config;
using Rebus.Encryption;
using Rebus.Logging;
using Rebus.Retry.Simple;
using Unipluss.Sign.Events.Client.Infrastructur;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Client
{
    /// <summary>
    /// Event client for Signere.no. Remember to dispose of the client when done by calling the Dispose() method.
    /// </summary>
    public class EventClient:IDisposable
    {
        private const string testApiUrl = "https://testapi.signere.no";
        private const string productionApiUrl= "https://api.signere.no";


        private readonly string _apikey;
        private readonly string _connectionstring;
        private readonly Guid _documentProviderId;
        private readonly string _queuename;
        private readonly bool _secondaryKey;
        private readonly BuiltinHandlerActivator adapter;
        public IBus Bus;
        private Func<DocumentCanceledEvent, Task> DocumentCanceledEventFunc;
        private Func<DocumentPartiallySignedEvent, Task> DocumentPartialSignedEventFunc;
        private Func<DocumentSignedEvent, Task> DocumentSignedEventFunc;
        private Action<RebusLoggingConfigurer> rebusLoggingConfigurer;

        internal EventClient(BuiltinHandlerActivator adapter, string connectionstring, Guid documentProviderId,
            string apikey, bool secondaryKey)
        {
            this.adapter = adapter;
            _connectionstring = connectionstring;
            _documentProviderId = documentProviderId;
            _queuename = _documentProviderId.ToString("n");
            _apikey = apikey;
            _secondaryKey = secondaryKey;
           NoRebusLogger = true;
        }

        internal bool TestEnvironment { get; set; }
        internal string APIURL { get; set; }

        internal bool LogToConsole { get; set; }
        internal Rebus.Logging.LogLevel? logLevel { get; set; }

        internal void SubscribeToDocumentPadesSavedEvent(Func<DocumentPadesSavedEvent, byte[], Task> func)
        {
            adapter.Handle<DocumentPadesSavedEvent>(async (bus, context, msg) =>
            {
                var data = await DownloadPades(msg.DocumentId);
                await func(msg, data);
            });
        }

        internal void SubscribeToDocumentSDOSavedEvent(Func<DocumentSDOSavedEvent, byte[], Task> func)
        {
            adapter.Handle<DocumentSDOSavedEvent>(async (bus, context, msg) =>
            {
                var data = await DownloadSDO(msg.DocumentId);
                await func(msg, data);
            });
        }
        internal void SubscribeToDocumentSignedEvent(Func<DocumentSignedEvent, Task> func)
        {
            adapter.Handle(func);
        }

        internal void SubscribeToDocumentCanceledEvent(Func<DocumentCanceledEvent, Task> func)
        {
            adapter.Handle(func);
        }

        internal void SubscribeToDocumentPartiallySignedEvent(Func<DocumentPartiallySignedEvent, Task> func)
        {
            adapter.Handle(func);
        }

        internal void SubscribeToDocumentFormSignedEvent(Func<DocumentFormSignedEvent, Task> func)
        {
            adapter.Handle(func);
        }

        internal void SubscribeToDocumentFormPartiallySignedEvent(Func<DocumentFormPartiallySignedEvent, Task> func)
        {
            adapter.Handle(func);
        }

        /// <summary>
        ///     Sets up the EventClient to download events from the ServiceBus and files from the Signere API. Note that the primary key gives elevated privileges and is not necessary for subscribing to events - using secondary key is normally sufficient (see SetupWithSecondaryKey).
        /// </summary>
        /// <param name="azureServiceBusConnectionString">Your ServiceBus connection string. Contact support@signere.no to get this</param>
        /// <param name="DocumentProvider">Your account ID</param>
        /// <param name="ApiKey">Your primary API key</param>
        /// <returns>EventClient set up using the primary API key</returns>
        public static EventClient SetupWithPrimaryApiKey(string azureServiceBusConnectionString, Guid DocumentProvider,string ApiKey)
        {
            var adapter = new BuiltinHandlerActivator();

            return new EventClient(adapter, azureServiceBusConnectionString, DocumentProvider, ApiKey, false);
        }

        /// <summary>
        ///     Sets up the EventClient to download events from the ServiceBus and files from the Signere API.
        /// </summary>
        /// <param name="azureServiceBusConnectionString">Your ServiceBus connection string. Contact support@signere.no to get this</param>
        /// <param name="DocumentProvider">Your account ID</param>
        /// <param name="ApiKey">Your secondary API key</param>
        /// <returns>EventClient set up using the secondary API key</returns>
        public static EventClient SetupWithSecondaryApiKey(string azureServiceBusConnectionString, Guid DocumentProvider,
            string ApiKey)
        {
            var adapter = new BuiltinHandlerActivator();
            
            return new EventClient(adapter, azureServiceBusConnectionString, DocumentProvider, ApiKey, true);
        }

        

        internal void Start()
        {
            Bus = ConfigureRebus().Start();
        }

        internal void AddRebusCompatibeLogger(Action<RebusLoggingConfigurer> config)
        {
            rebusLoggingConfigurer = config;
            if (config != null)
            {
                this.NoRebusLogger = false;
            }
            else
            {
                this.NoRebusLogger = true;
            }
        }

        public bool NoRebusLogger { get; set; }

        private RebusConfigurer ConfigureRebus()
        {
            string encryptionKey = DownloadEncryptionKey();
            return Configure.With(adapter)
                .Transport(x => x.UseAzureServiceBus(_connectionstring, _queuename, AzureServiceBusMode.Basic)
                .DoNotCreateQueues())           
                .Options(c =>
                {
                    //c.SimpleRetryStrategy(_queuename + "_error", 5, true);
                    c.AddNamespaceFilter();
                    c.EnableCompression();
                    c.EnableEncryption(encryptionKey);
                })
                .Logging(x =>
                {
                    if(LogToConsole)
                        x.ColoredConsole(this.logLevel==null ? Rebus.Logging.LogLevel.Error : this.logLevel.Value);
                    else if (RebusLoggerFactory!=null)
                    {
                        x.Use(RebusLoggerFactory);
                    }else if (rebusLoggingConfigurer != null)
                    {
                        rebusLoggingConfigurer(x);
                    }
                    else if(NoRebusLogger)
                    {
                        x.None();
                    }
                });

            
        }

        internal IRebusLoggerFactory RebusLoggerFactory { get; set; }

        internal RebusLoggingConfigurer Configurer { get; set; }

        #region Download files
        private async Task<byte[]> DownloadSDO(Guid documentId)
        {
            var url = CreateUrl("api/DocumentFile/Signed/{0}", documentId, TestEnvironment);
            
            return await DownloadFile(url);
        }

        private async Task<byte[]> DownloadPades(Guid documentId)
        {
            var url = CreateUrl("api/DocumentFile/SignedPDF/{0}", documentId, TestEnvironment);
            return await DownloadFile(url);
        }


        private string DownloadEncryptionKey()
        {
            var apiUrl = ApiUrl(TestEnvironment);
            
            var url =string.Format("{0}/{1}",apiUrl, "api/events/encryptionkey");

            var resultBytes=Task.Run(async()=>await  DownloadFile(url)).Result;
            var result = Encoding.UTF8.GetString(resultBytes);
            
            //Hack to unescape string
            result= result.Substring(1,result.Length-2);
            return result;
        }

        private async Task<byte[]> DownloadFile(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var timestamp = DateTime.UtcNow.ToString("s");
                    client.DefaultRequestHeaders.Add("API-ID", _documentProviderId.ToString());
                    client.DefaultRequestHeaders.Add("API-TIMESTAMP", timestamp);
                    client.DefaultRequestHeaders.Add("API-USINGSECONDARYTOKEN", _secondaryKey.ToString());
                    client.DefaultRequestHeaders.Add("API-TOKEN", GenerateTokenForUrl(url, "GET", _apikey, timestamp));
                    client.DefaultRequestHeaders.Add("API-ALGORITHM", "SHA512");
                    client.DefaultRequestHeaders.Add("API-RETURNERRORHEADER", "true");
                    return await client.GetByteArrayAsync(url);
                }
                catch (Exception e)
                {
                    if(LogToConsole)
                        Console.WriteLine(e);

                    throw e;
                }
            }
        }

        private static string GenerateTokenForUrl(string url, string httpverb, string secretKey, string timestamp)
        {
            var urlWithTimeStamp = string.Format("{0}&Timestamp={1}&Httpverb={2}", url, timestamp, httpverb);
            return GetSHA512(urlWithTimeStamp, secretKey);
        }

        private static string GetSHA512(string text, string key)
        {
            Encoding encoding = new UTF8Encoding();

            var keyByte = encoding.GetBytes(key);
            var hmacsha512 = new HMACSHA512(keyByte);

            var messageBytes = encoding.GetBytes(text);
            var hashmessage = hmacsha512.ComputeHash(messageBytes);
            return ByteToString(hashmessage);
        }

        private static string ByteToString(byte[] buff)
        {
            var sbinary = "";

            for (var i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

       
        private string CreateUrl(string path, Guid documentId, bool testEnvironment)
        {
            var apiUrl = ApiUrl(testEnvironment);
            return string.Format("{0}/{1}", apiUrl, string.Format(path, documentId));
        }

        private string ApiUrl(bool testEnvironment)
        {
            var apiUrl = testEnvironment ? testApiUrl : productionApiUrl;
            if (!string.IsNullOrWhiteSpace(APIURL))
                apiUrl = APIURL;
            return apiUrl;
        }

        #endregion

        /// <summary>
        /// IMPORTANT! Disposes the EventClient, which also disposes the bus. You should always call this method when your program has completed.
        /// </summary>
        public void Dispose()
        {
            
            if(Bus!=null)
                Bus.Dispose();
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
    }
}