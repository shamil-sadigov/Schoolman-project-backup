namespace Domain.Models
{
    public interface IEntityBase<TKey> : IEntity<TKey>, 
                                         IDeleteableEntity, 
                                         IAuditableEntity
    {
      
    }
}
