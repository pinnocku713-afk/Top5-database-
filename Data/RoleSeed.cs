using Microsoft.AspNetCore.Identity;
namespace Top5.Data;
    public class RoleSeed
    {
        public static async Task Run(IServiceProvider services, string? admin_email, string? staff_email)
        {
            RoleManager<IdentityRole> role_manager = services.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> user_manager = services.GetRequiredService<UserManager<IdentityUser>>();


        string[] role_names = { "Admin", "User", "Staff" };
        foreach (string role_name in role_names)
        {
            bool role_exists = await role_manager.RoleExistsAsync(role_name);
            if (!role_exists)
            {
                await role_manager.CreateAsync(new IdentityRole(role_name));
            }
        }

        if (!string.IsNullOrWhiteSpace(staff_email))
        {
            IdentityUser? staff_user= await user_manager.FindByEmailAsync(staff_email);
            if (staff_user == null)
            {
                return;
            }
            bool is_staff = await user_manager.IsInRoleAsync(staff_user, "Staff");
            if (!is_staff)
            {
                await user_manager.AddToRoleAsync(staff_user, "Staff");
            }
        }

        if (string.IsNullOrWhiteSpace(admin_email))
        {
            return;
        }

        IdentityUser? admin_user = await user_manager.FindByEmailAsync(admin_email);

        if (admin_user == null)
        {
            return;
        }


        bool is_admin = await user_manager.IsInRoleAsync(admin_user, "admin");
        if (!is_admin)
        {
            await user_manager.AddToRoleAsync(admin_user, "Admin");
        }

    }

}

