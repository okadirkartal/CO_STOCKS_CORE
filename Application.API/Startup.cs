using System;
using System.Threading.Tasks;
using Application.API.Filter;
using Application.API.Jwt;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.IdentityModel.Tokens;

namespace Application.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BuildAppSettingsProvider();
        }

        private void BuildAppSettingsProvider()
        {
            AppSettingsProvider.MongoDbConnectionString =
                Configuration.GetSection("MongoDbConnection:connectionString").Value;
            AppSettingsProvider.MongoDbDatabase = Configuration.GetSection("MongoDbConnection:Database").Value;
        }

        private IConfiguration Configuration { get; }

        private CorsPolicy GenerateCorsPolicy()
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();
            return corsBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => { options.AddPolicy("AllowOrigin", GenerateCorsPolicy()); });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("Authentication:Issuer").Value,
                        ValidAudience = Configuration.GetSection("Authentication:Audience").Value,
                        IssuerSigningKey =
                            JwtSecurityKey.Create(Configuration.GetSection("Authentication:SecurityKey").Value)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"OnAuthenticationFailed {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine($"OnTokenValidated : {context.SecurityToken}");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Member", policy => policy.RequireClaim("MembershipId"));
            }); 
            services.AddResponseCaching();
            services.AddMemoryCache();
            
            
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<MvcOptions>(x => x.Conventions.Add(new ModelStateValidatorConvension()));
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockSettingsRepository, StockSettingsRepository>();
            
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseResponseCaching();
            app.UseMvc();
        }
    }
}