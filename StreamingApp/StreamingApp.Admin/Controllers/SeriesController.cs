using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class SeriesController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public SeriesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var series = await _dbContext.Series
            .AsNoTracking()
            .Include(item => item.SeriesGenres)
            .ThenInclude(seriesGenre => seriesGenre.Genre)
            .OrderBy(item => item.Title)
            .ToListAsync(cancellationToken);

        var model = series.Select(item => new SeriesListItemViewModel
        {
            Id = item.Id,
            Title = item.Title,
            Slug = item.Slug,
            IsPublished = item.IsPublished,
            Genres = string.Join(", ", item.SeriesGenres.Select(seriesGenre => seriesGenre.Genre.Name).OrderBy(name => name))
        }).ToList();

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new SeriesFormViewModel
        {
            Genres = await GetGenreCheckboxesAsync([], cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeriesFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.Genres = await GetGenreCheckboxesAsync(model.SelectedGenreIds, cancellationToken);
            return View(model);
        }

        var now = DateTime.UtcNow;
        var series = new StreamingApp.Domain.Entities.Series
        {
            Title = model.Title.Trim(),
            Slug = BuildSlug(model.Slug, model.Title),
            Description = model.Description?.Trim(),
            ThumbnailUrl = model.ThumbnailUrl?.Trim(),
            TrailerUrl = model.TrailerUrl.Trim(),
            IsPublished = model.IsPublished,
            CreatedAt = now,
            UpdatedAt = now
        };

        foreach (var genreId in model.SelectedGenreIds.Distinct())
        {
            series.SeriesGenres.Add(new SeriesGenre { GenreId = genreId });
        }

        _dbContext.Series.Add(series);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var series = await _dbContext.Series
            .Include(item => item.SeriesGenres)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (series is null)
        {
            return NotFound();
        }

        var selectedGenreIds = series.SeriesGenres.Select(seriesGenre => seriesGenre.GenreId).ToList();
        var model = new SeriesFormViewModel
        {
            Id = series.Id,
            Title = series.Title,
            Slug = series.Slug,
            Description = series.Description,
            ThumbnailUrl = series.ThumbnailUrl,
            TrailerUrl = series.TrailerUrl,
            IsPublished = series.IsPublished,
            SelectedGenreIds = selectedGenreIds,
            Genres = await GetGenreCheckboxesAsync(selectedGenreIds, cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SeriesFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.Genres = await GetGenreCheckboxesAsync(model.SelectedGenreIds, cancellationToken);
            return View(model);
        }

        var series = await _dbContext.Series
            .Include(item => item.SeriesGenres)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (series is null)
        {
            return NotFound();
        }

        series.Title = model.Title.Trim();
        series.Slug = BuildSlug(model.Slug, model.Title);
        series.Description = model.Description?.Trim();
        series.ThumbnailUrl = model.ThumbnailUrl?.Trim();
        series.TrailerUrl = model.TrailerUrl.Trim();
        series.IsPublished = model.IsPublished;
        series.UpdatedAt = DateTime.UtcNow;

        series.SeriesGenres.Clear();
        foreach (var genreId in model.SelectedGenreIds.Distinct())
        {
            series.SeriesGenres.Add(new SeriesGenre { SeriesId = series.Id, GenreId = genreId });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var series = await _dbContext.Series.FindAsync([id], cancellationToken);
        if (series is not null)
        {
            _dbContext.Series.Remove(series);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<GenreCheckboxViewModel>> GetGenreCheckboxesAsync(
        IEnumerable<int> selectedGenreIds,
        CancellationToken cancellationToken)
    {
        var selected = selectedGenreIds.ToHashSet();

        return await _dbContext.Genres
            .AsNoTracking()
            .OrderBy(genre => genre.Name)
            .Select(genre => new GenreCheckboxViewModel
            {
                Id = genre.Id,
                Name = genre.Name,
                IsSelected = selected.Contains(genre.Id)
            })
            .ToListAsync(cancellationToken);
    }

    private static string BuildSlug(string? slug, string title)
    {
        return string.IsNullOrWhiteSpace(slug)
            ? title.Trim().ToLowerInvariant().Replace(' ', '-')
            : slug.Trim().ToLowerInvariant();
    }
}
