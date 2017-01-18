using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL
{
    public class AdvContext : DbContext
    {
        public AdvContext()
            : base("DefaultConnection")
        {
        }

        public AdvContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Adv> advs { get; set; }
        public DbSet<CarAdv> caradvs { get; set; }
        
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogComment> Comments { get; set; }

        public DbSet<BlogPost> Posts { get; set; }

        public DbSet<SiteMessage> SiteMessages { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<CarBrand> CarBrand { get; set; }

        public DbSet<CarModel> CarModel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            
        modelBuilder.Entity<Adv>().Map(m =>
        {
            //m.MapInheritedProperties();
            m.ToTable("Adv");
        });

        modelBuilder.Entity<CarAdv>().Map(m =>
        {
            //m.MapInheritedProperties();
            m.ToTable("CarAdv");
        });            
    
        }
    }

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }


}
