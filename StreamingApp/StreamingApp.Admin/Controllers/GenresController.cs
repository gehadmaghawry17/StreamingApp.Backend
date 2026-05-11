using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingApp.Admin.Models;
using StreamingApp.Domain.Entities;
using StreamingApp.Infrastructure.Persistence;

namespace StreamingApp.Admin.Controllers;

public sealed class GenresController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public GenresController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var genres = await _dbContext.Genres
            .AsNoTracking()
            .OrderBy(genre => genre.Name)
            .ToListAsync(cancellationToken);

        return View(genres);
    }

    public IActionResult Create()
    {
        return View(new GenreFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GenreFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var genre = new Genre
        {
            Name = model.Name.Trim()
        };

        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var genre = await _dbContext.Genres.FindAsync([id], cancellationToken);
        if (genre is null)
        {
            return NotFound();
        }

        return View(new GenreFormViewModel
        {
            Id = genre.Id,
            Name = genre.Name
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GenreFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var genre = await _dbContext.Genres.FindAsync([id], cancellationToken);
        if (genre is null)
        {
            return NotFound();
        }

        genre.Name = model.Name.Trim();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var genre = await _dbContext.Genres.FindAsync([id], cancellationToken);
        if (genre is not null)
        {
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }
}
