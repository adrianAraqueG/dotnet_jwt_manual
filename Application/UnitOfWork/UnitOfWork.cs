using Application.Repository;
using Domain.Interfaces;
using Persistence;

namespace Application.UnitOfWork;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly APIContext _context;
    public IUserRepository _users;
    public IVehicleRepository _vehicles;
    public IRoleRepository _roles;
    public IRefreshTokenRepository _refreshTokens;
    public IBlacklistRepository _blacklist;
    public UnitOfWork(APIContext context){
        _context = context;
    }

    
    public IUserRepository Users{
        get{
            if(_users == null){
                return _users = new UserRepository(_context);
            }
            return _users;
        }
    }

    public IVehicleRepository Vehicles{
        get{
            if(_vehicles == null){
                return _vehicles = new VehicleRepository(_context);
            }
            return _vehicles;
        }
    }

    public IRoleRepository Roles{
        get{
            if(_roles == null){
                return _roles = new RoleRepository(_context);
            }
            return _roles;
        }
    }
    public IRefreshTokenRepository RefreshTokens{
        get{
            if(_refreshTokens == null){
                return _refreshTokens = new RefreshTokenRepository(_context);
            }
            return _refreshTokens;
        }
    }
    public IBlacklistRepository Blacklist{
        get{
            if(_blacklist == null){
                return _blacklist = new BlacklistRepository(_context);
            }
            return _blacklist;
        }
    }


    /*
    * DEFAULT
    */
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}