using ContractManagment_Al_Doori_.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ContractManagment_Al_Doori_.Models.Entities.Identity;
using ContractManagment_Al_Doori_.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ContractManagment_Al_Doori_.Models.Infrastructure.Database
{
    public class ContractDbContext : IdentityDbContext<User, Role, int>
    {

        #region DbSets
        public DbSet<Advisor>? Advisors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        public DbSet<AdvisorContract> AdvisorContracts { get; set; }
        #endregion

        #region Constructor
        public ContractDbContext(DbContextOptions options) : base(options)
        {
        }
        #endregion

        #region OnModelCreating 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Advisor>().HasData(DatabaseInit.Advisors);
            modelBuilder.Entity<Client>().HasData(DatabaseInit.Clients);
            modelBuilder.Entity<Contract>().HasData(DatabaseInit.Contract);
            modelBuilder.Entity<AdvisorContract>().HasData(DatabaseInit.AdvisorContracts);

            //Generation Of Roles 
            modelBuilder.Entity<Role>().HasData(DatabaseInit.roles);
      

        }
        #endregion


    }
}
