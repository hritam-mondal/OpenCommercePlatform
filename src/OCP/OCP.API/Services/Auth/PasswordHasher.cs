using System.Security.Cryptography;

namespace OCP.API.Services.Auth;

public static class PasswordHasher
{
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 100_000;

    // Returns (hashBytes, saltBytes)
    private static (byte[] Hash, byte[] Salt) HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        }
        var salt = RandomNumberGenerator.GetBytes(SaltSize); 
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);
        return (hash, salt);
    }

    // Base64 convenience: return strings safe for text columns
    public static (string HashBase64, string SaltBase64) HashPasswordToBase64(string password)
    {
        var (hash, salt) = HashPassword(password);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    // Verify from raw bytes
    private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            storedSalt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }

    // Verify when DB stored base64 strings
    public static bool VerifyPasswordFromBase64(string password, string storedHashBase64, string storedSaltBase64)
    {
        if (string.IsNullOrEmpty(storedHashBase64) || string.IsNullOrEmpty(storedSaltBase64))
        {
            return false;
        }

        var storedHash = Convert.FromBase64String(storedHashBase64);
        var storedSalt = Convert.FromBase64String(storedSaltBase64);
        return VerifyPassword(password, storedHash, storedSalt);
    }
}
