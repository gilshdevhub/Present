using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class SeedIdentityContext
{
    public static async Task SeedRolesAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {


        if (roleManager.Roles.Count() == 0 && userManager.Users.Count() == 0)
        {
            var role = new AppRole
            {
                Name = "SuperAdmin"

            };

            var isSucceededRole = await roleManager.CreateAsync(role);

            var user = new AppUser
            {
                DisplayName = "Bob",
                UserName = "bob@test.com",
                Email = "bob@test.com",
                PhoneNumber = "01234"
            };

            var isSucceededUser = await userManager.CreateAsync(user, "Pa$$w0rd");

            if (isSucceededUser.Succeeded && isSucceededRole.Succeeded)
            {
                _ = await userManager.AddToRoleAsync(user, role.Name);
            }

            role = new AppRole
            {
                Name = "BackOffice"

            };

            _ = await roleManager.CreateAsync(role);

            role = new AppRole
            {
                Name = "Steward"

            };

            _ = await roleManager.CreateAsync(role);


        }
    }
}

