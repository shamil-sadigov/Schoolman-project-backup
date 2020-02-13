namespace Domain.Models
{
    /// <summary>
    /// Base interface for all DB entities
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntityBase<TKey> : IEntity<TKey>, 
                                         IDeleteableEntity, 
                                         IAuditableEntity
    {
      
    }
}
