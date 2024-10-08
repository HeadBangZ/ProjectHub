﻿using ProjectHub.Domain.Common.ValueObjects;
using ProjectHub.Domain.Workspace.RelationShips;

namespace ProjectHub.Domain.Common.Entities;

public sealed class Tag
{
    public TagId Id { get; private init; } = TagId.NewEntityId();

    public string Name { get; set; }

    public string Color { get; set; }

    public IReadOnlyCollection<EpicTag> EpicTags { get; set; }
}
