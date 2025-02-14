namespace Services.DTOs;

public class TaskInvitationDto
{
    public string Username { get; set; }
    public Guid InviterId { get; set; }
    public Guid TaskId { get; set; }
}