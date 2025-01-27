using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsAuthenticationService.Models;
[Table("Role")]
public class Role
{
    [Key]
    [Column("id_role")]
    public int IdRole { get; set; }
    [Column("designation")]

    public string Designation { get; set; }
    public ICollection<UserRole>? UserRoles { get; set; } // Many-to-many relationship with roles

}
