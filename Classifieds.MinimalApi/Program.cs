using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Classifieds.MinimalApi.Auth;
using Classifieds.MinimalApi.Data;
using Classifieds.MinimalApi.Endpoints;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
builder.Services.AddNpgsql<ClassifiedsContext>(connectionString);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuer = true,
        ValidateAudience = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenGenerator>();

var app = builder.Build();

app.MigrateDb();

app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapAdsEndpoints();
app.MapCategoriesEndpoints();

app.Run();