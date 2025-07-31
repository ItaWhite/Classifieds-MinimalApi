using Classifieds.MinimalApi.Data;
using Classifieds.MinimalApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
builder.Services.AddNpgsql<ClassifiedsContext>(connectionString);
    
var app = builder.Build();

app.MigrateDb();
app.MapAdsEndpoints();
app.MapCategoriesEndpoints();

app.Run();