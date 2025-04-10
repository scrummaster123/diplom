using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Afisha.Domain.Enums;

namespace Afisha.Domain.Entities;

public class EventUser
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }
    
    public long EventId { get; set; }
    [Required]
    public Event Event { get; set; }

    public long UserId { get; set; }
    [Required]
    public User User { get; set; }
    
    public EventRole UserRole { get; set; }
}