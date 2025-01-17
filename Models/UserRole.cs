using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsAuthenticationService.Models;
[Table("UserRole")]
public class UserRole
{
    [Key]
    [Column("id_user")]
    public int IdUtilisateur { get; set; }
    public Utilisateur User { get; set; }

    [Column("id_role")]
    public int IdRole { get; set; }
    public Role Role { get; set; }

}
