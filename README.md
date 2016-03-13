# Signere.Events
### Signere event client.

Subscribe to Document events from Signere.no using Rebus and AzureServicebus.
This is much easier than polling from a webservice. The events are instant and more resilient than polling.

All eventdata is encrypted with AES256 encryption.

The client is built with a fluent API see example:

```csharp
     var client=EventClient
  
          .SetupWithPrimaryApiKey("",Guid.NewGuid(),"")
          .UseTestEnvironment(true)   
          .LogToConsole()                            
          .SubScribeToDocumentSignedEvent(DocumentSignedEvent)
          .SubScribeToDocumentCancledEvent(DocumentCancledEvent)
          .SubScribeToDocumentPartialSignedEvent(DocumentPartialSignedEvent)
          .SubScribeToDocumentPadesSavedEvent(DocumentPadesSavedEvent)
          .SubScribeToDocumentSDOSavedEvent(DocumentSDOSavedEvent)
          .Start();


          client.Dispose();
```
