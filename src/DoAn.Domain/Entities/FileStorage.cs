using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities;

public class FileStorage: AuditableEntity<Guid>
{
    public int Code { get; set; }
    public string Status { get; set; }
    public string MimeType { get; set; }
    public long Size { get; set; }
    public string OriginalName { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }
}