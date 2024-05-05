using Api;
using Business;
using Data;
using dotnet.EventDriven.Constant.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<TodoService>();
builder.Services.AddSingleton<TodoRepository>();

// Register PubSub(RabbitMQ) using MassTransit
builder.Services.AddMassTransit(x =>
{
   x.AddConsumer<CreateTodoConsumer, CreateTodoConsumerDefinition>();
   x.AddConsumer<DeleteTodoConsumer, DeleteTodoConsumerDefinition>();
   x.AddConsumer<UpdateTodoConsumer, UpdateTodoConsumerDefinition>();

   // x.AddSagas(entryAssembly);
   // x.AddSagaStateMachines(entryAssembly);
   
   // x.AddActivities(entryAssembly);

   // x.SetInMemorySagaRepositoryProvider();
   // x.UsingInMemory((context, cfg) =>
   // {
   //    cfg.ConfigureEndpoints(context);
   // });

   x.SetKebabCaseEndpointNameFormatter();
   x.UsingRabbitMq((context, config) =>
   {
      config.Host("localhost", "/", h =>
      {
         h.Username("test");
         h.Password("test");
      });
      config.ConfigureEndpoints(context);
   });
});


// Services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// [Start] Minimal API

app.MapGet("/todos", ([FromServices] TodoService todoService, CancellationToken token) =>
{
   var todos = todoService.GetTodos();
   return todos;
})
.WithName("GetTodos")
.WithOpenApi();

app.MapGet("/todo", ([FromQuery(Name = "id")] Guid id, [FromServices] TodoService todoService, CancellationToken token) =>
{
   var todos = todoService.GetTodos();
   return todos.FirstOrDefault(t => t.Id == id);
})
.WithName("GetTodoById")
.WithOpenApi();

app.MapPost("/todo", async ([FromBody] Todo? todo, [FromServices] TodoService todoService, CancellationToken token) =>
{
   if (todo is null) return;
   await todoService.CreateTodo(todo, token);
})
.WithName("CreateTodo")
.WithOpenApi();

app.MapDelete("/todo/{id}", async ([FromRoute(Name = "id")] Guid id, [FromServices] TodoService todoService, CancellationToken token) =>
{
   await todoService.DeleteTodo(id, token);
})
.WithName("DeleteTodo")
.WithOpenApi();

app.MapPut("/todo", async ([FromBody] Todo todo, [FromServices] TodoService todoService, CancellationToken token) =>
{
   await todoService.UpdateTodo(todo, token);
})
.WithName("UpdateTodo")
.WithOpenApi();

// [End] Minimal API

app.Run();
