using Dapper;

namespace Sample.Dapper.Persistence.Entities;

public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the entity identifier.
    /// </summary>
    /// <value>
    /// The entity identifier.
    /// </value>
    [Key]
    public long EntityId { get; set; }
}
