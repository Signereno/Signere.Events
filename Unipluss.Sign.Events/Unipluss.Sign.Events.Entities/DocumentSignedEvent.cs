using System;
using System.Collections.Generic;

namespace Unipluss.Sign.Events.Entities
{
    public class DocumentSignedEvent
    {
        public Guid DocumentId { get; set; }

        public DateTime SignedTimeStamp { get; set; }
       
        public string ExternalDocumentId { get; set; }

        public List< Signee> Signees { get; set; }
    }

    public class Signee
    {
        public Guid SigneeRefId { get; set; }

        public string SignName { get; set; }

        public DateTime SignedTime { get; set; }

        public string DateOfBirth { get; set; }

        public string ExternalSigneeId { get; set; }

        public string IdentityProviderId { get; set; }

        public string IdentityProvider { get; set; }

        public string SocialSecurityNumber { get; set; }

    }

    public class DocumentPartiallySignedEvent
    {
        public Guid DocumentId { get; set; }

        public Guid SigneeRefId { get; set; }

        public string SignName { get; set; }

        public DateTime SignedTime { get; set; }

        public string DateOfBirth { get; set; }

        public string ExternalDocumentId { get; set; }

        public string ExternalSigneeId { get; set; }

        public string IdentityProviderId { get; set; }

        public string IdentityProvider { get; set; }

        public string SocialSecurityNumber { get; set; }
    }

    public class DocumentCanceledEvent
    {
        public Guid DocumentId { get; set; }

        public string CanceledMessage { get; set; }

        public string ExternalDocumentId { get; set; }
    }

    public class DocumentPadesSavedEvent
    {
        public Guid DocumentId { get; set; }

        public string ExternalDocumentId { get; set; }

        public Guid? SchemaId { get; set; }

        public string Schema { get; set; }
    }

    public class DocumentSDOSavedEvent
    {
        public Guid DocumentId { get; set; }

        public string ExternalDocumentId { get; set; }

        public Guid? SchemaId { get; set; }

        public string Schema { get; set; }
    }

    public class DocumentFormPartiallySignedEvent
    {
        public Guid DocumentId { get; set; }

        public Guid SigneeRefId { get; set; }

        public Guid SchemaId { get; set; }

        public string Schema { get; set; }

        public string SignName { get; set; }

        public DateTime SignedTime { get; set; }

        public string DateOfBirth { get; set; }

        public string IdentityProviderId { get; set; }

        public string IdentityProvider { get; set; }

        public string SocialSecurityNumber { get; set; }

        public Dictionary<string,string> FormFields { get; set; }

        public string ExternalDocumentId { get; set; }
    }

    public class DocumentFormSignedEvent
    {
        public Guid DocumentId { get; set; }

        public DateTime SignedTimeStamp { get; set; }

        public Guid SchemaId { get; set; }

        public string Schema { get; set; }

        public List<Signee> Signees { get; set; }

        public Dictionary<string, string> FormFields { get; set; }

        public string ExternalDocumentId { get; set; }

    }

}
