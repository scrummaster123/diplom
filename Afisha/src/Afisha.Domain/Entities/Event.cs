﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities;

public class Event : EntityBase<long>
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public new long Id { get; init; }

    /// <summary>
    /// Id места проведения мероприятия
    /// </summary>
    public long LocationId { get; set; }

    /// <summary>
    /// Место проведения мероприятия
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    /// Дата начала мероприятия
    /// </summary>
    public DateOnly DateStart { get; set; }

    public bool IsOpenToRegister { get; set; }
    
    /// <summary>
    ///     Many to many сущность участников мероприятия с их ролями
    /// </summary>
    public ICollection<EventUser> EventParticipants { get; set; } = [];

    public override string ToString()
    {
        return $"{Id}";
    }
}