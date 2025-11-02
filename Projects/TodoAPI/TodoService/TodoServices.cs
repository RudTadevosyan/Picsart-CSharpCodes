using TodoCore;
using TodoCore.Interfaces;
using TodoShared;

namespace TodoService;

public class TodoServices
{
    private ITodoRepository _todoRepository;
    private static int _id = 1;
    public TodoServices(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public TodoModel? GetById(int id)
    {
        return _todoRepository.GetById(id);
    }

    public IEnumerable<TodoModel> GetAll()
    {
        return _todoRepository.GetAll();
    }

    public void CreateTodo(TodoCreationModel model)
    {
        TodoModel todo = new TodoModel
        {
            Id = _id++,
            Name = model.Name,
            Description = model.Description,
            CreationDate = DateTime.Now,
            DueDate = model.DueDate,
            IsDone = false
        };
        
        _todoRepository.Create(todo);
    }

    public bool UpdateTodo(int id, TodoUpdateModel model)
    {
        var todo = _todoRepository.GetById(id);
        if (todo == null) return false;

        todo.Name = model.Name;
        todo.Description = model.Description;
        todo.DueDate = model.DueDate;
        todo.IsDone = model.IsDone;
        
        _todoRepository.Update(todo);
        return true;
    }

    public bool Delete(int id)
    {
        return _todoRepository.Delete(id);
    }
}