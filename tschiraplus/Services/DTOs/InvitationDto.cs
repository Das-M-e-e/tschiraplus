namespace Services.DTOs;

public class InvitationDto
{
    public string Username { get; set; }
    public Guid InviterId { get; set; }
    public Guid ProjectId { get; set; }
}