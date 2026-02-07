using Microsoft.AspNetCore.Identity;

namespace AnimalFarm.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; }
}
