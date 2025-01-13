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

    public async Task<IActionResult> AddPreferenceAsync(string userName, string genre, string imdbScore, string language)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var preferenceForm = new PreferenceForm
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Genres = genre,
            IMDBScore = imdbScore,
            Language = language
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
