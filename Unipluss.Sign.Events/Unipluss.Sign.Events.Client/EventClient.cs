using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.AzureServiceBus.Config;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Retry.Simple;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Client
{
    public class EventClient
    {
        private readonly string _apikey;
        private readonly string _connectionstring;
        private readonly Guid _documentProviderId;
        private readonly string _queuename;
        private readonly bool _secondaryKey;
        private readonly BuiltinHandlerActivator adapter;
        public IBus Bus;
        private Func<DocumentCancledEvent, Task> DocumentCancledEventFunc;
        private Func<DocumentPartialSignedEvent, Task> DocumentPartialSignedEventFunc;
        private Func<DocumentSignedEvent, Task> DocumentSignedEventFunc;

        internal EventClient(BuiltinHandlerActivator adapter, string connectionstring, Guid documentProviderId,
            string apikey, bool secondaryKey)
        {
            this.adapter = adapter;
            _connectionstring = connectionstring;
            _documentProviderId = documentProviderId;
            _queuename = _documentProviderId.ToString("n");
            _apikey = apikey;
            _secondaryKey = secondaryKey;
        }

        internal bool TestEnvironment { get; set; }
        internal string APIURL { get; set; }

        internal void SubScribeToDocumentPadesSavedEvent(Func<DocumentPadesSavedEvent, byte[], Task> func)
        {
            adapter.Handle<DocumentPadesSavedEvent>(async (bus, context, msg) =>
            {
                var data = await DownloadPades(msg.DocumentId);
                await func(msg, data);
            });
        }

        internal void SubScribeToDocumentSDOSavedEvent(Func<DocumentSDOSavedEvent, byte[], Task> func)
        {
            adapter.Handle<DocumentSDOSavedEvent>(async (bus, context, msg) =>
            {
                var data = await DownloadSDO(msg.DocumentId);
                await func(msg, data);
            });
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

        /// <summary>
        ///     Setup the EventClient to download events from the ServiceBus and files from the Signere API
        /// </summary>
        /// <param name="azureServiceBusConnectionString">ServiceBus connectionstring contact signere support to get this</param>
        /// <param name="DocumentProvider">Your account ID</param>
        /// <param name="ApiKey">Your primary API key</param>
        /// <returns></returns>
        public static EventClient SetupWithPrimaryApiKey(string azureServiceBusConnectionString, Guid DocumentProvider,
            string ApiKey)
        {
            var adapter = new BuiltinHandlerActivator();


            return new EventClient(adapter, azureServiceBusConnectionString, DocumentProvider, ApiKey, false);
        }

        /// <summary>
        ///     Setup the EventClient to download events from the ServiceBus and files from the Signere API
        /// </summary>
        /// <param name="azureServiceBusConnectionString">ServiceBus connectionstring contact signere support to get this</param>
        /// <param name="DocumentProvider">Your account ID</param>
        /// <param name="ApiKey">Your secondary API key</param>
        /// <returns></returns>
        public static EventClient SetupWithSecondaryApiKey(string azureServiceBusConnectionString, Guid DocumentProvider,
            string ApiKey)
        {
            var adapter = new BuiltinHandlerActivator();


            return new EventClient(adapter, azureServiceBusConnectionString, DocumentProvider, ApiKey, true);
        }

        internal void Start()
        {
            Bus = Configure.With(adapter)
                .Transport(x => x.UseAzureServiceBus(_connectionstring, _queuename, AzureServiceBusMode.Standard))
                .Options(c => { c.SimpleRetryStrategy(_queuename + "_error", 5, true); })
                //.Logging(x=>x.)
                .Start();
        }


        #region Download files
        private async Task<byte[]> DownloadSDO(Guid documentId)
        {
            var url = CreateUrl("api/DocumentFile/Signed/{0}", documentId, TestEnvironment);

            return await DownloadFile(url);
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

        private async Task<byte[]> DownloadPades(Guid documentId)
        {
            var url = CreateUrl("api/DocumentFile/SignedPDF/{0}", documentId, TestEnvironment);
            return await DownloadFile(url);
        }

        private string CreateUrl(string path, Guid documentId, bool testEnvironment)
        {
            var apiUrl = testEnvironment ? "https://testapi.signere.no" : "https://api.signere.no";
            if (!string.IsNullOrWhiteSpace(APIURL))
                apiUrl = APIURL;
            return string.Format("{0}/{1}", apiUrl, string.Format(path, documentId));
        }
        #endregion

    }
}