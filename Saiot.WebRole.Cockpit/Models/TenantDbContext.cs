using System;
using System.Data.Entity;

namespace Saiot.WebRole.Cockpit.Models
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext()
            : base("Server=tcp:h24f8jdq8f.database.windows.net,1433;Database=saiotCockpitTenants;User ID=saiotadmin@h24f8jdq8f;Password=HSR-22550044;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;")
           // : base("DefaultConnection")
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<IssuingAuthorityKey> IssuingAuthorityKeys { get; set; }

        public DbSet<SignupToken> SignupTokens { get; set; }
    }
}