using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly APIContext _context;
    public RoleRepository(APIContext context): base(context){
        _context = context;
    }
}