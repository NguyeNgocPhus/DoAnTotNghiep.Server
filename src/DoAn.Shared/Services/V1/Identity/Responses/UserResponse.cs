namespace DoAn.Shared.Services.V1.Identity.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}