using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using APITaxiV2.Services;

var builder = WebApplication.CreateBuilder(args);

var CorsRules = "CorsRules";

builder.Services.AddCors(option => 
    option.AddPolicy(name: CorsRules, 
        builder => 
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    ) 
);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json");
var secretkey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    // Política para Empresa (id_rol = 3)
    options.AddPolicy("Empresa", policy =>
        policy.RequireClaim("id_rol", "3"));

    // Política para Secretaria (id_rol = 4)
    options.AddPolicy("Secretaria", policy =>
        policy.RequireClaim("id_rol", "4"));

    // Política para Admin (id_rol = 5)
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("id_rol", "5"));

    // Política para Conductor (id_rol = 1)
    options.AddPolicy("Conductor", policy =>
        policy.RequireClaim("id_rol", "1"));

    // Política para Secretaria o Admin (id_rol = 4 o 5)
    options.AddPolicy("Secretaria-Admin", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("id_rol", "4") || // Secretaria
            context.User.HasClaim("id_rol", "5") // Admin
        ));

    // Política para Secretaria o Empresa (id_rol = 4 o 3)
    options.AddPolicy("Secretaria-Empresa", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("id_rol", "4") || // Secretaria
            context.User.HasClaim("id_rol", "3")    // Empresa
        ));

    // Política para Empresa o Admin (id_rol = 3 o 5)
    options.AddPolicy("Empresa-Admin", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("id_rol", "3") || // Empresa
            context.User.HasClaim("id_rol", "5") // Admin
        ));

    // Política para Empresa o Conductor (id_rol = 3 o 1 o 6)
    options.AddPolicy("Empresa-Conductor", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("id_rol", "3") || // Empresa
            context.User.HasClaim("id_rol", "1")  ||// Conductor
            context.User.HasClaim("id_rol", "6") // Conductor y Propietario
        ));


});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "API para la Gestión de Taxis",
            Version = "v2.0",
            Description = "En esta API se encuentran los datos del sistema de información para la gestión y control de Taxis que circulan por Facatativá - Cundinamarca",
            Contact = new OpenApiContact
            {
                Name = "Soporte de Sistema",
                Email = "soportedesistemafaca@gmail.com",
                Url = new Uri("https://www.taxisfacatativa.somee.com"),
            },
        });
    });

builder.Services.AddScoped<IEmail, EmailServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(CorsRules);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
