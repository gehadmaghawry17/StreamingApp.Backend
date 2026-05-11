using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class SubscriptionPlansController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public SubscriptionPlansController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var plans = await _dbContext.SubscriptionPlans
            .AsNoTracking()
            .OrderBy(plan => plan.Price)
            .Select(plan => new SubscriptionPlanListItemViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Duration = plan.Duration,
                Price = plan.Price,
                DurationDays = plan.DurationDays
            })
            .ToListAsync(cancellationToken);

        return View(plans);
    }

    public IActionResult Create()
    {
        return View(new SubscriptionPlanFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionPlanFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var plan = new SubscriptionPlan
        {
            Name = model.Name.Trim(),
            Duration = model.Duration,
            Price = model.Price,
            DurationDays = model.DurationDays,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.SubscriptionPlans.Add(plan);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var plan = await _dbContext.SubscriptionPlans.FindAsync([id], cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        return View(new SubscriptionPlanFormViewModel
        {
            Id = plan.Id,
            Name = plan.Name,
            Duration = plan.Duration,
            Price = plan.Price,
            DurationDays = plan.DurationDays
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubscriptionPlanFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var plan = await _dbContext.SubscriptionPlans.FindAsync([id], cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        plan.Name = model.Name.Trim();
        plan.Duration = model.Duration;
        plan.Price = model.Price;
        plan.DurationDays = model.DurationDays;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var plan = await _dbContext.SubscriptionPlans.FindAsync([id], cancellationToken);
        if (plan is not null)
        {
            _dbContext.SubscriptionPlans.Remove(plan);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }
}
