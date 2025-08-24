namespace MinimalAPI;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Interfaces;
using MinimalAPI.Domain.Services;
using MinimalAPI.Domain.ViewModels;
using MinimalAPI.Infra.DB;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

public class Startup
{
    private string? key;
    IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        key = Configuration.GetSection("Jwt").GetValue<String>("key");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IVehicleService, VehicleService>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Add JWT Token this format: Bearer {token}"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        services.AddAuthorization();

        services.AddDbContext<MyContext>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString("mysql"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("mysql"))
                );
            }
        );
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        // IMPORTANTE: UseRouting() DEVE vir antes de UseEndpoints()
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            #region Routes

            endpoints.MapGet("/", () => Results.Json(new Home()))
                .AllowAnonymous().WithTags("Home");

            #region Administradores

            string GenerateTokenJWT(Admin admin)
            {
                if (String.IsNullOrEmpty(key)) return string.Empty;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>()
                {
                    new Claim("Email", admin.Email),
                    new Claim("Perfil", admin.Perfil),
                    new Claim(ClaimTypes.Role, admin.Perfil)
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }


            endpoints.MapPost("/admin/login", ([FromBody] LoginDTO loginDto, IAdminService adminService) =>
            {
                var admin = adminService.Login(loginDto);

                if (admin != null)
                {
                    var token = GenerateTokenJWT(admin);
                    return Results.Ok(token);
                }

                return Results.Unauthorized();
            }).AllowAnonymous().WithTags("Administrator");

            endpoints.MapPost("/admin", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
                {
                    var validation = new ValidationErrors(
                        messages: adminDTO.Validation()
                    );
                    if (validation.IsError)
                    {
                        return Results.BadRequest(validation);
                    }

                    Admin admin = new Admin()
                    {
                        Email = adminDTO.Email,
                        Password = adminDTO.Password,
                        Perfil = adminDTO.Perfil?.ToString() ?? PerfilEnum.EDITOR.ToString()
                    };
                    adminService.Insert(admin);

                    return Results.Created($"/admin/{admin.Id}", new AdminViewModel(admin));
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM" })
                .WithTags("Administrator");

            endpoints.MapGet("/admin/{id}", ([FromRoute] int id, IAdminService adminService) =>
                {
                    var admin = adminService.GetById(id);
                    if (admin == null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(new AdminViewModel(admin));
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM" })
                .WithTags("Administrator");

            endpoints.MapGet("/admin", ([FromQuery] int? page, IAdminService adminService) =>
                {
                    var admins = adminService.GetAll(page);
                    return Results.Ok(admins.Select(adm => new AdminViewModel(adm)));
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM" })
                .WithTags("Administrator");

            #endregion

            #region Veiculos

            endpoints.MapPost("/vehicle", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
                {
                    var validation = new ValidationErrors(
                        messages: vehicleDTO.Validation()
                    );
                    if (validation.IsError)
                    {
                        return Results.BadRequest(validation);
                    }

                    var vehicle = new Vehicle()
                    {
                        Name = vehicleDTO.Name,
                        Brand = vehicleDTO.Brand,
                        Year = vehicleDTO.Year
                    };
                    vehicleService.Insert(vehicle);
                    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM,EDITOR" })
                .WithTags("Vehicle");

            endpoints.MapGet("/vehicle", ([FromQuery] int? page, IVehicleService vehicleService) =>
                {
                    var vehicles = vehicleService.GetAll(page ?? 0);
                    return Results.Ok(vehicles);
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM,EDITOR" })
                .WithTags("Vehicle");

            endpoints.MapGet("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetById(id);
                    if (vehicle == null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(vehicle);
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM,EDITOR" })
                .WithTags("Vehicle");

            endpoints.MapPut("/vehicle/{id}",
                    ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
                    {
                        var validation = new ValidationErrors(
                            messages: vehicleDTO.Validation()
                        );
                        if (validation.Messages.Any())
                        {
                            return Results.BadRequest(validation);
                        }

                        var vehicleOld = vehicleService.GetById(id);
                        if (vehicleOld == null)
                        {
                            return Results.NotFound();
                        }

                        vehicleOld.Name = vehicleDTO.Name;
                        vehicleOld.Brand = vehicleDTO.Brand;
                        vehicleOld.Year = vehicleDTO.Year;

                        vehicleService.Update(vehicleOld);

                        return Results.Ok(vehicleOld);
                    })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM" })
                .WithTags("Vehicle");

            endpoints.MapDelete("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetById(id);
                    if (vehicle == null)
                    {
                        return Results.NotFound();
                    }

                    vehicleService.Delete(vehicle.Id);
                    return Results.Ok();
                })
                .RequireAuthorization(new AuthorizeAttribute() { Roles = "ADM" })
                .WithTags("Vehicle");

            #endregion

            #endregion
        });
    }
}