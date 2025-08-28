using Classifieds.MinimalApi.Auth;
using Classifieds.MinimalApi.Entities;
using Classifieds.MinimalApi.Dtos;

namespace Classifieds.MinimalApi.Mapping;

public static class UserMapping
{
    public static User ToEntity(this UserRegistrationDto dto)
    {
        return new User
        {
            Name = dto.Name,
            Login = dto.Login,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            RoleId = 1
        };
    }
}