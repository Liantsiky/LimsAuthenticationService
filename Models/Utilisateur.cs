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

    public required string Identifiant { get; set; }
    [Column("password")]
    public required string Password { get; set; }  // Hashed password
    
    public ICollection<UserRole>? UserRoles { get; set; } // Many-to-many relationship with roles

    [Column("id_departement")]
    public int IdDepartement { get; set;}
    
    [ForeignKey("IdDepartement")]
    public Departement Departement { get; set;}

}
