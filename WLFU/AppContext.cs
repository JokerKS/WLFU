using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Product;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;

namespace JokerKS.WLFU
{
    public class AppContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public AppContext(): base("WLFUdb")
        {
            Database.SetInitializer(new DbInitial());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(16,2);
            modelBuilder.Entity<Product>()
                .HasMany(u => u.Images)
                .WithRequired(a => a.Product)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("Users", "dbo");
        }

        public static AppContext Create()
        {
            return new AppContext();
        }
    }

    public class DbInitial : CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext db)
        {
            db.Roles.Add(new AppRole() { Name = "administrator", Description = "Full rights" });
            db.Roles.Add(new AppRole() { Name = "moderator", Description = "Privileges to edit products and orders" });
            db.Roles.Add(new AppRole() { Name = "client", Description = "Right to add items and place orders" });
            db.SaveChanges();

            AppUser admin = new AppUser()
            {
                FirstName = "Viktor",
                Lastname = "Kozenko",
                UserName = "jokerks",
                Email = "kozenkovsktor102@gmail.com",
                BirthDate = new System.DateTime(1994, 12, 6)
            };

            AppUserManager userManager = new AppUserManager(new UserStore<AppUser>(db));
            RoleManager<AppRole> roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(db));

            if (userManager.Create(admin, "qwerty").Succeeded)
            {
                if (roleManager.RoleExists("administrator"))
                    userManager.AddToRole(admin.Id, "administrator");
            }
        }
    }

    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
                : base(store)
        {
        }
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            AppContext db = context.Get<AppContext>();
            return new AppUserManager(new UserStore<AppUser>(db));
        }
    }

    class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store)
                    : base(store)
        { }
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new
                    RoleStore<AppRole>(context.Get<AppContext>()));
        }
    }
}