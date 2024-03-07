using AutoMapper;
using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TrigonosEnergyWebAPI.Middleware;
using WebApi.Dto;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BanksDBContext>(Options => Options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el prefijo Bearer en el campo texto abajo.\n\nEjemplo: \"Bearer {Token}\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<DataBanks>();
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("APIAuth", new OpenApiInfo()
    {
        Title = "API Prueba Finix",
        Version = "v1",
        Description = "Backend .NET CORE 7",
        Contact = new OpenApiContact()
        {
            Email = "ejoocontactos@gmail.com",
            Name = "Elvis Olmedo",

        },
        License = new OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
        }

    });
    options.SwaggerDoc("APIBanks", new OpenApiInfo()
    {
        Title = "API Prueba Finix",
        Version = "v1",
        Description = "Backend .NET CORE 7",
        Contact = new OpenApiContact()
        {
            Email = "ejoocontactos@gmail.com",
            Name = "Elvis Olmedo",

        },
        License = new OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});
var key = Encoding.ASCII.GetBytes("Clave secreta privada prueba finixGroup%");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();
//if(app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
app.UseCors("CorsRule");
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>();
    ////MIGRACION DB
    try
    {
        var identityContext = services.GetRequiredService<BanksDBContext>();
        await identityContext.Database.MigrateAsync();
    }
    catch (Exception e)
    {

        var loggerError = logger.CreateLogger<Program>();
        loggerError.LogError(e, "Error al migrar");
    }
    //
    // CARGAR DATOS A TRAVEZ DE API
    var bankDataService = services.GetRequiredService<DataBanks>();
    await bankDataService.LoadDataFromApiAsync().ConfigureAwait(false);
    //
}
//
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/APIAuth/swagger.json", "API Auth");
    options.SwaggerEndpoint("/swagger/APIBanks/swagger.json", "API Banks");
    options.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();