using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Auramode.Services;

public class AuthService
{
    public static (string hash, string salt) HashPassword(string password)
    {
        byte[] saltBytes = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return (hashed, Convert.ToBase64String(saltBytes));
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        try
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(hashed),
                Convert.FromBase64String(storedHash));
        }
        catch
        {
            return false;
        }
    }
}

