using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using F1WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using F1WebAPI.Context;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using F1WebAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using F1WebAPI.Services;

namespace F1WebAPI
{
public class Startup
{
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_MyAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder => 
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    //allow any method will allow get, delete, post etc
                    

                 
                    }
                );
            }
            );
            services.AddDbContext<F1Context>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("F1Context")));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddScoped<IDataContext, F1Context>();

            services.AddTransient<DriverResultService>();
            services.AddTransient<DriverService>();
            services.AddTransient<RaceResultService>();
            services.AddTransient<TeamService>();
            services.AddTransient<TrackService>();

            services.AddScoped<IRepositoryBase<Team>, RepositoryBase<Team>>();
            services.AddScoped<IRepositoryBase<Driver>, RepositoryBase<Driver>>();
            services.AddScoped<IRepositoryBase<Track>, RepositoryBase<Track>>();
            services.AddScoped<IRepositoryBase<DriverResult>, RepositoryBase<DriverResult>>();
            services.AddScoped<IRepositoryBase<RaceResult>, RepositoryBase<RaceResult>>();

            services.AddControllers();


            ///decode the secret using base 64
            var clientSecret = Base64UrlTextEncoder.Decode(Configuration.GetSection("Authentication").GetSection("ClientSecret").Value);
          

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = Configuration.GetSection("Authentication").GetSection("AuthenticationIssuer").Value,
                    ValidAudience = Configuration.GetSection("Authentication").GetSection("Client_Id").Value ,
              
                    IssuerSigningKey = new SymmetricSecurityKey(clientSecret)                    
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


            ////this is used to allow the api to talk to the website. cos they are on different addresses 

            app.UseCors(
                  MyAllowSpecificOrigins
               );




            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

 

        }
    }
}
