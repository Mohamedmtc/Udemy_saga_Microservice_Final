using MassTransit;
using Messages.Order.Command;
using Messages.Order.Event;
using System.Threading.Tasks;

namespace CartService.Consumer
{
    public class CheckOrderStateConsumer : IConsumer<ICheckOrderStateCommand>
    {
        private readonly ILogger<CheckOrderStateConsumer> _logger;


        public CheckOrderStateConsumer(ILogger<CheckOrderStateConsumer> logger)
        {
            _logger = logger;

        }
        public async Task Consume(ConsumeContext<ICheckOrderStateCommand> context)
        {
            // do some sorting of business
            if (context.Message.IsCanceled)
            {
                await context.Publish<IOrderCanceledEvent>(new
                {
                    context.Message.OrderId,
                    ExceptionMessage = "order canceled"
                });

                // Log the step
                _logger.LogInformation("Order Canceled - OrderId: {OrderId}", context.Message.OrderId);
            }
            else
            {
                await context.Publish<IOrderFinishedEvent>(new
                {
                    context.Message.OrderId
                });

                // Log the step
                _logger.LogInformation("Order Finished - OrderId: {OrderId}", context.Message.OrderId);
            }
        }
    }
}
