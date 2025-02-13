namespace Core.Models;

public class ProjectInvitationModel
{
    public Guid ProjectInvitationId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid RecipientId { get; set; }
    public Guid InviterId { get; set; }
    public bool Accepted { get; set; }
}