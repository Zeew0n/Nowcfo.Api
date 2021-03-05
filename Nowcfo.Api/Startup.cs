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
using System.Text;

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
            services.AddControllers();

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


            //Mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen();

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StreamHarbor API V1");
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
