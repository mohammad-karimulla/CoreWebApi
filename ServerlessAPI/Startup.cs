using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerlessAPI.Models.Hubs;

namespace ServerlessAPI
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
                    options.WithOrigins("http://localhost:4200", 
                                        "http://deployangulars3bucket.s3-website.eu-west-2.amazonaws.com/")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                );
            });

            // Configure AWS services
            AWSOptions awsOptions = Configuration.GetAWSOptions();
            awsOptions.Credentials = new BasicAWSCredentials(
                Configuration.GetValue<string>("AWS-Credential:AccessKey"),
                Configuration.GetValue<string>("AWS-Credential:SecretKey")
                );
            awsOptions.Region = RegionEndpoint.EUWest2;
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();

            services.AddControllers();

            // SignalR
            services.AddSignalR();
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
                endpoints.MapHub<SNSHub>("/snsHub");
            });
        }
    }
}
