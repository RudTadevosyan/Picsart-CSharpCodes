namespace TodoShared;

public class TodoUpdateModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsDone { get; set; }
}