using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.MappingProfile;
using Nowcfo.Application.Repository;
using Nowcfo.Application.Services.CurrentUserService;
using Nowcfo.Application.Services.EmailService;
using Nowcfo.Application.Services.JwtService;
using Nowcfo.Application.Services.RoleService;
using Nowcfo.Application.Services.UserAuthService;
using Nowcfo.Application.Services.UserService;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;
using Nowcfo.Infrastructure.Data;
using Nowcfo.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nowcfo.API
{
    public class Startup
    {

        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString =  Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
            //services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);
            //services.AddControllers().AddJsonOptions(options =>
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            var origins = Configuration["CorsUrl"].Split(",");
            services.AddCors(options =>
            {
                options.AddPolicy("BasePolicy",
                builder =>
                {
                    builder

                    .AllowAnyOrigin()
                     .AllowAnyMethod()
                    .AllowAnyHeader();
                   
                });

            });
            
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = Configuration["AuthSettings:Issuer"];
                options.Audience = Configuration["AuthSettings:Audience"];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            services.AddAuthentication(auth =>
            {


                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = Configuration["AuthSettings:Issuer"];
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthSettings:Audience"],
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = _signingKey,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.SaveToken = true;
            });
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            services.AddAuthorization();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IUserAuthService, UserAuthService>();
            
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IJwtService, JwtService>();
           
            services.AddTransient<IMailService, SendGridMailService>();
            services.AddTransient<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IDapperRepository, DapperRepository>();



            //Mapper
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Now CFO API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
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

            services.AddMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NowCFO OrgManager API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseCors("BasePolicy");
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
