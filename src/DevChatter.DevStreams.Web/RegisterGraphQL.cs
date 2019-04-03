using DevChatter.DevStreams.Infra.GraphQL;
using DevChatter.DevStreams.Infra.GraphQL.Types;
using GraphQL;
using GraphQL.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DevChatter.DevStreams.Web
{
    public static class RegisterGraphQL
    {
        public static void Configure(IServiceCollection services, IHostingEnvironment env)
        {
            // For GraphQL
            services.AddScoped<IDependencyResolver>(s =>
                new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<DevStreamsSchema>();
            services.AddScoped<DevStreamsQuery>();
            services.AddScoped<ChannelType>();
            services.AddScoped<ScheduledStreamType>();
            services.AddScoped<TagType>();
            services.AddScoped<TwitchChannelType>();
            services.AddScoped<StreamSessionType>();
            services.AddScoped<IsoDayOfWeekGraphType>();
            services.AddScoped<LocalTimeGraphType>();
            services.AddScoped<InstantGraphType>();

            services.AddGraphQL(options =>
            {
                options.ExposeExceptions = env.IsDevelopment();
            })
            .AddGraphTypes(ServiceLifetime.Scoped)
            .AddDataLoader();
        }
    }
}
