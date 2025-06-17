using Serilog;

namespace NoteTakingApi.Common.Extensions;

public static class SerilogConfiguration
{
    public static void AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        services.AddSerilog(Log.Logger);
    }
}