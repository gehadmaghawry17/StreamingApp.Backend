using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class PromotedContentController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public PromotedContentController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var promotedContent = await _dbContext.PromotedContents
            .AsNoTracking()
            .OrderBy(item => item.DisplayOrder)
            .Select(item => new PromotedContentListItemViewModel
            {
                Id = item.Id,
                Label = item.Label,
                ContentType = item.ContentType,
                ContentId = item.ContentId,
                DisplayOrder = item.DisplayOrder,
                IsActive = item.IsActive
            })
            .ToListAsync(cancellationToken);

        return View(promotedContent);
    }

    public IActionResult Create()
    {
        return View(new PromotedContentFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PromotedContentFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var now = DateTime.UtcNow;
        var item = new PromotedContent
        {
            Label = model.Label.Trim(),
            ContentType = model.ContentType,
            ContentId = model.ContentId,
            DisplayOrder = model.DisplayOrder,
            IsActive = model.IsActive,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.PromotedContents.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var item = await _dbContext.PromotedContents.FindAsync([id], cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        return View(new PromotedContentFormViewModel
        {
            Id = item.Id,
            Label = item.Label,
            ContentType = item.ContentType,
            ContentId = item.ContentId,
            DisplayOrder = item.DisplayOrder,
            IsActive = item.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PromotedContentFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var item = await _dbContext.PromotedContents.FindAsync([id], cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        item.Label = model.Label.Trim();
        item.ContentType = model.ContentType;
        item.ContentId = model.ContentId;
        item.DisplayOrder = model.DisplayOrder;
        item.IsActive = model.IsActive;
        item.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var item = await _dbContext.PromotedContents.FindAsync([id], cancellationToken);
        if (item is not null)
        {
            _dbContext.PromotedContents.Remove(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }
}
