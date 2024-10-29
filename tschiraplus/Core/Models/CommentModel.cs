namespace Core.Models;

public class CommentModel
{
    public Guid CommentId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastUpdatedTime { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public List<AttachmentModel>? Attachments { get; set; }
    public bool isEdited { get; set; }
    public bool isDeleted { get; set; }
}