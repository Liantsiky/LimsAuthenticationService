namespace LimsAuthenticationService.Dto;

public class RegisterDto
{
    public string Identifiant { get; set; }
    public string Password { get; set; }
    public List<int> Roles { get; set; } // Optional: Only if assigning roles during registration
    public int IdDepartement { get; set; }
}
