using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestrator.Presistance
{
    public class OrchSagaDbContext : DbContext
    {
        public DbSet<OrderStateData> OrderStateData { get; set; }
        public OrchSagaDbContext()
        {
        }

        public OrchSagaDbContext(DbContextOptions<OrchSagaDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Database=OrchestrationDB;Persist Security Info=True;User ID=sa;Password=sa@123;TrustServerCertificate=True;");
        }
    }
}
