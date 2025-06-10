using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Mapping;

namespace DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<Account>, IUnitOfWork
    {
        
        //public DbSet<About>? About { get; set; }
        public DbSet<Auction>? Auction { get; set; }
        public DbSet<Boss>? Boss { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<Contact>? Contact { get; set; }
        public DbSet<Customer>? Customer { get; set; }
        public DbSet<Employee>? Employee { get; set; }
        public DbSet<Gallery>? Gallery { get; set; }
        public DbSet<GroupGallery>? GroupGallery { get; set; }
        public DbSet<GroupProduct>? GroupProduct { get; set; }
        public DbSet<Link>? Link { get; set; }
        public DbSet<News>? News { get; set; }
        public DbSet<Post>? Post { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<Setting>? Setting { get; set; }
        public DbSet<Slider>? Slider { get; set; }
        public DbSet<TenderOffer>? TenderOffer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                  .UseSqlServer(ConnectionString.Value)
                  .UseLazyLoadingProxies();
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Seed();
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}