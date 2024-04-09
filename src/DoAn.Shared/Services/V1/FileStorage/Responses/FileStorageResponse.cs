namespace DoAn.Shared.Services.V1.FileStorage.Responses;

public class FileStorageResponse
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public string Status { get; set; }
    public string MimeType { get; set; }
    public long Size { get; set; }
    public string OriginalName { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }
}