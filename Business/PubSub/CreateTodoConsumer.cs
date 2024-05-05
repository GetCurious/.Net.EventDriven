using Data;
using MassTransit;
using Microsoft.Extensions.Logging;
using dotnet.EventDriven.Constant.Models;

namespace Api;


public class CreateTodoConsumer : IConsumer<CreateTodo>
{
    private readonly IBus _bus;
    private readonly ILogger<CreateTodoConsumer> _logger;
    private readonly TodoRepository _todoRepository;

    public CreateTodoConsumer(
        IBus bus,
        ILogger<CreateTodoConsumer> logger,
        TodoRepository todoRepository)
    {
        _bus = bus;
        _logger = logger;
        _todoRepository = todoRepository;
    }

    public async Task Consume(ConsumeContext<CreateTodo> context)
    {
        _logger.LogInformation("Consuming CreateTodo");
        var todo = context.Message.Data;
        await _todoRepository.CreateTodo(todo);
    }
}

public class CreateTodoConsumerDefinition : ConsumerDefinition<CreateTodoConsumer>
{
    public CreateTodoConsumerDefinition()
    {
        EndpointName = "demo-create-todo";
        // EndpointConvention.Map<CreateTodo>(new Uri($"queue:{nameof(Api)}"));
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateTodoConsumer> consumerConfigurator,
        IRegistrationContext registrationContext)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 5000));
        endpointConfigurator.UseInMemoryOutbox(registrationContext);
    }

}
