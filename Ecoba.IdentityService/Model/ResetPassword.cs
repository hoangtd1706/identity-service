using System.ComponentModel.DataAnnotations;

namespace Ecoba.IdentityService.Model;

public class ResetPassword
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ReNewPassword { get; set; }

}

