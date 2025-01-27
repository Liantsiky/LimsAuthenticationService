using Microsoft.EntityFrameworkCore;

using LimsAuthenticationService.Data;
using LimsAuthenticationService.Models;
using LimsAuthenticationService.Utils;
using LimsAuthenticationService.Dto;


namespace LimsAuthenticationService.Services;
public class AuthService : IAuthService
{
    private readonly AuthDbContext _context;

    public AuthService(AuthDbContext context)
    {
        _context = context;
    }

    // Register a new user
    public async Task RegisterUserAsync(RegisterDto userDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        // Hash the password
        try
        {
            string hashedPassword = AuthUtils.HashPassword(userDto.Password);
            // Create a new user
            Utilisateur user = new Utilisateur
            {
                Identifiant = userDto.Identifiant,
                Password = hashedPassword
            };

            // Add user to the database
            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();
    

            // Handle roles
            foreach (string roleName in userDto.Roles)
            {
                // Check if the role exists
                Role role = await _context.Roles.FirstOrDefaultAsync(r => r.Designation == roleName);
                if (role == null)
                {
                    // Create the role if it doesn't exist
                    role = new Role { Designation = roleName };
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }

                // Assign the role to the user
                _context.UserRoles.Add(new UserRole { IdUtilisateur = user.IdUtilisateur, IdRole = role.IdRole });
            }

            // Save all changes
            await _context.SaveChangesAsync();
            
            //commit transaction
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            
            await transaction.RollbackAsync();
            throw new Exception("An error occurred during user registration. Changes were rolled back.", ex);
        }
    }

    // Login and return a simple JWT token
    public async Task<string> LoginUserAsync(LoginDto loginDto)
    {
        string result = null;
        var user = await _context.Utilisateurs
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Identifiant == loginDto.Identifiant);

        if (user == null || !AuthUtils.VerifyPassword(user.Password, loginDto.Password))
        {
            result = "Invalid credentials"; // Invalid credentials
            return result;
        }
        // Retrieve roles
        List<String> roles = user.UserRoles.Select(ur => ur.Role.Designation).ToList();
        
        result = AuthUtils.GenerateToken(user,roles);
        return result;
    }
}
