# Signere.Events
### Signere event client.

With this event client, it is possible to subscribe to document events from Signere.no by using Rebus and AzureServiceBus.
This is much easier than polling from a web service. The events are instant and more resilient than polling. All event data are encrypted using AES256 encryption.

The client is built with a fluent API - see an example below for how to set it up:

```csharp
     var client=EventClient
  
          .SetupWithPrimaryApiKey("","","")
          .UseTestEnvironment(true)   
          .LogToConsole()                            
          .SubscribeToDocumentSignedEvent(DocumentSignedEvent)
          .SubscribeToDocumentCanceledEvent(DocumentCanceledEvent)
          .SubscribeToDocumentPartiallySignedEvent(DocumentPartiallySignedEvent)
          .SubscribeToDocumentPadesSavedEvent(DocumentPadesSavedEvent)
          .SubscribeToDocumentSDOSavedEvent(DocumentSDOSavedEvent)
          .Start();

     //Always remember to dispose the client!
     client.Dispose();
```
Using the example above, the client will subscribe to the events "DocumentSigned", "DocumentCanceled", "DocumentPartiallySigned", "DocumentPadesSaved" and "DocumentSDOSaved" (you can choose the ones that are relevant to you). Note that in the first line, .SetupWithPrimaryApiKey, you have to provide your service bus connection string, your account ID and your API key, respectively (which are left blank in the example above). You can find the account ID and API keys associated with your account at any time by logging in to signere.no. To obtain a service bus connection string, you can contact support@signere.no.

### Nuget
Download the library from [Nuget](http://www.nuget.org/packages/Signere.Events/)
