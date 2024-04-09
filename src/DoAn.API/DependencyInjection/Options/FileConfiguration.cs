namespace DoAn.API.DependencyInjection.Options;

public class FileConfiguration
{
    public string FfmpegImage { get; set; } = "";

    public bool VirusScanningRequired { get; set; } = true;

    public bool VideoThumbnailRequired { get; set; } = true;

    public string UploadFolder { get; set; } = "";

    public string ExportFolder { get; set; } = "";

    public bool EnableRecaptchaProtection { get; set; } = true;

    public int MaxSize { get; set; } = 25;

    public List<string> AllowedExtensions { get; set; } = new List<string>();

    public List<string> AllowedContentTypes { get; set; } = new List<string>();
    

    public List<string> VideoExts { get; set; } = new List<string>();
}