using System;
using System.Threading.Tasks;
using Rebus.Messages;
using Rebus.Pipeline;

namespace Unipluss.Sign.Events.Client.Rebus.v4.Infrastructur
{
    public class NamespaceFilterStep : IIncomingStep
    {
        public async Task Process(IncomingStepContext context, Func<Task> next)
        {

            var message = context.Load<TransportMessage>();
            var messageType = message.Headers.ContainsKey(Headers.Type) ? message.Headers[Headers.Type] : "";
            if (!messageType.StartsWith("Unipluss.Sign.Events.Entities")) return;

            await next();
        }
    }
}
