using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Text.Json.Serialization;

namespace LimsAuthenticationService.Models;

[Table("Utilisateur")]
public class Utilisateur
{
    [Key]
    [Column("id_user")]
    public int IdUtilisateur { get; set; }
    [Column("identifiant")]

    public string Identifiant { get; set; }
    [Column("password")]
    public string Password { get; set; }  // Hashed password
    
    public ICollection<UserRole> UserRoles { get; set; } // Many-to-many relationship with roles

}
