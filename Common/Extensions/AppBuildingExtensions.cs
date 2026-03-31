using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Middlewares;
using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.UserRoles.Services;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;
using Simply.Auth.Core.Abstractions;

namespace Ressource_API.Common.Extensions;

public static class AppBuildingExtensions
{
    public static void BuildSolution(this WebApplicationBuilder builder)
    {
        builder.InjectDependencies();
        var app = builder.Build();

        app.ApplyPendingMigrations();

        app.SeedDatabase();

        // CORS
        app.AddCorsRules();

        app.UseHttpsRedirection();

        app.AddMiddlewares();

        app.AddOpenApiMapping();

        app.MapControllers().WithGroupName("api");

        app.Run();
    }

    private static void AddMiddlewares(this WebApplication app)
    {
        app.UseAuthorization();
        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();
    }

    private static void SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var authService = scope.ServiceProvider.GetRequiredService<ISimplyAuthService>();

        var adminRole = context.UsersRoles.FirstOrDefault(r => r.RoleLabel == "Administrateur")
                        ?? throw new Exception("Administrator role does not exist");
        var userRole = context.UsersRoles.FirstOrDefault(r => r.RoleLabel == "Utilisateur")
                       ?? throw new Exception("user role does not exist");

        var userEmail = Environment.GetEnvironmentVariable("DEFAULT_USER_ACCOUNT_EMAIL");
        var userPassword = Environment.GetEnvironmentVariable("DEFAULT_USER_ACCOUNT_PASSWORD");
        var adminEmail = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_ACCOUNT_EMAIL");
        var adminPassword = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_ACCOUNT_PASSWORD");

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword) ||
            string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPassword))
        {
            Console.WriteLine("Default credentials not set. Skipping seeding.");
            return;
        }

        var existingUsers = context.Users
            .Include(u => u.Logins)
            .Include(u => u.PasswordsInfos)
            .Where(u => u.UserName == "default_user" || u.UserName == "default_administrator")
            .ToList();

        var existingDefaultUser = existingUsers.FirstOrDefault(u => u.UserName == "default_user");
        var existingAdminUser = existingUsers.FirstOrDefault(u => u.UserName == "default_administrator");

        // default user
        if (existingDefaultUser != null)
        {
            existingDefaultUser.UserRoleId = userRole.Id;
            existingDefaultUser.IsActive = true;
            existingDefaultUser.UpdateTime = DateTime.UtcNow;

            var existingUserLogin = existingDefaultUser.Logins.FirstOrDefault();
            if (existingUserLogin != null)
            {
                existingUserLogin.Email = userEmail;
                existingUserLogin.PasswordHash = authService.HashPassword(userPassword);
                existingUserLogin.UpdateTime = DateTime.UtcNow;
            }
            else
            {
                context.Logins.Add(new Login
                {
                    Id = Guid.NewGuid(),
                    Email = userEmail,
                    PasswordHash = authService.HashPassword(userPassword),
                    CreationTime = DateTime.UtcNow,
                    UserId = existingDefaultUser.Id
                });
            }

            if (!existingDefaultUser.PasswordsInfos.Any())
            {
                context.PasswordsInfos.Add(new PasswordInfo
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.UtcNow,
                    UserId = existingDefaultUser.Id
                });
            }
        }
        else
        {
            var userInfos = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Utilisateur",
                LastName = "Défaut",
                UserName = "default_user",
                IsActive = true,
                CreationTime = DateTime.UtcNow,
                UserRoleId = userRole.Id
            };
            context.Users.Add(userInfos);
            context.Logins.Add(new Login
            {
                Id = Guid.NewGuid(),
                Email = userEmail,
                PasswordHash = authService.HashPassword(userPassword),
                CreationTime = DateTime.UtcNow,
                UserId = userInfos.Id
            });
            context.PasswordsInfos.Add(new PasswordInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                UserId = userInfos.Id
            });
        }

        // default admin
        if (existingAdminUser != null)
        {
            existingAdminUser.UserRoleId = adminRole.Id;
            existingAdminUser.IsActive = true;
            existingAdminUser.UpdateTime = DateTime.UtcNow;

            var existingAdminLogin = existingAdminUser.Logins.FirstOrDefault();
            if (existingAdminLogin != null)
            {
                existingAdminLogin.Email = adminEmail;
                existingAdminLogin.PasswordHash = authService.HashPassword(adminPassword);
                existingAdminLogin.UpdateTime = DateTime.UtcNow;
            }
            else
            {
                context.Logins.Add(new Login
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    PasswordHash = authService.HashPassword(adminPassword),
                    CreationTime = DateTime.UtcNow,
                    UserId = existingAdminUser.Id
                });
            }

            if (!existingAdminUser.PasswordsInfos.Any())
            {
                context.PasswordsInfos.Add(new PasswordInfo
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.UtcNow,
                    UserId = existingAdminUser.Id
                });
            }
        }
        else
        {
            var adminInfos = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Administrateur",
                LastName = "Défaut",
                UserName = "default_administrator",
                IsActive = true,
                CreationTime = DateTime.UtcNow,
                UserRoleId = adminRole.Id
            };
            context.Users.Add(adminInfos);
            context.Logins.Add(new Login
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                PasswordHash = authService.HashPassword(adminPassword),
                CreationTime = DateTime.UtcNow,
                UserId = adminInfos.Id
            });
            context.PasswordsInfos.Add(new PasswordInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                UserId = adminInfos.Id
            });
        }

        context.SaveChanges();

        Console.WriteLine($"Default user recreated with email: {userEmail}");
        Console.WriteLine($"Default admin recreated with email: {adminEmail}");
    }

    private static void AddCorsRules(this WebApplication app)
    {
        app.UseCors("AllowClientApp");
        app.UseCors("AllowBacklog");
    }

    private static void AddOpenApiMapping(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    private static void ApplyPendingMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            dbContext.Database.Migrate();

            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
    }
}