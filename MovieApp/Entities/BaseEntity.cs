namespace MovieApp.Entities;

public abstract class BaseEntity
{
    /// <summary>
    /// The Id property is a unique identifier for each entity, usually all database tables use a primary key to index data.
    /// Also, the primary key is used by the ORM to uniquely identify the entity instance and track the changes to it.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The CreatedAt property represents the date and time when the entity was created.
    /// It is better to use UTC than the local timezone to remove ambiguity.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The UpdatedAt property represents the date and time when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This method updates the UpdatedAt property to the current date and time in UTC.
    /// </summary>
    public void UpdateTime() => UpdatedAt = DateTime.UtcNow;
}
