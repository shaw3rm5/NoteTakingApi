using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Infrastructure.Database;

namespace NoteTakingApi.Infrastructure;

public static class InfrastructureDependencies
{
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                .UseSnakeCaseNamingConvention());
    }
}