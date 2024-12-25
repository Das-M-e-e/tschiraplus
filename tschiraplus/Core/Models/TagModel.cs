namespace Core.Models;

public class TagModel
{
    public Guid TagId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string ColorCode { get; set; }
    public DateTime CreatedAt { get; set; }
}