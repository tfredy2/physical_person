using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PhysicalPerson.Configurations;
using PhysicalPerson.Data;
using PhysicalPerson.General;
using PhysicalPerson.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalPerson
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PhysicalPerson",
                    Version = "v1",
                    Description = "Api desarrollada para crear, modificar, eliminar y actualizar personas fisicas, solo puede ser realizar cambios una persona que este registrada."
                });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Auth",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                          In = ParameterLocation.Header
                        },
                        new List<string>()
                      }
                }   );
            });
            

            services.AddCors(options =>
            {
                options.AddPolicy("*",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            ServiceExtensions.Configuracion = Configuration;
            //var keyToken = ServiceExtensions.Configuracion.GetSection("JWT").GetSection("JWTKEY").Value;
            var keyToken = ServiceExtensions.Configuracion.GetSection("AppSettingsToken");
            services.Configure<AppSettingsToken>(keyToken);

            //JWT
            AppSettingsToken appSettings = keyToken.Get<AppSettingsToken>();
            var key = Encoding.ASCII.GetBytes(appSettings.KeyToken);
            services.AddAuthentication(e =>
            {
                e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(e => {
                    e.RequireHttpsMetadata = false;
                    e.SaveToken = true;
                    e.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddTransient<ICrud, Crud>();
            services.AddTransient<IManejoErrores, ManejoErrores>();
            services.AddTransient<ILogin, Login>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhysicalPerson v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("*");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
