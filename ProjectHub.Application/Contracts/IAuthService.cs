﻿using Microsoft.AspNetCore.Identity;
using ProjectHub.Application.DTOs.ApiUserDtos;

namespace ProjectHub.Application.Contracts;

public interface IAuthService
{
    Task<IEnumerable<IdentityError>> Register(CreateApiUserDtoRequest request);
    Task<AuthDtoResponse?> AuthenticateUser(LoginDtoRequest loginRequest);
    Task<AuthDtoResponse> VerifyRefreshToken(AuthDtoResponse request);
}
