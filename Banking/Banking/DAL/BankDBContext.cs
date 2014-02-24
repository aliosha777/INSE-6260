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

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<BankAccountModel> Accounts { get; set; }

        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // TODO: Verify this is not redundant
            modelBuilder
                .Entity<CustomerModel>()
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