using System;
using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using Newtonsoft.Json;

namespace API.Services;

public class TokenService
{
    private readonly string _secretKey;

    public TokenService(string secretKey)
    {
        _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
    }

    public string GenerateTokenSignature(PayloadTokenDto clientInfo, DateTime expiration)
    {
        var tokenData = new
        {
            exp = expiration,
            data = clientInfo
        };

        string payload = JsonConvert.SerializeObject(tokenData);
        return ComputeSignature(payload);
    }

    public bool VerifyTokenSignature(string signature, PayloadTokenDto clientInfo, DateTime expiration)
    {
        // Primero, verifica si el token ha expirado
        Console.WriteLine($"Actual: {DateTime.UtcNow}\nVence: {expiration}");
        if (expiration < DateTime.UtcNow)
        {
            return false; // El token ha caducado
        }

        var tokenData = new
        {
            exp = expiration, // Usamos la fecha de expiración proporcionada
            data = clientInfo
        };

        string expectedPayload = JsonConvert.SerializeObject(tokenData);
        string computedSignature = ComputeSignature(expectedPayload);

        return signature == computedSignature;
    }

    public string GenerateRefreshToken()
    {
        Console.WriteLine(Guid.NewGuid().ToString());
        return Guid.NewGuid().ToString();
    }

    private string ComputeSignature(string data)
    {
        using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] hashBytes = hmac.ComputeHash(dataBytes);
        return Convert.ToBase64String(hashBytes);
    }
}

