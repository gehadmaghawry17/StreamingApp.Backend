using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class ReviewsController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public ReviewsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var reviews = await _dbContext.Reviews
            .AsNoTracking()
            .Include(review => review.User)
            .OrderBy(review => review.IsApproved)
            .ThenByDescending(review => review.CreatedAt)
            .Select(review => new ReviewListItemViewModel
            {
                Id = review.Id,
                UserEmail = review.User.Email,
                ContentType = review.ContentType,
                ContentId = review.ContentId,
                BodyPreview = review.Body.Length > 120 ? review.Body.Substring(0, 120) + "..." : review.Body,
                IsApproved = review.IsApproved,
                CreatedAt = review.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return View(reviews);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var review = await _dbContext.Reviews
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (review is null)
        {
            return NotFound();
        }

        return View(new ReviewFormViewModel
        {
            Id = review.Id,
            UserEmail = review.User.Email,
            ContentType = review.ContentType,
            ContentId = review.ContentId,
            Body = review.Body,
            IsApproved = review.IsApproved
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReviewFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var review = await _dbContext.Reviews.FindAsync([id], cancellationToken);
        if (review is null)
        {
            return NotFound();
        }

        review.Body = model.Body.Trim();
        review.IsApproved = model.IsApproved;
        review.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var review = await _dbContext.Reviews.FindAsync([id], cancellationToken);
        if (review is not null)
        {
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }
}
