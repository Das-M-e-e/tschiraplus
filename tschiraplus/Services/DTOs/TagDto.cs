namespace Services.DTOs;

public class TagDto
{
    public Guid TagId { get; init; }
    public string Title { get; init; }
    public Guid? ProjectId { get; init; }
    public string? Description { get; init; }
    public string ColorCode { get; init; }
    
}