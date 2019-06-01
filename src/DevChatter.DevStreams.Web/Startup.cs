using Dapper;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Twitch;
using DevChatter.DevStreams.Infra.Dapper;
using DevChatter.DevStreams.Infra.Dapper.Services;
using DevChatter.DevStreams.Infra.Dapper.TypeHandlers;
using DevChatter.DevStreams.Infra.Db.Migrations;
using DevChatter.DevStreams.Infra.GraphQL;
using DevChatter.DevStreams.Infra.Twitch;
using DevChatter.DevStreams.Web.Caching;
using DevChatter.DevStreams.Web.Data;
using FluentMigrator.Runner;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DevChatter.DevStreams.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _env;

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

            services.Configure<CacheSettings>(
                Configuration.GetSection(nameof(CacheSettings)));

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

            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, UserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();

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

            services.AddScoped<ITagService, TagService>();

            services.AddScoped<ITwitchApiClient, TwitchApiClient>();

            services.AddTransient<ITagSearchService, TagSearchService>();
            services.AddTransient<ICrudRepository, DapperCrudRepository>();
            services.AddScoped<IChannelRepository, DapperChannelRepository>();
            services.AddTransient<IChannelAggregateService, ChannelAggregateService>();

            services.AddMemoryCache();

            services.AddScoped(typeof(TwitchStreamService));
            CachedTwitchStreamService TwitchServiceFactory(IServiceProvider x) => new CachedTwitchStreamService((ITwitchStreamService) x.GetService(typeof(TwitchStreamService)), (IMemoryCache)x.GetService(typeof(IMemoryCache)), (IOptions<CacheSettings>)x.GetService(typeof(IOptions<CacheSettings>)));
            services.AddScoped<ITwitchStreamService, CachedTwitchStreamService>(TwitchServiceFactory);

            services.AddScoped<ChannelSearchService>();
            CachedChannelSearchService ChannelSearchServiceFactory(IServiceProvider x) => new CachedChannelSearchService((IChannelSearchService) x.GetService(typeof(ChannelSearchService)), (IMemoryCache)x.GetService(typeof(IMemoryCache)), (IOptions<CacheSettings>)x.GetService(typeof(IOptions<CacheSettings>)));
            services.AddScoped<IChannelSearchService, CachedChannelSearchService>(ChannelSearchServiceFactory);

            services.AddScoped<ITwitchChannelService, TwitchChannelService>();

            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddTransient<IChannelPermissionsService,
                ChannelPermissionsService>();

            RegisterGraphQL.Configure(services, _env);

            services
                .AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/My");
                    options.Conventions.AuthorizeFolder("/Manage",
                        "RequireAdministratorRole");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole",
                    policy => policy.RequireRole("Administrator"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IMigrationRunner migrationRunner, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<InitialSettings> initialSettings)
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

            SetUpDefaultUsersAndRoles(userManager, roleManager, initialSettings.Value)
                .Wait();

            app.UseAuthentication();

            app.UseGraphQL<DevStreamsSchema>(path: "/graphql");
            if (env.IsDevelopment())
            {
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
                {
                    Path = "/ui/playground"
                });
            }

            app.UseMvc();
        }

        private async Task SetUpDefaultUsersAndRoles(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, InitialSettings settings)
        {
            const string roleName = "Administrator";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var identityRole = new IdentityRole(roleName);
                var roleCreateResult = await roleManager.CreateAsync(identityRole);
            }

            var usersInRole = (await userManager.GetUsersInRoleAsync(roleName));
            if (!usersInRole.Any() 
                && await userManager.FindByEmailAsync(settings.AdminUsername) == null)
            {
                var user = new IdentityUser(settings.AdminUsername);
                user.Email = settings.AdminUsername;
                
                var result = await userManager.CreateAsync(user, settings.AdminPassword);

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
