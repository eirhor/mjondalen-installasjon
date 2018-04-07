using Microsoft.EntityFrameworkCore;

namespace MjondalenInstallasjon.Data.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
        
        
    }
}