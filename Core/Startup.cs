using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.PostgreSql;


namespace Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            Configuration = configuration;
            PFE.SMSNotification.Utility.Config.DIRECTORY_TRACE = configuration["trace:DIRECTORY_TRACE"];
            PFE.SMSNotification.Utility.Config.TRACE_LEVEL = configuration["trace:TRACE_LEVEL"];
            PFE.SMSNotification.Utility.Config.CONNECTION_STRING = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddHangfire(configuration => configuration
                .UsePostgreSqlStorage(connectionString));

            // Add the Hangfire server
            services.AddHangfireServer();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ...

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseHangfireServer();

            app.UseHangfireDashboard(); // Use the Hangfire Dashboard

            // Schedule the job to process the SMS queue
            RecurringJob.AddOrUpdate(() => ProcessSmsQueue(), Cron.Minutely);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public void ProcessSmsQueue()
        {
            // Implement your logic to process the SMS queue here
            // This method will be executed on the schedule defined in the Configure method
        }
    }
}
