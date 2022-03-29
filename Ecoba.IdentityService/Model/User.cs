using System.ComponentModel.DataAnnotations;

namespace Ecoba.IdentityService.Model;

public class User
{
    [Key]
    public string Username { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSatl { get; set; }
    public bool IsResetPassword { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool Editable { get; set; }
    public bool IsActive { get; set; }

}

