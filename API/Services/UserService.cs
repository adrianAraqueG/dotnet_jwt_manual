using Domain.Interfaces;
using System.Linq;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using API.Helpers;

namespace API.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenService _tokenService;
    public UserService(IUnitOfWork unitOfWork){
        _unitOfWork = unitOfWork;
        _tokenService = new TokenService("secretKey");
    }

    public async Task<object> RegisterAsync(RegUserDto model)
    {   
        Console.WriteLine("Datos a guardar:");
        Console.WriteLine(model.Username);
        Console.WriteLine(model.Password);
        Console.WriteLine(model.Email);

        //Segunda verificación de datos desde cliente

        //Verificar que el usuario no exista (con user y mail)

        var user = new User(){
            Username = model.Username,
            Password = PasswordHasher.HashPassword(model.Password),
            Email = model.Email,
            IdRole = 1
        };

        try{
            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveAsync();
            return new{
                message = "¡Usuario Registrado Correctamente!"
            };
        }catch (Exception err){
            Console.WriteLine(err);
            return new{
                message = "No pudimos registrar el usuario, inténtelo más tarde."
            };
        }
    }
    
    public async Task<DataUserDto> LoginAsync(LogUserDto model)
    {
        var response = new DataUserDto();

        var existingUser = _unitOfWork.Users.Find(u => u.Username.ToLower() == model.Username.ToLower()).FirstOrDefault();
        if(existingUser == null)
        {
            response.Message = "Ese usuario ta trucho";
            response.IsAuthenticated = false;
            return response;
        }

        bool isPasswordValid = existingUser.Password == PasswordHasher.HashPassword(model.Password);
        if (!isPasswordValid)
        {
            response.Message = "Contraseña Incorrecta";
            response.IsAuthenticated = false;
            return response;
        }

        // Usuario autenticado correctamente
        DateTime expiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(5));
        var token = _tokenService.GenerateTokenSignature(new PayloadTokenDto{Username = model.Username, Email = existingUser.Email}, expiration);
        var refreshToken = await GenerateAndStoreRefreshToken(existingUser);

        response.Message = "Autenticado correctamente";
        response.IsAuthenticated = true;
        response.Username = model.Username;
        response.Email = existingUser.Email;
        response.Token = token;
        response.RefreshToken = refreshToken;
        response.Expiration = expiration;

        return response;
    }

    public async Task<bool> Logout(string token)
    {
        var blacklistEntity = new Blacklist
        {
            Token = token
        };

        _unitOfWork.Blacklist.Add(blacklistEntity);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<DataUserDto> RefreshUserToken(string refreshToken)
    {
        var storedToken = _unitOfWork.RefreshTokens.Find(t => t.Token == refreshToken).FirstOrDefault();

        if (storedToken == null || storedToken.Expiration < DateTime.UtcNow)
        {
            return null;
        }

        var user = _unitOfWork.Users.Find(u => u.Id == storedToken.IdUser).FirstOrDefault();

        DateTime newTokenExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(5));

        // Genera un nuevo token principal y refreshToken
        var newToken = _tokenService.GenerateTokenSignature(new PayloadTokenDto { Username = user.Username, Email = user.Email }, newTokenExpiration);
        var newRefreshToken = await GenerateAndStoreRefreshToken(user);

        return new DataUserDto
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expiration = newTokenExpiration
        };
    }
    
    public async Task<string> GenerateAndStoreRefreshToken(User user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(10),
            IdUser = user.Id
        };
        
        _unitOfWork.RefreshTokens.Add(refreshTokenEntity);
        await _unitOfWork.SaveAsync();

        return refreshToken;
    }

    public async Task<object> ValidateTokenAsync(DataUserDto model)
    {
        var response = new DataUserDto
        {
            Username = model.Username,
            Email = model.Email
        };

        if (string.IsNullOrWhiteSpace(model.Token) || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email))
        {
            response.Message = "Datos incompletos.";
            response.IsAuthenticated = false;
            return response;
        }

        // Verificar si el token está en la lista negra
        var isBlacklisted = await _unitOfWork.Blacklist.Any(b => b.Token == model.Token);
        if (isBlacklisted)
        {
            Console.WriteLine(isBlacklisted+"holaaa");
            response.Message = "Token inválido.";
            response.IsAuthenticated = false;
            return response;
        }

        bool isValidToken = _tokenService.VerifyTokenSignature(model.Token, new PayloadTokenDto { Username = model.Username, Email = model.Email }, model.Expiration);
        
        if (isValidToken)
        {
            response.Message = "Token válido.";
            response.IsAuthenticated = true;
        }
        else
        {
            response.Message = "Token inválido o expirado.";
            response.IsAuthenticated = false;
        }

        return response;
    }



}