using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.FileStorage.Responses;

namespace DoAn.Shared.Services.V1.FileStorage.Commands;

public class UploadFileCommand : ICommand<FileStorageResponse>
{
    public byte[] Data { get; set; }
    public long Length { get; set; }
    public string Name { get; set; }
    public string MimeType { get; set; }
}