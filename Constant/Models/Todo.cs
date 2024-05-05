
using System.ComponentModel.DataAnnotations;

namespace dotnet.EventDriven.Constant.Models;

public record Todo
{
    public Guid Id { get; set; } = Guid.Empty;

    [Required(ErrorMessage = "Please enter a todo.")]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;
}


// [PubSub Commands] for point-to-point actions
public record CreateTodo(Todo Data);
public record UpdateTodo(Todo Data);
public record DeleteTodo(Guid Data);

// [PubSub Events] for idempotent intentions
public record CreatedTodo();
public record UpdatedTodo();
public record DeletedTodo();

// DEV Note: https://github.com/MassTransit/MassTransit/discussions/3495