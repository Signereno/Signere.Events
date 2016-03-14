using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unipluss.Sign.Events.Client;
using Unipluss.Sign.Events.Entities;

namespace Unipluss.Sign.Events.Test
{
     class Program
    {
        static void Main(string[] args)
        {
            var client=EventClient
                
                .SetupWithPrimaryApiKey("",Guid.NewGuid(),"")
                .UseTestEnvironment(true)   
                .LogToConsole()                            
                .SubscribeToDocumentSignedEvent(DocumentSignedEvent)
                .SubscribeToDocumentCanceledEvent(DocumentCanceledEvent)
                .SubscribeToDocumentPartialSignedEvent(DocumentPartialSignedEvent)
                .SubscribeToDocumentPadesSavedEvent(DocumentPadesSavedEvent)
                .SubscribeToDocumentSDOSavedEvent(DocumentSDOSavedEvent)
                .Start();

            Console.ReadLine();
            client.Dispose();
        }

        private static async Task DocumentSDOSavedEvent(DocumentSDOSavedEvent arg1, byte[] sdoBytes)
        {
            System.IO.File.WriteAllBytes(string.Format("{0}.sdo", arg1.DocumentId), sdoBytes);
        }

        private static async Task DocumentPadesSavedEvent(DocumentPadesSavedEvent arg1, byte[] padesBytes)
        {
            System.IO.File.WriteAllBytes(string.Format("{0}_pades.pdf",arg1.DocumentId),padesBytes);
            
        }

        private static async Task DocumentPartialSignedEvent(DocumentPartialSignedEvent arg)
        {
            System.IO.File.WriteAllText(string.Format("{0}_partial.json", arg.DocumentId), Newtonsoft.Json.JsonConvert.SerializeObject(arg, Formatting.Indented));
        }

        private static async  Task DocumentCanceledEvent(DocumentCanceledEvent arg)
        {
            System.IO.File.WriteAllText(string.Format("{0}_cancled.json", arg.DocumentId), Newtonsoft.Json.JsonConvert.SerializeObject(arg, Formatting.Indented));
        }

        private static Task DocumentSignedEvent(DocumentSignedEvent arg)
        {
            System.IO.File.WriteAllText(string.Format("{0}.json", arg.DocumentId), Newtonsoft.Json.JsonConvert.SerializeObject(arg, Formatting.Indented));
            return Task.FromResult(true);
        }
    }
}
