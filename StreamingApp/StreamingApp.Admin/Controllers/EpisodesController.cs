using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class EpisodesController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public EpisodesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var episodes = await _dbContext.Episodes
            .AsNoTracking()
            .Include(episode => episode.Season)
            .ThenInclude(season => season.Series)
            .OrderBy(episode => episode.Season.Series.Title)
            .ThenBy(episode => episode.Season.SeasonNumber)
            .ThenBy(episode => episode.EpisodeNumber)
            .Select(episode => new EpisodeListItemViewModel
            {
                Id = episode.Id,
                SeriesTitle = episode.Season.Series.Title,
                SeasonTitle = episode.Season.Title,
                EpisodeNumber = episode.EpisodeNumber,
                Title = episode.Title,
                DurationSeconds = episode.DurationSeconds
            })
            .ToListAsync(cancellationToken);

        return View(episodes);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new EpisodeFormViewModel
        {
            SeasonOptions = await GetSeasonOptionsAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EpisodeFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.SeasonOptions = await GetSeasonOptionsAsync(cancellationToken);
            return View(model);
        }

        var episode = new Episode
        {
            SeasonId = model.SeasonId,
            EpisodeNumber = model.EpisodeNumber,
            Title = model.Title.Trim(),
            Description = model.Description?.Trim(),
            VideoUrl = model.VideoUrl.Trim(),
            DurationSeconds = model.DurationSeconds,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Episodes.Add(episode);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var episode = await _dbContext.Episodes.FindAsync([id], cancellationToken);
        if (episode is null)
        {
            return NotFound();
        }

        return View(new EpisodeFormViewModel
        {
            Id = episode.Id,
            SeasonId = episode.SeasonId,
            EpisodeNumber = episode.EpisodeNumber,
            Title = episode.Title,
            Description = episode.Description,
            VideoUrl = episode.VideoUrl,
            DurationSeconds = episode.DurationSeconds,
            SeasonOptions = await GetSeasonOptionsAsync(cancellationToken)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EpisodeFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.SeasonOptions = await GetSeasonOptionsAsync(cancellationToken);
            return View(model);
        }

        var episode = await _dbContext.Episodes.FindAsync([id], cancellationToken);
        if (episode is null)
        {
            return NotFound();
        }

        episode.SeasonId = model.SeasonId;
        episode.EpisodeNumber = model.EpisodeNumber;
        episode.Title = model.Title.Trim();
        episode.Description = model.Description?.Trim();
        episode.VideoUrl = model.VideoUrl.Trim();
        episode.DurationSeconds = model.DurationSeconds;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var episode = await _dbContext.Episodes.FindAsync([id], cancellationToken);
        if (episode is not null)
        {
            _dbContext.Episodes.Remove(episode);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetSeasonOptionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Seasons
            .AsNoTracking()
            .Include(season => season.Series)
            .OrderBy(season => season.Series.Title)
            .ThenBy(season => season.SeasonNumber)
            .Select(season => new SelectListItem(
                $"{season.Series.Title} - Season {season.SeasonNumber}: {season.Title}",
                season.Id.ToString()))
            .ToListAsync(cancellationToken);
    }
}
