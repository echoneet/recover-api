namespace Recover.Models;

public class Issue
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public bool IsCancelled { get; set; } = false;
}