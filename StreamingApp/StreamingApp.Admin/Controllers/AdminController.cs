using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class AdminController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public AdminController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = new AdminDashboardViewModel
        {
            GenreCount = await _dbContext.Genres.CountAsync(cancellationToken),
            MovieCount = await _dbContext.Movies.CountAsync(cancellationToken),
            SeriesCount = await _dbContext.Series.CountAsync(cancellationToken),
            SeasonCount = await _dbContext.Seasons.CountAsync(cancellationToken),
            EpisodeCount = await _dbContext.Episodes.CountAsync(cancellationToken),
            UserCount = await _dbContext.Users.CountAsync(cancellationToken),
            SubscriptionPlanCount = await _dbContext.SubscriptionPlans.CountAsync(cancellationToken),
            UserSubscriptionCount = await _dbContext.UserSubscriptions.CountAsync(cancellationToken),
            PendingReviewCount = await _dbContext.Reviews.CountAsync(review => !review.IsApproved, cancellationToken),
            PromotedContentCount = await _dbContext.PromotedContents.CountAsync(cancellationToken)
        };

        return View(model);
    }
}
