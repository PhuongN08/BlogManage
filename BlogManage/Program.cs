using BlogManage.AutoMapper;
using BlogManage.Models;
using BlogManage.Services.AdminServices;
using BlogManage.Services.AuthenServices;
using BlogManage.Services.ManagerServices;
using BlogManage.Services.PublicServices;
using BlogManage.Services.WriterServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace BlogManage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger + JWT Bearer Setup
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ALS API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer abcdef12345'",
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
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(ApplicationMapper));

            // DbContext
            builder.Services.AddDbContext<BlogManageContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            // JWT Authentication
            var jwtConfig = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = true;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,

                      ValidIssuer = jwtConfig["Issuer"],
                      ValidAudience = jwtConfig["Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(jwtConfig["SecretKey"] ?? throw new Exception("JWT SecretKey not configured"))),

                      RoleClaimType = ClaimTypes.Role
                  };
              });

            // Role-based Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
                options.AddPolicy("WriterOnly", policy => policy.RequireRole("Writer"));
            });

            // Dependency Injection for services
            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IBlogManageService, BlogManageService>();
            builder.Services.AddScoped<ICategoryService, CategoryServices>();
            builder.Services.AddScoped<IAuthenService, AuthenSerivce>();
            builder.Services.AddScoped<IAccountListServices, AccountListServices>();
            builder.Services.AddScoped<IWriterBlogService, WriterBlogServices>();
            builder.Services.AddScoped<IMyProfileService, MyProfileServices>();
            builder.Services.AddScoped<ICommentService, CommentServices>();
            builder.Services.AddScoped<IReportService, ReportServices>();
            builder.Services.AddScoped<JwtTokenService>();

            // Identity & Password hasher
            builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ALS API v1");
                });
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("CORSPolicy");

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}