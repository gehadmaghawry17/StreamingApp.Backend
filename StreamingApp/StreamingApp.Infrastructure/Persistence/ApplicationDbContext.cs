using Microsoft.EntityFrameworkCore;
using StreamingApp.Domain.Entities;
using StreamingApp.Domain.Entities.Userss;

namespace StreamingApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public DbSet<ViewingProgress> ViewingProgress { get; set; }
    public DbSet<WatchHistory> WatchHistories { get; set; }

    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<DeviceSession> DeviceSessions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Otp> Otps { get; set; }

    public DbSet<SearchLog> SearchLogs { get; set; }
    public DbSet<SearchTrend> SearchTrends { get; set; }

    public DbSet<Download> Downloads { get; set; }
    public DbSet<PromotedContent> PromotedContents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}