using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Banking.Application.Models;

namespace Banking.Application.DAL
{
    public class BankDBContext : DbContext
    {
        public BankDBContext()
            : base("BankingDatabase")
        { 
        }

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<AddressModel> Addresses { get; set; }

        public DbSet<BankAccountModel> Accounts { get; set; }

        public DbSet<TransactionModel> Transactions { get; set; }

        public DbSet<InvestmentModel> Investments { get; set; }

        public DbSet<InvestmentIntervalModel> InvestmentIntervals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

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