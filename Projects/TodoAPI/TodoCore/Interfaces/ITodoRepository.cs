namespace TodoCore.Interfaces;

public interface ITodoRepository
{
    IEnumerable<TodoModel> GetAll();
    TodoModel? GetById(int id);
    void Create(TodoModel todo);
    void Update(TodoModel todo);
    bool Delete(int id);
}