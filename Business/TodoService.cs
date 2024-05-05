using Data;
using dotnet.EventDriven.Constant.Models;
using MassTransit;

namespace Business;

public class TodoService
{
    private readonly IBus _bus;
    private readonly TodoRepository _todoRepository;

    public TodoService(IBus bus, TodoRepository todoRepository)
    {
        _bus = bus;
        _todoRepository = todoRepository;
    }

    public List<Todo> GetTodos()
    {
        var todos = _todoRepository.GetTodos();
        return todos;
    }

    public async Task CreateTodo(Todo todo, CancellationToken cancellationToken)
    {
        // await _bus.Publish(new CreateTodo(todo), cancellationToken);
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:demo-create-todo"));
        await endpoint.Send(new CreateTodo(todo), cancellationToken);
    }

    public async Task DeleteTodo(Guid id, CancellationToken cancellationToken)
    {
        // await _bus.Publish(new DeleteTodo(id), cancellationToken);
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:demo-delete-todo"));
        await endpoint.Send(new DeleteTodo(id), cancellationToken);
    }

    public async Task UpdateTodo(Todo todo, CancellationToken cancellationToken)
    {
        // await _bus.Publish(new UpdateTodo(todo), cancellationToken);
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:demo-update-todo"));
        await endpoint.Send(new UpdateTodo(todo), cancellationToken);
    }

}
