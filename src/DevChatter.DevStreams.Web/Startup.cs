using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Infra.Dapper;
using DevChatter.DevStreams.Infra.Dapper.Services;
using DevChatter.DevStreams.Infra.Dapper.TypeHandlers;
using DevChatter.DevStreams.Infra.Db.Migrations;
using DevChatter.DevStreams.Infra.Twitch;
using DevChatter.DevStreams.Web.Data;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace DevChatter.DevStreams.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<DatabaseSettings>(
                Configuration.GetSection("ConnectionStrings"));

            services.Configure<TwitchSettings>(
                Configuration.GetSection("TwitchSettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 1;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(
                    builder => builder
                        .AddSqlServer()
                        .WithGlobalConnectionString(Configuration.GetConnectionString("DefaultConnection"))
                        .ScanIn(typeof(CreateTagsTable).Assembly).For.Migrations());

            SqlMapper.AddTypeHandler(InstantHandler.Default);
            SqlMapper.AddTypeHandler(LocalTimeHandler.Default);


            services.AddScoped<IStreamSessionService, DapperSessionLookup>();
            services.AddScoped<IScheduledStreamService, ScheduledStreamService>();
            services.AddTransient<ITagSearchService, TagSearchService>();
            services.AddTransient<ICrudRepository, DapperCrudRepository>();
            services.AddTransient<IChannelSearchService, ChannelSearchService>();
            services.AddTransient<IChannelAggregateService, ChannelAggregateService>();
            services.AddTransient<ITwitchService, TwitchService>();

            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IMigrationRunner migrationRunner, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            InitializeDatabase(app, migrationRunner);

            SetUpDefaultUsersAndRoles(userManager, roleManager).Wait();

            app.UseAuthentication();

            app.UseMvc();

        }

        private async Task SetUpDefaultUsersAndRoles(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            const string roleName = "Administrator";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var identityRole = new IdentityRole(roleName);
                var roleCreateResult = await roleManager.CreateAsync(identityRole);
            }

            const string defaultUserAccountName = "chatter1@example.com"; // TODO: Pull from Config
            const string defaultUserPassword = "Passw0rd!"; // TODO: Pull from Config
            var usersInRole = (await userManager.GetUsersInRoleAsync(roleName));
            if (!usersInRole.Any() 
                && await userManager.FindByEmailAsync(defaultUserAccountName) == null)
            {
                var user = new IdentityUser(defaultUserAccountName);
                user.Email = defaultUserAccountName;
                
                var result = await userManager.CreateAsync(user, defaultUserPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        private void InitializeDatabase(IApplicationBuilder app, IMigrationRunner migrationRunner)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
            migrationRunner.MigrateUp();
        }
    }
}
