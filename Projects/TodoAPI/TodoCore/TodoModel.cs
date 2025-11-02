namespace TodoCore;

public class TodoModel //Database Model
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsDone { get; set; }
}