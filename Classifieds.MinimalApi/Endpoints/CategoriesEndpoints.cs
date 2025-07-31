using Classifieds.MinimalApi.Data;
using Classifieds.MinimalApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Classifieds.MinimalApi.Endpoints;

public static class CategoriesEndpoints
{
    public static WebApplication MapCategoriesEndpoints(this WebApplication app)
    {
        // GET    /categories
        app.MapGet("/categories", async (ClassifiedsContext dbContext) =>
        {
            var result = await dbContext.Categories.Select(c => c.ToDto()).AsNoTracking().ToListAsync();
            return Results.Ok(result);
        });
        
        return app;
    }
}