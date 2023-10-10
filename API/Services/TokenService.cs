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
}

