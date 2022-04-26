namespace Ecoba.IdentityService.Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class UserRole
{
    [Key]
    public int Id { get; set; }
    public string Role { get; set; }
    public bool Editable { get; set; }
    [ForeignKey(nameof(User))]
    public string Username { get; set; }
    public User User { get; set; }
}
