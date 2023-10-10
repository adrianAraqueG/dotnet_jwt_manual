using Domain.Entities;

namespace Domain.Interfaces;
public interface IUnitOfWork
{
    IUserRepository Users {get; }
    IVehicleRepository Vehicles {get; }
    IRoleRepository Roles {get; }
    IBlacklistRepository Blacklist {get; }
    IRefreshTokenRepository RefreshTokens {get; }
    Task<int> SaveAsync();
}