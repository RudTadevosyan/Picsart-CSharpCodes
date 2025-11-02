using TodoCore;
using TodoCore.Interfaces;
namespace TodoDAL;

public class TodoRepository : ITodoRepository
{
    private static readonly Dictionary<int, TodoModel> TodoDb = new();

    public IEnumerable<TodoModel> GetAll()
    {
        return TodoDb.Values.ToList();
    }

    public TodoModel? GetById(int id)
    {
        return TodoDb.GetValueOrDefault(id);
    }

    public void Create(TodoModel todo)
    {
        TodoDb.Add(todo.Id, todo);
    }

    public void Update(TodoModel todo)
    {
        TodoDb[todo.Id] = todo;
    }

    public bool Delete(int id)
    {
        return TodoDb.Remove(id);
    }
}