
using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Middlewares;

namespace Ressource_API.Common.Extensions;


public static class AppBuildingExtensions
{
    public static void BuildSolution(this WebApplicationBuilder builder)
    {
        builder.InjectDependencies();
        var app = builder.Build();
        
        app.ApplyPendingMigrations();
        
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