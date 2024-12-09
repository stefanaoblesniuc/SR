using Microsoft.AspNetCore.Mvc;
using MovieApp.DataBase;
using MovieApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.Services;

public class PreferenceFormService
{
    private readonly MovieAppDbContext _dbContext;

    public PreferenceFormService(MovieAppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<IActionResult> AddPreferenceAsync(Guid userId, List<string> genres, string imdbScore, string language)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var preferenceForm = new PreferenceForm
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Genres = genres,
            IMDBScore = imdbScore,
            Language = language,
            CreatedDate = DateTime.UtcNow
        };

        _dbContext.PreferenceForms.Add(preferenceForm);
        await _dbContext.SaveChangesAsync();

        return new ObjectResult(new { message = "Preferences saved successfully!" });
    }

    public async Task<PreferenceForm> GetPreferencesAsync(Guid userId)
    {
        var preference = await _dbContext.PreferenceForms
            .FirstOrDefaultAsync(p => p.UserId == userId);

        return preference;
    }
}
