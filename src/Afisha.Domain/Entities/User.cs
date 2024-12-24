using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities
{
    public class User : EntityBase<long>
    {
        public string UserName { get; set; }
        
        public ICollection<Event> Events { get; set; } = Array.Empty<Event>();
    }
}
