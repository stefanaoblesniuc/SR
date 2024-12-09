using Microsoft.AspNetCore.Mvc.ViewEngines;
using Recombee.ApiClient.Bindings;

namespace MovieApp.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;

    /// <summary>
    /// References to other entities such as this are used to automatically fetch correlated data, this is called a navigation property.
    /// Collection such as this can be used for Many-To-One or Many-To-Many relations.
    /// Note that this field will be null if not explicitly requested via a Include query, also note that the property is used by the ORM, in the database this collection doesn't exist. 
    /// </summary>
    /// 
    public ICollection<FavoriteMovie> FavoriteMovies { get; set; }
    public ICollection<PreferenceForm> PreferenceForms { get; set; }

}