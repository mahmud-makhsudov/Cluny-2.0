using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Shared.Constants;

namespace ClunyApi.Data
{
    public static class SeedIdentityData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider appServices, IConfiguration config)
        {
            using var scope = appServices.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(SeedIdentityData).FullName!);

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                var roles = new[] { AuthConstants.RoleAdmin, AuthConstants.RoleCustomer };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                await CreateOrEnsureUserAsync(userManager, config["AdminUser:Email"], config["AdminUser:Password"], AuthConstants.RoleAdmin, logger);
                await CreateOrEnsureUserAsync(userManager, config["CustomerUser:Email"], config["CustomerUser:Password"], AuthConstants.RoleCustomer, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding roles and users.");
                throw;
            }
        }

        private static async Task CreateOrEnsureUserAsync(
            UserManager<IdentityUser> userManager,
            string? email,
            string? password,
            string role,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.LogInformation("{Role} configuration not found; skipping {Role} seeding.", role, role);
                return;
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (!addRoleResult.Succeeded)
                    {
                        logger.LogWarning("{Role} user created but adding role failed: {Errors}", role, string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                    else
                    {
                        logger.LogInformation("{Role} user created: {Email}", role, email);
                    }
                }
                else
                {
                    logger.LogWarning("{Role} user creation failed: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (addRoleResult.Succeeded)
                    {
                        logger.LogInformation("Existing user added to {Role} role: {Email}", role, email);
                    }
                    else
                    {
                        logger.LogWarning("Failed to add existing user to {Role} role: {Errors}", role, string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogInformation("{Role} user already exists and is in role: {Email}", role, email);
                }
            }
        }
    }
}