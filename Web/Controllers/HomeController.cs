using System.Diagnostics;
using dotnet.EventDriven.Constant.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    // [Start] TODO-App
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken token)
    {
        var httpClient = _httpClientFactory.CreateClient("api");
        var todo = await httpClient.GetFromJsonAsync<List<Todo>>($"/todos", token);
        return View(todo);
    }

    [HttpGet]
    [Route("GetTodo/{id}")]
    public async Task<IActionResult> GetTodo([FromRoute(Name = "id")] Guid? id, CancellationToken token)
    {
        if (id is null || id == Guid.Empty)
            return PartialView("Todo/_TodoItem");

        var httpClient = _httpClientFactory.CreateClient("api");
        var todo = await httpClient.GetFromJsonAsync<Todo>($"/todo?id={id}", token);
        return PartialView("Todo/_TodoItem", todo);
    }

    [HttpGet]
    [Route("EditTodo/{id}")]
    public async Task<IActionResult> EditTodo([FromRoute(Name = "id")] Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return PartialView("Todo/_EditTodoItem", new Todo());

        var httpClient = _httpClientFactory.CreateClient("api");
        var todo = await httpClient.GetFromJsonAsync<Todo>($"/todo?id={id}", token);
        return PartialView("Todo/_EditTodoItem", todo);
    }

    [HttpDelete]
    [Route("DeleteTodo/{id}")]
    public async Task<IActionResult> DeleteTodo([FromRoute(Name = "id")] Guid id, CancellationToken token)
    {
        var httpClient = _httpClientFactory.CreateClient("api");
        var response = await httpClient.DeleteAsync($"/todo/{id}", token);
        if (!response.IsSuccessStatusCode)
            return BadRequest();

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodo([FromForm] Todo? todo, CancellationToken token)
    {
        var httpClient = _httpClientFactory.CreateClient("api");
        var response = await httpClient.PutAsJsonAsync("/todo", todo, token);
        if (!response.IsSuccessStatusCode)
            return BadRequest();

        return PartialView("Todo/_TodoItem", todo);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo([FromForm] Todo todo, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return PartialView("Todo/_EditTodoItem", todo);

        todo = todo with { Id = Guid.NewGuid() };
        var httpClient = _httpClientFactory.CreateClient("api");
        var response = await httpClient.PostAsJsonAsync("/todo", todo, token);
        if (!response.IsSuccessStatusCode)
            return BadRequest();

        return PartialView("Todo/_TodoItem", todo);
    }
    // [End] TODO-App

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
