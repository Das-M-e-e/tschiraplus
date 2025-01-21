namespace Core.Models;

public class TagModel
{
    public Guid TagId { get; set; }
    public string Title { get; set; }
    public Guid? ProjectId { get; set; }
    public string? Description { get; set; }
    public string ColorCode { get; set; }
}