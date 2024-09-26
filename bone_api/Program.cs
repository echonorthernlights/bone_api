using bone_api.Authentication;
using bone_api.Middlewares.bone_api.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add CORS policy to allow requests from your React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin() // Your React app's URL
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    x =>
    {
        x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
        {
            Description = "Api key to secure the API",
            Type = SecuritySchemeType.ApiKey,
            Name = AuthConfig.ApiKeyHeader,
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme"
        });
        var scheme = new OpenApiSecurityScheme()
        {
            Reference = new OpenApiReference()
            {
                Type = ReferenceType.SecurityScheme,
                Id = "ApiKey"
            },
            In = ParameterLocation.Header
        };
        var requirements = new OpenApiSecurityRequirement(){
            {scheme, new List<string>()}
        };
        x.AddSecurityRequirement(requirements);
    }
);


builder.Services.AddScoped<ApiKeyAuthenticationFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Add this line
//app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
