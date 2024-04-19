namespace DoAn.Application.Abstractions;

public interface IPasswordGeneratorService
{
    
    /// <summary>
    /// To hash a password
    /// </summary>
    /// <param name="password">Raw password to hash</param>
    /// <returns>Hash password</returns>
    public string HashPassword(string password);

    /// <summary>
    /// To verify a hashed password is correct or not
    /// </summary>
    /// <param name="hashedPassword">Hashed password string</param>
    /// <param name="password">Raw password</param>
    /// <returns>True if correct, False if not</returns>
    public bool VerifyHashPassword(string? hashedPassword, string password);
}