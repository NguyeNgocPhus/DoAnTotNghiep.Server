namespace DoAn.Shared.Services.V1.Identity.Responses;

public class LoginResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}