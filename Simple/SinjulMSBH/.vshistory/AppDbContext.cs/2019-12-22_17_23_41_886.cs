using Microsoft.EntityFrameworkCore;

namespace Simple.SinjulMSBH
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Customer> Customers { get; set; }

		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Customer>().OwnsOne(navigationExpression: c => c.HomeAddress).WithOwner();
		}
	}
}
