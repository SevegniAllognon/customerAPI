using HomeAssigment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeAssigment.DataAccessLayer
{
    public class CustomerDbContext:DbContext
    {
        #region Constructors
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {

        }
        #endregion

        #region DbSets
        public virtual DbSet<Customer> Customers { get; set; }



        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasIndex(x => new { x.emailaddress }).IsUnique();
            base.OnModelCreating(modelBuilder);

            #region Model creating

            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Name=ConnectionStrings:CustomerService");
        }

        #endregion

    }
}
