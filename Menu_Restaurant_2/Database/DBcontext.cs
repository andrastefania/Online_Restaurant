using System.Data.Entity;
using Menu_Restaurant_2.Model;

namespace Menu_Restaurant_2.DataBase
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<FoodMenu> FoodMenu { get; set; }
        public DbSet<Orders> Orders { get; set; }

        public RestaurantDbContext()
            : base("Data Source=0903-ANDRA\\SQLEXPRESS;Initial Catalog=RestaurantMenuDB;Integrated Security=True")
        {
        }

        static RestaurantDbContext()
        {
            Database.SetInitializer<RestaurantDbContext>(null); // dezactivează re-crearea DB
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Food>().ToTable("Food");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<FoodMenu>().ToTable("FoodMenu");
            modelBuilder.Entity<Orders>().ToTable("Orders");

            modelBuilder.Entity<Orders>()
                .Property(o => o.FoodPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TransportPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalPrice)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
