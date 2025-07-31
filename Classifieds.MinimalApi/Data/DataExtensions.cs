using Microsoft.EntityFrameworkCore;

namespace Classifieds.MinimalApi.Data;

public static class DataExtensions
{
    public static WebApplication MigrateDb(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ClassifiedsContext>();
        dbContext.Database.Migrate();
        
        return app;
    }
}