using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebus.Messages;
using Rebus.Pipeline;

namespace Unipluss.Sign.Events.Client.Infrastructur
{
    public class NamespaceFilterStep : IIncomingStep
    {
        public async Task Process(IncomingStepContext context, Func<Task> next)
        {
            var message = context.Load<TransportMessage>();
            var messageType = message.Headers.ContainsKey(Rebus.Messages.Headers.Type) ? message.Headers[Rebus.Messages.Headers.Type] : "";
            if (!messageType.StartsWith("Unipluss.Sign.Events.Entities")) return;

            await next();
        }
    }
}
