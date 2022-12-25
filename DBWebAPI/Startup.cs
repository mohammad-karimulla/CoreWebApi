using DBWebAPI.Models;
using DBWebAPI.Models.CodeFirst;
using DBWebAPI.Models.DataFirst;
using DBWebAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace DBWebAPI
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
            // Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowAngularAppUrl", options =>
                    // options.AllowAnyOrigin()
                    options.WithOrigins("http://localhost:4200", 
                                        "http://deployangulars3bucket.s3-website.eu-west-2.amazonaws.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials());
            });

            // JSON Serializer
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            );

            // DI into Controllers
            services.AddSingleton<IFileOperations, FileOperations>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddControllers();

            // SignalR
            services.AddSignalR();

            // Connection to 'OrgCodeFirst' Database
            services.AddDbContext<OrgCodeFirstContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("OrganizationAppCodeFirstCon")));

            // Connection to 'OrgDataFirst' Database present in Azure Portal
            services.AddDbContext<OrgDataFirstContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("OrganizationAzureDataFirstCon")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Adds a CORS middleware to allow cross domain requests
            app.UseCors("AllowAngularAppUrl");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // To enable static files serving for the current request path
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
            //    RequestPath = "/Photos"
            //});
        }
    }
}
