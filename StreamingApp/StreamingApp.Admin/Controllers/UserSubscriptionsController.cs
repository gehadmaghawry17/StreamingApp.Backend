using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class UserSubscriptionsController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public UserSubscriptionsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var subscriptions = await _dbContext.UserSubscriptions
            .AsNoTracking()
            .Include(subscription => subscription.User)
            .Include(subscription => subscription.SubscriptionPlan)
            .OrderByDescending(subscription => subscription.CreatedAt)
            .Select(subscription => new UserSubscriptionListItemViewModel
            {
                Id = subscription.Id,
                UserEmail = subscription.User.Email,
                PlanName = subscription.SubscriptionPlan.Name,
                Status = subscription.Status,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                AutoRenew = subscription.AutoRenew
            })
            .ToListAsync(cancellationToken);

        return View(subscriptions);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new UserSubscriptionFormViewModel
        {
            UserOptions = await GetUserOptionsAsync(cancellationToken),
            PlanOptions = await GetPlanOptionsAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserSubscriptionFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync(model, cancellationToken);
            return View(model);
        }

        var subscription = new UserSubscription
        {
            UserId = model.UserId,
            SubscriptionPlanId = model.SubscriptionPlanId,
            Status = model.Status,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            AutoRenew = model.AutoRenew,
            CancelledAt = model.CancelledAt,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.UserSubscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var subscription = await _dbContext.UserSubscriptions.FindAsync([id], cancellationToken);
        if (subscription is null)
        {
            return NotFound();
        }

        var model = new UserSubscriptionFormViewModel
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CancelledAt = subscription.CancelledAt
        };

        await LoadOptionsAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserSubscriptionFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync(model, cancellationToken);
            return View(model);
        }

        var subscription = await _dbContext.UserSubscriptions.FindAsync([id], cancellationToken);
        if (subscription is null)
        {
            return NotFound();
        }

        subscription.UserId = model.UserId;
        subscription.SubscriptionPlanId = model.SubscriptionPlanId;
        subscription.Status = model.Status;
        subscription.StartDate = model.StartDate;
        subscription.EndDate = model.EndDate;
        subscription.AutoRenew = model.AutoRenew;
        subscription.CancelledAt = model.CancelledAt;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var subscription = await _dbContext.UserSubscriptions.FindAsync([id], cancellationToken);
        if (subscription is not null)
        {
            _dbContext.UserSubscriptions.Remove(subscription);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadOptionsAsync(UserSubscriptionFormViewModel model, CancellationToken cancellationToken)
    {
        model.UserOptions = await GetUserOptionsAsync(cancellationToken);
        model.PlanOptions = await GetPlanOptionsAsync(cancellationToken);
    }

    private async Task<List<SelectListItem>> GetUserOptionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.Email)
            .Select(user => new SelectListItem(user.Email, user.Id.ToString()))
            .ToListAsync(cancellationToken);
    }

    private async Task<List<SelectListItem>> GetPlanOptionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SubscriptionPlans
            .AsNoTracking()
            .OrderBy(plan => plan.Name)
            .Select(plan => new SelectListItem($"{plan.Name} - {plan.Price:C}", plan.Id.ToString()))
            .ToListAsync(cancellationToken);
    }
}
