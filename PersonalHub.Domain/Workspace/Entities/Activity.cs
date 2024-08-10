﻿using PersonalHub.Domain.Base.Entities;
using PersonalHub.Domain.Workspace.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalHub.Domain.Workspace.Entities;

public class Activity : BaseEntity
{
    public Guid Id { get; private set; } = new();

    [Required]
    public Guid FeatureId { get; set; }

    [Required]
    [StringLength(75)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsCompleted { get; set; } = false;
}
