using System.IdentityModel.Tokens.Jwt;
using Classifieds.MinimalApi.Data;
using Classifieds.MinimalApi.Dtos;
using Classifieds.MinimalApi.Entities;
using Classifieds.MinimalApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Classifieds.MinimalApi.Endpoints;

public static class AdsEndpoints
{
    const string GetAdvertisementEndpointName = "GetAd";
    
    public static WebApplication MapAdsEndpoints(this WebApplication app)
    {
        // GET    /
        app.MapGet("/", () => Results.LocalRedirect("/ads"));
        
        // GET    /ads
        app.MapGet("ads", async (ClassifiedsContext dbContext) =>
        {
            var result = await dbContext.Ads.Include(ad => ad.Category).Include(ad => ad.User)
                .Select(ad => ad.ToSummaryDto()).AsNoTracking().ToListAsync();
            
            return Results.Ok(result);
        });

        // GET    /ads/{id}
        app.MapGet("ads/{id}", async (int id, ClassifiedsContext dbContext) =>
        {
            var ad = await dbContext.Ads.FindAsync(id);
            
            return ad is null ? Results.NotFound() : Results.Ok(ad.ToDetailsDto());
        }).WithName(GetAdvertisementEndpointName);

        // POST   /ads
        app.MapPost("ads", async (CreateAdDto newAdDto, ClassifiedsContext dbContext, HttpContext httpContext) =>
        {
            int userId = int.Parse(httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            Ad ad = newAdDto.ToEntity(userId);
            await dbContext.Ads.AddAsync(ad);
            await dbContext.SaveChangesAsync();
            
            return Results.CreatedAtRoute(GetAdvertisementEndpointName, new { id = ad.Id }, ad.ToDetailsDto());
        }).RequireAuthorization().WithParameterValidation();

        // PUT    /ads/{id}
        app.MapPut("ads/{id}", async (int id, UpdateAdDto updatedAdDto, ClassifiedsContext dbContext, HttpContext httpContext) =>
        {
            var existingAd = await dbContext.Ads.FindAsync(id);
            if (existingAd is null) return Results.NotFound();

            int userId = int.Parse(httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            if (userId != existingAd.UserId && !httpContext.User.IsInRole("Admin")) return Results.Forbid();
            
            dbContext.Ads.Entry(existingAd).CurrentValues.SetValues(updatedAdDto.ToEntity(id, existingAd.UserId));
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        }).RequireAuthorization().WithParameterValidation();

        // DELETE /ads/{id}
        app.MapDelete("ads/{id}", async (int id, ClassifiedsContext dbContext, HttpContext httpContext) =>
        {
            var ad = await dbContext.Ads.FindAsync(id);
            if (ad is null) return Results.NotFound();
            
            var userId = int.Parse(httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            if (userId != ad.UserId && !httpContext.User.IsInRole("Admin")) return Results.Forbid();
            
            dbContext.Ads.Remove(ad);
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        }).RequireAuthorization();
        
        return app;
    }
}