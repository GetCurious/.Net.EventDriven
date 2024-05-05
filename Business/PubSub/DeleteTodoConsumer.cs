using Data;
using MassTransit;
using Microsoft.Extensions.Logging;
using dotnet.EventDriven.Constant.Models;

namespace Api;


public class DeleteTodoConsumer : IConsumer<DeleteTodo>
{
    private readonly IBus _bus;
    private readonly ILogger<DeleteTodoConsumer> _logger;
    private readonly TodoRepository _todoRepository;

    public DeleteTodoConsumer(
        IBus bus,
        ILogger<DeleteTodoConsumer> logger,
        TodoRepository todoRepository)
    {
        _bus = bus;
        _logger = logger;
        _todoRepository = todoRepository;
    }

    public async Task Consume(ConsumeContext<DeleteTodo> context)
    {
        _logger.LogInformation("Consuming DeleteTodo");
        Guid todoId = context.Message.Data;
        await _todoRepository.DeleteTodo(todoId);
    }
}

public class DeleteTodoConsumerDefinition : ConsumerDefinition<DeleteTodoConsumer>
{
    public DeleteTodoConsumerDefinition()
    {
        EndpointName = "demo-delete-todo";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeleteTodoConsumer> consumerConfigurator,
        IRegistrationContext registrationContext)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 5000));
        endpointConfigurator.UseInMemoryOutbox(registrationContext);
    }

}
