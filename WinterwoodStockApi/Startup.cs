using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WinterwoodStock.Library.Interfaces.Repositories;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Repositories;
using WinterwoodStock.Library.Services;

namespace WinterwoodStockApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<WinterwoodDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("BatchDBContextConnectionString")));

            services.AddControllers();

            // services
            services.AddTransient<IWinterwoodStockService, WinterwoodStockService>();
            services.AddTransient<IAuthService, AuthService>();

            // repositories
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBatchRepository, BatchRepository>();
            services.AddTransient<IStockRepository, StockRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Winterwood Stock API";
                    document.Info.Description = "Stock/Batch update details";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Kaan Torun",
                        Email = "KaanTorun@digitedge.co.uk",
                        Url = "https://www.digitedge.co.uk"
                    };
                };
            });

            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });*/

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["AuthOptions:Domain"],
                        ValidAudience = _configuration["AuthOptions:Domain"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:SecurityKey"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }


            List<string> urlsToIgnore = new List<string>();
            urlsToIgnore.Add("http://localhost:4200");
            urlsToIgnore.Add("https://winterwoodbatchstocksystem.azurewebsites.net");
            urlsToIgnore.Add("https://winterwoodapi.azurewebsites.net");

            //allow cors requests
            app.UseCors(builder => builder.WithOrigins(urlsToIgnore.ToArray()).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("v1/swagger.json", "Winterwoodstock API V1");
            //});

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            //Swashbuckle.AspNetCore.Swagger.SwaggerOptions options = new Swashbuckle.AspNetCore.Swagger.SwaggerOptions { };
            //app.UseSwagger(options);

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = string.Empty;
            //});

            //app.UseSwagger(c =>
            //{
            //    c.SerializeAsV2 = true;
            //});

            app.UseSwaggerUi3();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            /*app.UseSwagger(c =>
            {
                c.SerializeAsV2 = false;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });*/

            app.UseHttpsRedirection();
            //app.UseMiddleware<SetCurrentUserMiddleware>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
