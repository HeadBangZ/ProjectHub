﻿using PersonalHub.Application.Contracts;
using PersonalHub.Application.DTOs.FeatureDtos;
using PersonalHub.Application.Extensions;
using PersonalHub.Domain.Contracts;
using PersonalHub.Domain.Workspace.ValueObjects;

namespace PersonalHub.Application.Services;

public class FeatureService : IFeatureService
{
    private readonly IFeatureRepository _featureRepository;
    public FeatureService(IFeatureRepository featureRepository)
    {
        _featureRepository = featureRepository;
    }

    public async Task<FeatureDtoResponse> AddFeature(CreateFeatureDtoRequest request)
    {
        var feature = request.MapCreateDtoToFeature();

        await _featureRepository.AddAsync(feature);
        return feature.MapFeatureToDto();
    }

    // OPTIMIZE: Add query rules, for pagination, include details 
    public async Task<FeatureDtoResponse?> GetFeature(Guid id)
    {
        var feature = await _featureRepository.GetAsync(new FeatureId(id));

        if (feature == null)
        {
            return null;
        }

        var featureDto = feature.MapFeatureToDto();

        return featureDto;
    }

    // OPTIMIZE: Add query rules, for pagination, include details, filter 
    public async Task<List<FeatureDtoResponse>> GetAllFeatures()
    {
        var featureDtos = new List<FeatureDtoResponse>();
        var features = await _featureRepository.GetAllAsync();

        foreach (var feature in features)
        {
            featureDtos.Add(feature.MapFeatureToDto());
        }
        return featureDtos;
    }

    public async Task DeleteFeature(Guid id)
    {
        await _featureRepository.DeleteAsync(new FeatureId(id));
    }

    public async Task UpdateFeature(Guid id, UpdateFeatureDtoRequest request)
    {
        var existingFeature = await _featureRepository.GetAsync(new FeatureId(id));

        if (existingFeature == null)
        {
            throw new Exception($"Feature with ID - {id} was not found");
        }

        var updatedFeature = request.MapDtoToFeature(existingFeature);

        updatedFeature.ModifiedAt = DateTime.Now;

        await _featureRepository.UpdateAsync(updatedFeature);
    }
}
