using Data;
using MassTransit;
using Microsoft.Extensions.Logging;
using dotnet.EventDriven.Constant.Models;

namespace Api;


public class UpdateTodoConsumer : IConsumer<UpdateTodo>
{
    private readonly IBus _bus;
    private readonly ILogger<UpdateTodoConsumer> _logger;
    private readonly TodoRepository _todoRepository;

    public UpdateTodoConsumer(
        IBus bus,
        ILogger<UpdateTodoConsumer> logger,
        TodoRepository todoRepository)
    {
        _bus = bus;
        _logger = logger;
        _todoRepository = todoRepository;
    }

    public async Task Consume(ConsumeContext<UpdateTodo> context)
    {
        _logger.LogInformation("Consuming UpdateTodo");
        var todo = context.Message.Data;
        await _todoRepository.UpdateTodo(todo);
    }
}

public class UpdateTodoConsumerDefinition : ConsumerDefinition<UpdateTodoConsumer>
{
    public UpdateTodoConsumerDefinition()
    {
        EndpointName = "demo-update-todo";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UpdateTodoConsumer> consumerConfigurator,
        IRegistrationContext registrationContext)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 5000));
        endpointConfigurator.UseInMemoryOutbox(registrationContext);
    }

}
