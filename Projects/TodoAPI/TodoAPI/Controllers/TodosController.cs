using Microsoft.AspNetCore.Mvc;
using TodoCore;
using TodoService;
using TodoShared;

namespace TodoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly TodoServices _todoService;

    public TodosController(TodoServices todoService)
    {
        _todoService = todoService;
    }

    [HttpGet("todos")]
    public ActionResult<IEnumerable<TodoModel>> Get()
    {
        return Ok(_todoService.GetAll());
    }

    [HttpGet("todos/{id}")]
    public ActionResult<TodoModel> GetById(int id)
    {
        var todo = _todoService.GetById(id);
        if (todo == null) return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    public ActionResult Create([FromBody] TodoCreationModel model) //it will create TodoCreationModel and pass
    {
        _todoService.CreateTodo(model);
        return Ok();
    }


    [HttpPut("todos/update/{id}")]
    public ActionResult Update(int id, [FromBody] TodoUpdateModel model)
    {
        bool updated = _todoService.UpdateTodo(id, model);
        if (!updated) return NotFound();
        return Ok();
    }

    [HttpDelete("todos/delete/{id}")]
    public ActionResult Delete(int id)
    {
        if (!_todoService.Delete(id)) return NotFound();
        return Ok();
    }
}