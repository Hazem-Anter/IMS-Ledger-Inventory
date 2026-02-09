
using IMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Persistence
{
    // AuthDbContext is a specialized DbContext for handling authentication and authorization using ASP.NET Core Identity.
    // It inherits from IdentityDbContext,
    // which provides all the necessary DbSet properties and configurations for managing users, roles, and their relationships.
    
    // The generic parameters specify that we are using ApplicationUser as our user entity,
    // IdentityRole<int> for roles with integer keys, and int as the type for primary keys.
    
    public class AuthDbContext 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        // Constructor that accepts DbContextOptions and passes them to the base class constructor
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }
    }
}
