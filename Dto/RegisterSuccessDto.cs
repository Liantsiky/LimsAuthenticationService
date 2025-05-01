using LimsAuthenticationService.Models;

namespace LimsAuthenticationService.Dto;

public class RegisterSuccessDto
{
    public string Identifiant { get; set; }
    public List<Role> Roles { get; set; } // Optional: Only if assigning roles during registration
    public Departement Departement { get; set; }
}
