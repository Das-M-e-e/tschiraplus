using Core.Enums;

namespace Core.Models;

public class AttachmentModel
{
    public Guid AttachmentId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? CommentId { get; set; }
    public Guid UploadedBy { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string? Description { get; set; }
    public long FileSize { get; set; }
    public FileType FileType { get; set; }
    public DateTime UploadedDate { get; set; }
}