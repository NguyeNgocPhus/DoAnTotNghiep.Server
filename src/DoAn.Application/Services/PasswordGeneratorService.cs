using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using DoAn.Application.Abstractions;

namespace DoAn.Application.Services;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    public string HashPassword(string password)
    {
        byte[] salt;
        byte[] buffer2;
        if (password == null)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("Password is null");
        }

        using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }

        byte[] dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        return Convert.ToBase64String(dst);
    }

    public bool VerifyHashPassword(string? hashedPassword, string password)
    {
        byte[] buffer4;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (hashedPassword == null)
        {
            return false;
        }

        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        byte[] src = Convert.FromBase64String(hashedPassword);
        if ((src.Length != 0x31) || (src[0] != 0))
        {
            return false;
        }

        byte[] dst = new byte[0x10];
        Buffer.BlockCopy(src, 1, dst, 0, 0x10);
        byte[] buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
        using (var bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
        {
            buffer4 = bytes.GetBytes(0x20);
        }

        return ByteArraysEqual(buffer3, buffer4);
    }

    [ExcludeFromCodeCoverage]
    private bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2) return true;
        // ReSharper disable ConditionIsAlwaysTrueOrFalse
        if (b1 == null || b2 == null) return false;
        if (b1.Length != b2.Length) return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i]) return false;
        }

        return true;
    }
}