﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unipluss.Sign.Events.Entities
{
    public class DocumentSignedEvent
    {
        public Guid DocumentId { get; set; }

        public DateTime SignedTimeStamp { get; set; }
       
        public string ExternalDocumentId { get; set; }

        public IEnumerable<Signee> Signees { get; set; }
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
    }

    public class DocumentPartialSignedEvent
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
    }

    public class DocumentCancledEvent
    {
        public Guid DocumentId { get; set; }

        public string CancledMessage { get; set; }

        public string ExternalDocumentId { get; set; }
    }

    public class DocumentPadesSavedEvent
    {
        public Guid DocumentId { get; set; }

        public string ExternalDocumentId { get; set; }
    }

    public class DocumentSDOSavedEvent
    {
        public Guid DocumentId { get; set; }

        public string ExternalDocumentId { get; set; }
    }
}
