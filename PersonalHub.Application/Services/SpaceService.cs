﻿using PersonalHub.Application.Contracts;
using PersonalHub.Application.DTOs;
using PersonalHub.Application.Extensions;
using PersonalHub.Domain.Contracts;

namespace PersonalHub.Application.Services;

public class SpaceService : ISpaceService
{
    private readonly ISpaceRepository _spaceRepository;
    public SpaceService(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<SpaceDto> AddSpace(CreateSpaceDto spaceDto)
    {
        var space = spaceDto.MapCreateDtoToSpace();

        await _spaceRepository.AddAsync(space);
        return space.MapSpaceToDto();
    }

    public Task DeleteSpace(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<SpaceDto>> GetAllSpaces()
    {
        throw new NotImplementedException();
    }

    public Task<SpaceDto?> GetSpace(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSpace(Guid id, UpdateSpaceDto spaceDto)
    {
        throw new NotImplementedException();
    }
}
