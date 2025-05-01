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
                Password = hashedPassword,
                IdDepartement = userDto.IdDepartement
            };

            // Add user to the database
            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();
    

            // Handle roles
            foreach (int idRoles in userDto.Roles)
            { 
                _context.UserRoles.Add(new UserRole { IdUtilisateur = user.IdUtilisateur, IdRole = idRoles });
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

    public RegisterSuccessDto FromUtilisateurToRegisterSuccess(Utilisateur user)
    {
        List<Role> roles = new List<Role>();
        foreach (UserRole userRole in user.UserRoles)
        {
            roles.Add(userRole.Role);
        }
        RegisterSuccessDto registerSuccess = new RegisterSuccessDto
        {
            Identifiant = user.Identifiant,
            Roles = roles,
            Departement = user.Departement
        };
        return registerSuccess;
    }
}
