using Classifieds.MinimalApi.Auth;
using Classifieds.MinimalApi.Data;
using Classifieds.MinimalApi.Dtos;
using Classifieds.MinimalApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Classifieds.MinimalApi.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        // POST   /registration
        app.MapPost("registration", async (UserRegistrationDto registrationDto, ClassifiedsContext dbContext) =>
        {
            if (await dbContext.Users.AnyAsync(u => u.Login == registrationDto.Login)) 
                return Results.Conflict("Логин уже существует.");

            var user = registrationDto.ToEntity();

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { user.Id, user.Login });
        });

        // POST   /login
        app.MapPost("login", async (UserLoginDto loginDto, TokenGenerator tokenGenerator, ClassifiedsContext dbContext) =>
        {
            var user = await dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == loginDto.Login);
            if (user is null || !PasswordHasher.Verify(loginDto.Password, user.PasswordHash)) 
                return Results.Unauthorized();
    
            var token = tokenGenerator.GenerateToken(loginDto.Login, user.Role.Name);

            return Results.Ok(new
            {
                accessToken = token,
                tokenType = "Bearer",
                expiresIn = 900
            });
        });
        
        return app;
    }
}