using System.Text.Json.Serialization;

namespace Framework.Domain;

public abstract class Entity<TKey> : IEntity
{
    public TKey Id { get; set; }

}