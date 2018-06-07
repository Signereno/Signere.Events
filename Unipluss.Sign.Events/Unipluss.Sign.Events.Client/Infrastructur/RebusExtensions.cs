using Rebus.Config;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;

namespace Unipluss.Sign.Events.Client.Infrastructur
{
    public static class RebusExtensions
    {
        public static void AddNamespaceFilter(this OptionsConfigurer configurer)
        {
            configurer.Decorate<IPipeline>(c =>
            {
                var incomingStep = new NamespaceFilterStep();
                var pipeline = c.Get<IPipeline>();
                return new PipelineStepInjector(pipeline)
                    .OnReceive(incomingStep, PipelineRelativePosition.Before, typeof(DeserializeIncomingMessageStep));
            });
        }
    }
}