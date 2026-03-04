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

        if (context.Logins.Any())
            return;

        var adminRole = context.UsersRoles.FirstOrDefault(r => r.RoleLabel == "Administrateur") ??
                        throw new Exception("Administrator role does not exist");
        var userRole = context.UsersRoles.FirstOrDefault(r => r.RoleLabel == "Utilisateur") ??
                       throw new Exception("user role does not exist");

        var userEmail = Environment.GetEnvironmentVariable("DEFAULT_USER_ACCOUNT_EMAIL");
        var userPassword = Environment.GetEnvironmentVariable("DEFAULT_USER_ACCOUNT_PASSWORD");

        var adminEmail = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_ACCOUNT_EMAIL");
        var adminPassword = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_ACCOUNT_PASSWORD");

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            Console.WriteLine(
                "Warning: No administrators exist and DEFAULT_ADMIN_ACCOUNT_EMAIL/DEFAULT_ADMIN_ACCOUNT_PASSWORD are not set. Skipping admin seeding.");
            return;
        }

        if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPassword))
        {
            Console.WriteLine(
                "Warning: No administrators exist and DEFAULT_USER_ACCOUNT_EMAIL/DEFAULT_USER_ACCOUNT_PASSWORD are not set. Skipping user seeding.");
            return;
        }


 

        var userInfos = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = "Utilisateur",
            LastName = "Défaut",
            UserName = "default_user",
            IsActive = true,
            CreationTime = DateTime.UtcNow,
            UserRoleId = userRole.Id,
        };

        var userLogin = new Login()
        {
            Id = Guid.NewGuid(),
            Email = userEmail,
            PasswordHash = authService.HashPassword(userPassword),
            CreationTime = DateTime.UtcNow,
            UserId = userInfos.Id
        };
        
        var userPasswordInfos = new PasswordInfo()
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTime.UtcNow,
            User = userInfos,
            UserId = userInfos.Id,
        };



        var adminInfos = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = "Administrateur",
            LastName = "Défaut",
            UserName = "default_administrator",
            IsActive = true,
            CreationTime = DateTime.UtcNow,
            UserRoleId = userRole.Id,
        };
        
        var adminLogin = new Login()
        {
            Id= Guid.NewGuid(),
            Email = adminEmail,
            PasswordHash = authService.HashPassword(adminPassword),
            CreationTime = DateTime.UtcNow,
            UserId = adminInfos.Id,
        };

        var adminPasswordInfos = new PasswordInfo()
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTime.UtcNow,
            User = adminInfos,
            UserId = adminInfos.Id,
        };

        var devEnv = Environment.GetEnvironmentVariable("DEPLOYMENT_ENVIRONMENT");

        if (devEnv == "DEV")
        {
            context.Logins.Add(userLogin);
            context.Users.Add(userInfos);
            context.PasswordsInfos.Add(userPasswordInfos);
            Console.WriteLine($"Added default user for development environment: {userLogin.Email}");
        }
        
   
        context.Logins.Add(adminLogin);
        context.Users.Add(adminInfos);
        context.PasswordsInfos.Add(adminPasswordInfos);


        context.SaveChanges();

        Console.WriteLine($"Default user created with email: {userEmail}");
        Console.WriteLine($"Default administrator created with email: {adminEmail}");
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