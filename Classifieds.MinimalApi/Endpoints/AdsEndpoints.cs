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
            var result = await dbContext.Ads.Include(ad => ad.Category).Select(ad => ad.ToSummaryDto())
                .AsNoTracking().ToListAsync();
            
            return Results.Ok(result);
        });

        // GET    /ads/{id}
        app.MapGet("ads/{id}", async (int id, ClassifiedsContext dbContext) =>
        {
            var ad = await dbContext.Ads.FindAsync(id);
            
            return ad is null ? Results.NotFound() : Results.Ok(ad.ToDetailsDto());
        }).WithName(GetAdvertisementEndpointName);

        // POST   /ads
        app.MapPost("ads", async (CreateAdDto newAdDto, ClassifiedsContext dbContext) =>
        {
            Ad ad = newAdDto.ToEntity();
            await dbContext.Ads.AddAsync(ad);
            await dbContext.SaveChangesAsync();
            
            return Results.CreatedAtRoute(GetAdvertisementEndpointName, new { id = ad.Id }, ad.ToDetailsDto());
        }).WithParameterValidation();

        // PUT    /ads/{id}
        app.MapPut("ads/{id}", async (int id, UpdateAdDto updatedAdDto, ClassifiedsContext dbContext) =>
        {
            var existingAd = await dbContext.Ads.FindAsync(id);
            
            if (existingAd is null) return Results.NotFound();
            
            dbContext.Ads.Entry(existingAd).CurrentValues.SetValues(updatedAdDto.ToEntity(id));
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        }).WithParameterValidation();

        // DELETE /ads/{id}
        app.MapDelete("ads/{id}", async (int id, ClassifiedsContext dbContext) =>
        {
            await dbContext.Ads.Where(ad => ad.Id == id).ExecuteDeleteAsync();
            
            return Results.NoContent();
        });
        
        return app;
    }
}