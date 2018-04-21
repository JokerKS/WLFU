using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Notification;
using JokerKS.WLFU.Entities.Order;
using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Entities.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;

namespace JokerKS.WLFU
{
    public class AppContext : IdentityDbContext<AppUser>
    {
        public DbSet<ProductCategory> ProductCategories { get; set; }

        #region Product tables
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductRating> ProductRatings { get; set; }
        #endregion

        #region Auction tables
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionTag> AuctionTags { get; set; }
        public DbSet<AuctionComment> AuctionComments { get; set; }
        public DbSet<AuctionImage> AuctionImages { get; set; }
        public DbSet<BidAtAuction> Bids { get; set; } 
        #endregion


        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<BasketProduct> BasketProducts { get; set; }

        #region Order tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrderDetail> ProductOrderDetails { get; set; }
        public DbSet<AuctionOrderDetail> AuctionOrderDetails { get; set; }
        #endregion

        public DbSet<Notification> Notifications { get; set; }

        public AppContext(): base("WLFUdb")
        {
            Database.SetInitializer(new DbInitial());
        }

        #region OnModelCreating()
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(16, 2);

            modelBuilder.Entity<Product>()
                .HasMany(u => u.Images)
                .WithRequired(a => a.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Auction>()
                .HasMany(u => u.Images)
                .WithRequired(a => a.Auction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.BasketProducts)
                .WithRequired(a => a.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(u => u.Ratings)
                .WithRequired(a => a.Product)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("Users", "dbo");
        }
        #endregion

        #region Create()
        public static AppContext Create()
        {
            return new AppContext();
        } 
        #endregion
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