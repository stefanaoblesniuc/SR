using Microsoft.AspNetCore.Mvc;
using MovieApp.DTO.PreferenceForm;
using MovieApp.Entities;
using MovieApp.Services;

namespace MovieApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PreferenceFormController : ControllerBase
{
    private readonly PreferenceFormService _preferenceFormService;

    public PreferenceFormController(PreferenceFormService preferenceFormService)
    {
        _preferenceFormService = preferenceFormService;
    }

    // POST: api/PreferenceForm
    [HttpPost]
    public async Task<IActionResult> AddPreference([FromBody] PreferenceFormAddDTO preferenceForm)
    {
        var result = await _preferenceFormService.AddPreferenceAsync(preferenceForm.Username, preferenceForm.Genres, preferenceForm.IMDBScore, preferenceForm.Language);
        return result;
    }

    // GET: api/PreferenceForm/{userId}
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPreferences(Guid userId)
    {
        var preferences = await _preferenceFormService.GetPreferencesAsync(userId);

        if (preferences == null)
        {
            return NotFound(new { message = "Preferences not found." });
        }

        return Ok(new { message = "Preferences retrieved successfully.", preferences });
    }
}

