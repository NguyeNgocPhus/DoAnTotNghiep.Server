using DoAn.API.DependencyInjection.Options;
using DoAn.Application.Exceptions;
using DoAn.Shared.Extensions;
using DoAn.Shared.Services.V1.FileStorage.Commands;
using DoAn.Shared.Services.V1.FileStorage.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBox.Extensions;

namespace DoAn.API.Controllers;

public class FileStorageController : ApiControllerBase
{

    private readonly IMediator _mediator;
    private readonly FileConfiguration _fileConfiguration = new FileConfiguration();
  
    public FileStorageController(IConfiguration configuration, IMediator mediator)
    {
        _mediator = mediator;
        configuration.GetSection(nameof(FileConfiguration)).Bind(_fileConfiguration);
    }

    [HttpPost]
    [Route(Common.Url.ADMIN.FileStorage.Upload)]
    // [Permissions]
    public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        ValidateFile(file);
        var result = await _mediator.Send(new UploadFileCommand()
        {
            Data = file.OpenReadStream().ToByteArray(),
            Length = (long) file.Length,
            Name = file.FileName,
            MimeType = file.ContentType,
        },cancellationToken);

        if (!result.IsSuccess)
        {
            throw new UnableToUploadFileException( nameof(UploadFileAsync));
        }
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Route(Common.Url.ADMIN.FileStorage.Get)]
    public async Task<IActionResult> GetFileAsync([FromRoute] Guid id, [FromQuery] string name, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetFileStorageQuery()
        {
            Id = id
        }, cancellationToken);
        if (!result.IsSuccess)
        {
            // throw new FileNotFoundException(nameof(FileStorageController), nameof(GetFileAsync), $"File is not found");
        }
        var response = new FileContentResult(result.Value.Data, result.Value.MimeType)
        {
            FileDownloadName = $"{name}",
            EnableRangeProcessing = true
        };
        return response;
    
    }
    private void ValidateFile(IFormFile file)
    {
        if (!file.FileName.IsAllowedFileExtension(_fileConfiguration.AllowedExtensions) || !file.ContentType.IsAllowedMimeType(_fileConfiguration.AllowedContentTypes))
            throw new UnableToUploadFileException("File type or mimetype is not allowed");
        if (file.Length > 1048576 * _fileConfiguration.MaxSize)
            throw new FileSizeTooBigException( $"File size is too big, more than {_fileConfiguration.MaxSize} Mb");
    }
    
}
