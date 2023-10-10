using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
public class BlacklistRepository : GenericRepository<Blacklist>, IBlacklistRepository
{
    private readonly APIContext _context;
    public BlacklistRepository(APIContext context): base(context){
        _context = context;
    }
}