using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.FileStorage.Responses;

namespace DoAn.Shared.Services.V1.FileStorage.Queries;

public class GetFileStorageQuery: IQuery<FileStorageResponse>
{
    public Guid Id { get; set; }
}