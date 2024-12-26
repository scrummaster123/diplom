namespace Afisha.Domain.Entities.Abstractions
{
    public abstract class EntityBase<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
    }
}