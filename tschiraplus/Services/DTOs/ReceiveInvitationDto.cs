namespace Services.DTOs;

public class ReceiveInvitationDto
{
    public string ProjectName { get; set; }
    public string InviterName { get; set; }
    public bool Accepted { get; set; }

}