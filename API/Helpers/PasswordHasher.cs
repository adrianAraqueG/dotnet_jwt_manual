using System.Security.Cryptography;
using System.Text;

namespace API.Helpers;

public class PasswordHasher{
    public static string HashPassword(string password){
        using SHA256 sha256 = SHA256.Create();
        // Convierte la contraseña en un arreglo de bytes
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Calcula el hash SHA-256 de la contraseña
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);

        // Convierte el hash a una cadena hexadecimal
        StringBuilder hashBuilder = new();
        foreach (byte b in hashBytes)
        {
            hashBuilder.Append(b.ToString("x2"));
        }

        return hashBuilder.ToString();
    }
}

