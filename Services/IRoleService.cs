
using LimsAuthenticationService.Models;

namespace LimsAuthenticationService.Services;
public interface IRoleService
{
    Task<int> CountRoles();
    Task<List<Role>> GetRoles();
    Task<List<Role>> GetRolesFrom(int skiped, int size);
    Task<Role> GetRole(int id);
    Task<Role> CreateRole(Role role);
    // Task<bool> DeleteRole(int id, Role role);
    Task<bool> DeleteRole(int id);
    Task<Role> EditRole(Role role);
}