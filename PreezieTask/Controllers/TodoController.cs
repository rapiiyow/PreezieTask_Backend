using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> GetTodos()
    {
        return Ok(_todoService.GetTodos());
    }

    [HttpPost]
    public ActionResult AddTodo([FromBody] TodoItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Title))
        {
            return BadRequest("Todo title cannot be empty.");
        }

        _todoService.AddTodoItem(item);
        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult ToggleTodoCompletion(int id)
    {
        var result = _todoService.ToggleTodoCompletion(id);
        if (!result)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTodoItem(int id)
    {
        var result = _todoService.DeleteTodoItem(id);
        if (!result)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        return Ok();
    }
}
