namespace Services.DTOs;

public class TaskDto  //Kommunikations-Objekt für Service und anderen Ebenen 
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreationDate { get; set; }
}