using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SignalRProject.Models;

[Index(nameof(UserName), IsUnique = true)]
public class ApplicationUser : IdentityUser
{
    public override string UserName { get; set; }
}