using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MjondalenInstallasjon.Data.Data;

namespace MjondalenInstallasjon.Data
{
    public static class DataConfiguration
    {
        public static void ConfigureData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
        }
    }
}