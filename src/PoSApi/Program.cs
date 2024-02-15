using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PoSApi.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtBearerOptions", options));

var swaggerOptions = builder.Configuration.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>()!;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var scheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(swaggerOptions.AuthorizationUrl),
                TokenUrl = new Uri(swaggerOptions.TokenUrl)
            }
        },
        Type = SecuritySchemeType.OAuth2
    };

    options.AddSecurityDefinition("OAuth", scheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
            }, 
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("PoSApi.Resources.index.html");
    options.OAuthClientId(swaggerOptions.ClientId);
    options.OAuthScopes(swaggerOptions.Scopes);
    options.OAuthUsePkce();
    options.EnablePersistAuthorization();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
