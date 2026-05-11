using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class SeasonsController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public SeasonsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var seasons = await _dbContext.Seasons
            .AsNoTracking()
            .Include(season => season.Series)
            .Include(season => season.Episodes)
            .OrderBy(season => season.Series.Title)
            .ThenBy(season => season.SeasonNumber)
            .Select(season => new SeasonListItemViewModel
            {
                Id = season.Id,
                SeriesTitle = season.Series.Title,
                SeasonNumber = season.SeasonNumber,
                Title = season.Title,
                EpisodeCount = season.Episodes.Count
            })
            .ToListAsync(cancellationToken);

        return View(seasons);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new SeasonFormViewModel
        {
            SeriesOptions = await GetSeriesOptionsAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeasonFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.SeriesOptions = await GetSeriesOptionsAsync(cancellationToken);
            return View(model);
        }

        var season = new Season
        {
            SeriesId = model.SeriesId,
            SeasonNumber = model.SeasonNumber,
            Title = model.Title.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Seasons.Add(season);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var season = await _dbContext.Seasons.FindAsync([id], cancellationToken);
        if (season is null)
        {
            return NotFound();
        }

        return View(new SeasonFormViewModel
        {
            Id = season.Id,
            SeriesId = season.SeriesId,
            SeasonNumber = season.SeasonNumber,
            Title = season.Title,
            SeriesOptions = await GetSeriesOptionsAsync(cancellationToken)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SeasonFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.SeriesOptions = await GetSeriesOptionsAsync(cancellationToken);
            return View(model);
        }

        var season = await _dbContext.Seasons.FindAsync([id], cancellationToken);
        if (season is null)
        {
            return NotFound();
        }

        season.SeriesId = model.SeriesId;
        season.SeasonNumber = model.SeasonNumber;
        season.Title = model.Title.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var season = await _dbContext.Seasons.FindAsync([id], cancellationToken);
        if (season is not null)
        {
            _dbContext.Seasons.Remove(season);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetSeriesOptionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Series
            .AsNoTracking()
            .OrderBy(series => series.Title)
            .Select(series => new SelectListItem(series.Title, series.Id.ToString()))
            .ToListAsync(cancellationToken);
    }
}
