using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class MoviesController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public MoviesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var movies = await _dbContext.Movies
            .AsNoTracking()
            .Include(movie => movie.MovieGenres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .OrderBy(movie => movie.Title)
            .ToListAsync(cancellationToken);

        var model = movies.Select(movie => new MovieListItemViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            ReleaseYear = movie.ReleaseYear,
            IsPublished = movie.IsPublished,
            Genres = string.Join(", ", movie.MovieGenres.Select(movieGenre => movieGenre.Genre.Name).OrderBy(name => name))
        }).ToList();

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new MovieFormViewModel
        {
            Genres = await GetGenreCheckboxesAsync([], cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.Genres = await GetGenreCheckboxesAsync(model.SelectedGenreIds, cancellationToken);
            return View(model);
        }

        var now = DateTime.UtcNow;
        var movie = new Movie
        {
            Title = model.Title.Trim(),
            Slug = BuildSlug(model.Slug, model.Title),
            Description = model.Description?.Trim(),
            ThumbnailUrl = model.ThumbnailUrl?.Trim(),
            TrailerUrl = model.TrailerUrl.Trim(),
            VideoUrl = model.VideoUrl.Trim(),
            DurationSeconds = model.DurationSeconds,
            ReleaseYear = model.ReleaseYear,
            IsPublished = model.IsPublished,
            CreatedAt = now,
            UpdatedAt = now
        };

        foreach (var genreId in model.SelectedGenreIds.Distinct())
        {
            movie.MovieGenres.Add(new MovieGenre { GenreId = genreId });
        }

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies
            .Include(movie => movie.MovieGenres)
            .FirstOrDefaultAsync(movie => movie.Id == id, cancellationToken);

        if (movie is null)
        {
            return NotFound();
        }

        var selectedGenreIds = movie.MovieGenres.Select(movieGenre => movieGenre.GenreId).ToList();
        var model = new MovieFormViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            Description = movie.Description,
            ThumbnailUrl = movie.ThumbnailUrl,
            TrailerUrl = movie.TrailerUrl,
            VideoUrl = movie.VideoUrl,
            DurationSeconds = movie.DurationSeconds,
            ReleaseYear = movie.ReleaseYear,
            IsPublished = movie.IsPublished,
            SelectedGenreIds = selectedGenreIds,
            Genres = await GetGenreCheckboxesAsync(selectedGenreIds, cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MovieFormViewModel model, CancellationToken cancellationToken)
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

        var movie = await _dbContext.Movies
            .Include(movie => movie.MovieGenres)
            .FirstOrDefaultAsync(movie => movie.Id == id, cancellationToken);

        if (movie is null)
        {
            return NotFound();
        }

        movie.Title = model.Title.Trim();
        movie.Slug = BuildSlug(model.Slug, model.Title);
        movie.Description = model.Description?.Trim();
        movie.ThumbnailUrl = model.ThumbnailUrl?.Trim();
        movie.TrailerUrl = model.TrailerUrl.Trim();
        movie.VideoUrl = model.VideoUrl.Trim();
        movie.DurationSeconds = model.DurationSeconds;
        movie.ReleaseYear = model.ReleaseYear;
        movie.IsPublished = model.IsPublished;
        movie.UpdatedAt = DateTime.UtcNow;

        movie.MovieGenres.Clear();
        foreach (var genreId in model.SelectedGenreIds.Distinct())
        {
            movie.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FindAsync([id], cancellationToken);
        if (movie is not null)
        {
            _dbContext.Movies.Remove(movie);
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
