using System.Security.Cryptography;

namespace Extension;

public static class StringExtensions
{
    public static string HashPassword(this string password, out string salt)
    {
        salt = GenerateSalt(16);

        var hash = HashPasswordWithSalt(password, salt);
        return hash;
    }

    public static bool VerifyPassword(this string password, string salt, string hash)
    {
        var hashToVerify = HashPasswordWithSalt(password, salt);
        return SlowEquals(Convert.FromBase64String(hash), Convert.FromBase64String(hashToVerify));
    }

    private static string HashPasswordWithSalt(string password, string salt, int iterations = 10000,
        int hashByteSize = 32)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations,
                   HashAlgorithmName.SHA256))
        {
            var hash = pbkdf2.GetBytes(hashByteSize);
            return Convert.ToBase64String(hash);
        }
    }

    private static string GenerateSalt(int size)
    {
        var saltBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    private static bool SlowEquals(byte[] a, byte[] b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
            diff |= (uint)(a[i] ^ b[i]);
        return diff == 0;
    }
}
