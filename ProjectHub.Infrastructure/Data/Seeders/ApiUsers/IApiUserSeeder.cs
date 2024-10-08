﻿using ProjectHub.Domain.User.Entities;

namespace ProjectHub.Infrastructure.Data.Seeders.ApiUsers;

public interface IApiUserSeeder
{
    Task<ApiUser> Seed();
}