using System.Text.Json;
using LimsAuthenticationService.Data;
using LimsAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace LimsAuthenticationService.Services;

public class RoleService : IRoleService
{
    private readonly AuthDbContext _dbContext;
    public RoleService(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CountRoles()
    {
        int result = await _dbContext.Roles.CountAsync();
        return result;
    }
        
    public async Task<List<Role>> GetRoles()
    {
        List<Role> results = await _dbContext.Roles
        .ToListAsync();
        return results;
    }

    public async Task<List<Role>> GetRolesFrom(int skiped, int size)
    {        
        List<Role> results = await _dbContext.Roles
            .OrderByDescending(e => e.IdRole).Skip(skiped).Take(size)
            .ToListAsync();

        return results;
    }

     public async Task<Role> GetRole(int id)
    {
        Role result = await _dbContext.Roles
            .Where(p => p.IdRole == id)
            .FirstAsync();
        return result;
    }

    public async Task<Role> CreateRole(Role poste)
    {
        _dbContext.Roles.Add(poste);
        await _dbContext.SaveChangesAsync();
        Role result = await _dbContext.Roles.OrderBy(p => p.IdRole).LastAsync();

        return result;
    }

    public async Task<bool> DeleteRole(int id)
    {
        bool isDeleted = false;
        Role? role = await _dbContext.Roles.FirstOrDefaultAsync(p => p.IdRole== id);
        if(role == null)
        {
            throw new ArgumentException("Le role que vous souhaitez supprimer n'est pas dans la base de données");
        }
        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();
        isDeleted = true;
        return isDeleted;
    }

    public async Task<Role> EditRole(Role poste)
    {
        int id = poste.IdRole;
        _dbContext.Roles.Update(poste);
        await _dbContext.SaveChangesAsync();

        Role result = await this.GetRole(id);
        return result;
    }
}