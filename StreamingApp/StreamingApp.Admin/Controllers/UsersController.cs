using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class UsersController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public UsersController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Subscriptions)
            .OrderBy(user => user.Email)
            .Select(user => new UserListItemViewModel
            {
                Id = user.Id,
                PublicId = user.PublicId,
                Email = user.Email,
                IsEmailVerified = user.IsEmailVerified,
                CreatedAt = user.CreatedAt,
                SubscriptionCount = user.Subscriptions.Count
            })
            .ToListAsync(cancellationToken);

        return View(users);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        return View(new UserFormViewModel
        {
            Id = user.Id,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _dbContext.Users.FindAsync([id], cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        user.Email = model.Email.Trim();
        user.IsEmailVerified = model.IsEmailVerified;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
