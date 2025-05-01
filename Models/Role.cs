using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace LimsAuthenticationService.Models;
[Table("Role")]
public class Role
{
    [Key]
    [Column("id_role")]
    public int IdRole { get; set; }
    [Column("designation")]

    public required string Designation { get; set; }
    [JsonIgnore]
    public ICollection<UserRole>? UserRoles { get; set; } // Many-to-many relationship with roles

}
