using L2CodePackagingAPI.Data;
using L2CodePackagingAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Database configuration
builder.Services.AddDbContext<PackagingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IPackagingService, PackagingService>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

if (!string.IsNullOrEmpty(jwtKey))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}

// Configurar para rodar como Windows Service
builder.Host.UseWindowsService();

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Packaging API - Loja do Seu Manoel",
        Version = "v1",
        Description = "API para automatização do processo de embalagem de pedidos"
    });

    // JWT Security Definition
    if (!string.IsNullOrEmpty(jwtKey))
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "L2CodePackagingAPI API v1");
        c.RoutePrefix = string.Empty;
    });
}

using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var applyMigrations = config.GetValue<bool>("ApplyMigrationsOnStartup");

    if (applyMigrations)
    {
        var context = scope.ServiceProvider.GetRequiredService<PackagingDbContext>();

        // Apaga o banco (opcional)
        context.Database.EnsureDeleted();

        //Garante que o banco de dados para o contexto exista.
        context.Database.EnsureCreated();

        // Aplica as Migrations e recria o banco corretamente
        context.Database.Migrate();

        // Caminho para o appsettings.json
        string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        // Lê o conteúdo atual do appsettings.json
        string json = File.ReadAllText(appSettingsPath);
        var jsonObj = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

        // Atualiza o valor
        jsonObj["ApplyMigrationsOnStartup"] = false;

        // Serializa e salva de volta no arquivo
        string updatedJson = JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(appSettingsPath, updatedJson);
    }
}

app.UseHttpsRedirection();

if (!string.IsNullOrEmpty(jwtKey))
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


