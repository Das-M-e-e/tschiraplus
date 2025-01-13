namespace Services.DTOs;

public class TagDto
{
    public Guid TagId { get; init; }
    public string Title { get; init; }
    public Guid ProjectId { get; set; }
    public string? Description { get; set; }
    public string ColorCode { get; set; }
    
}