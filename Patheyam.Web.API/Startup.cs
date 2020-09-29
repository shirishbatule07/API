
namespace Patheyam.Web.Api
{
    using Lamar;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Storage.Data;
    using Patheyam.Web.Api.Filters;
    using Patheyam.Web.Api.Middleware;
    using Patheyam.Web.Api.Utils;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(Configuration.GetValue<string>("AllowedOrigins").Split(',')).AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddControllers(options => { options.Filters.Add(typeof(TrackActionPerformanceFilter)); });

            services.AddLogging();
            services.AddSingleton(Configuration);
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddHandlers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patheyam.Web.API", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Please enter into field the word 'Bearer' following by space and access token",
                    Scheme = "ApiKeyScheme"
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                   { key, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);
            });

            //Id 4 Authorization

            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            builder.AddJwtBearer(options =>
            {
                options.Authority = GetAuthority();
                options.RequireHttpsMetadata = false;
                options.Audience = Configuration.GetValue<string>("AppSettings:Audience");
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = e =>
                    {
                        Log.Warning(e.Exception.Message, "Authentication Failed!");
                        return Task.FromResult(e);
                    },
                    OnForbidden = e =>
                    {
                        //TODO: Log claims for more details, useful in role authorization 
                        Log.Warning("API access was forbidden!");
                        return Task.FromResult(e);
                    }
                };
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;
            var policyCollection = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .RemoveServerHeader()
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddObjectSrc().None();
                    builder.AddFormAction().Self();
                    builder.AddFrameAncestors().None();
                });

            app.UseSecurityHeaders(policyCollection);
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandler>();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins); });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                var basePath = System.Environment.GetEnvironmentVariable("ASPNETCORE_APPL_PATH");
                basePath = basePath == null
                    ? ""
                    : (basePath.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? basePath : $"{basePath}/");
                c.SwaggerEndpoint($"{basePath}swagger/v1/swagger.json", "Patheyam V1");
                c.RoutePrefix = ""; //serve the Swagger UI at the app's root
            });
        }

        public void ConfigureContainer(ServiceRegistry services)
        {
            services.Scan(s =>
            {
                s.ExcludeType<IConnectionFactory>();
                s.Assembly(Assembly.GetExecutingAssembly());
                s.Assembly(Assembly.Load("Patheyam.Domain"));
                s.Assembly(Assembly.Load("Patheyam.Engine"));
                s.Assembly(Assembly.Load("Patheyam.Storage"));
                s.WithDefaultConventions();
                s.LookForRegistries();
            });
        }

        private string GetAuthority()
        {
            return Configuration.GetValue<string>("AppSettings:IdentityServerUri") ??
                   throw new InvalidOperationException(
                       "AppSettings:IdentityServerUri could not be found in the config.");
        }
    }
}
