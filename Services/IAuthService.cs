using LimsAuthenticationService.Models;
using LimsAuthenticationService.Dto;


namespace LimsAuthenticationService.Services;
public interface IAuthService 
{
    
    Task RegisterUserAsync(RegisterDto userDto);
    Task<string> LoginUserAsync(LoginDto loginDto);
    RegisterSuccessDto FromUtilisateurToRegisterSuccess(Utilisateur user);
}