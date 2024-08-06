﻿using PersonalHub.Application.Contracts.Repositories;
using PersonalHub.Domain.Entities;
using PersonalHub.Infrastructure.Data.Contexts;

namespace PersonalHub.Infrastructure.Repositories;

public class UserStoryRepository : GenericRepository<UserStory>, IUserStoryRepository
{
    private readonly PersonalHubDbContext _context;

    public UserStoryRepository(PersonalHubDbContext context) : base(context)
    {
        _context = context;
    }
}
