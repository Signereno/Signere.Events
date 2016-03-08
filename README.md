# Signere.Events
### Signere event client.

Subscribe to Document events from Signere.no using Rebus and AzureServicebus.
This is much easier than pulling from a webservice. The events are instant and more resilient than polling.

All eventdata is encrypted with AES256 encryption.

The client is built with a fluent API see example:

```csharp
        EventClient
                .SetupWithPrimaryApiKey("connectionstring",AccountIDGUID,"yourapikey")
                .UseTestEnvironment(true)
                .SubScribeToDocumentSignedEvent(DocumentSignedEvent)
                .SubScribeToDocumentCancledEvent(DocumentCancledEvent)
                .SubScribeToDocumentPartialSignedEvent(DocumentPartialSignedEvent)
                .SubScribeToDocumentPadesSavedEvent(DocumentPadesSavedEvent)
                .Start();
```
