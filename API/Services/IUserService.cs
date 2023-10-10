using API.Dtos;
using Domain.Entities;

namespace API.Services;

public interface IUserService
{
    Task<object> RegisterAsync(RegUserDto model);
    Task<DataUserDto> LoginAsync(LogUserDto model);
    Task<bool> Logout(string token);
    Task<DataUserDto> RefreshUserToken(string refreshToken);
    Task<string> GenerateAndStoreRefreshToken(User user);
    Task<object> ValidateTokenAsync(DataUserDto model);
}