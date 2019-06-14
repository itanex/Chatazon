using Chatazon.Data;
using Chatazon.Models;
using Chatazon.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chatazon
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<Data.ApplicationDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("SQLServer"));
                options.UseMySql(Configuration.GetConnectionString("MySQL"));
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();

            JsonSerializer serializer = JsonSerializer.Create(settings);

            services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                provider => serializer,
                ServiceLifetime.Transient)
            );

            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });

            services.AddMvc();

            // Add application services.
            services.AddTransient<ChatService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            DbInitializer.Initialize(dbContext);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvcWithDefaultRoute();

            app.UseWebSockets();
            app.UseSignalR();
        }
    }
}
