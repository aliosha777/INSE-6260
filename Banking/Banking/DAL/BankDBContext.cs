using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Banking.Models;

namespace Banking.DAL
{
    public class BankDBContext : DbContext
    {
        public BankDBContext()
            : base("BankingDatabase")
        { 
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Accounts)
                .WithMany(a => a.Owners)
                .Map(
                    m =>
                    {
                        m.MapLeftKey("CustomerId");
                        m.MapRightKey("AccountId");
                        m.ToTable("CustomerAccounts");
                    });
        }
    }
}