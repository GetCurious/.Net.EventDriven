using dotnet.EventDriven.Constant.Models;

namespace Data;

public class TodoRepository
{
    private List<Todo> FakeTodoDb { get; set; }

    public TodoRepository()
    {
        FakeTodoDb =
        [
            new Todo() { Id = Guid.Parse("28bb7d1f-6105-4ff1-87b3-6b5b737f88fd"), Title = "Sleep", IsCompleted = true },
            new Todo() { Id = Guid.Parse("b8a69912-a9a3-401b-bdcb-1967664f4971"), Title = "Work", IsCompleted = false },
            new Todo() { Id = Guid.Parse("3bd88131-4abc-443c-b7dd-63c29b96ac4f"), Title = "Eat", IsCompleted = false },
        ];
    }

    public List<Todo> GetTodos()
    {
        return [.. FakeTodoDb];
    }

    public async Task CreateTodo(Todo todo)
    {
        Random random = new();
        await Task.Delay(1000 * random.Next(1, 3));

        if (todo.Id == Guid.Empty)
            todo = todo with { Id = Guid.NewGuid() };

        FakeTodoDb.Add(todo);
    }

    public async Task DeleteTodo(Guid id)
    {
        Random random = new();
        await Task.Delay(1000 * random.Next(1, 3));

        int index = FakeTodoDb.FindIndex(t => t.Id == id);
        if (index == -1) return;

        FakeTodoDb.RemoveAt(index);
    }

    public async Task UpdateTodo(Todo todo)
    {
        Random random = new();
        await Task.Delay(1000 * random.Next(1, 3));

        int index = FakeTodoDb.FindIndex(t => t.Id == todo.Id);
        if (index == -1) return;

        FakeTodoDb[index] = todo;
    }

}
