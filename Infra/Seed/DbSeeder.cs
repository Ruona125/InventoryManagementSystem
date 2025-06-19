using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Staff" },
                new Role { Name = "Manager" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync())
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@inventory.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), // <-- Change here!
                RoleId = adminRole.Id
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}