using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;

namespace DevChatter.DevStreams.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ScheduledStream> ScheduledStream { get; set; }
        public DbSet<StreamSession> StreamSessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpChannels(modelBuilder);
            SetUpScheduledStream(modelBuilder);
            SetUpStreamSession(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void SetUpStreamSession(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StreamSession>(builder =>
            {
                builder.Property(x => x.UtcStartTime)
                    .HasConversion(
                        x => x.ToUnixTimeTicks(),
                        x => Instant.FromUnixTimeTicks(x))
                    .IsRequired();
                builder.Property(x => x.UtcEndTime)
                    .HasConversion(
                        x => x.ToUnixTimeTicks(),
                        x => Instant.FromUnixTimeTicks(x))
                    .IsRequired();
                builder.Property("ScheduledStreamId")
                    .IsRequired();
                builder.Property(x => x.TzdbVersionId)
                    .IsRequired();
            });
        }

        private static void SetUpScheduledStream(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduledStream>(builder =>
            {
                builder.Property(x => x.DayOfWeek)
                    .HasConversion(
                        x => x.ToString(),
                        x => (IsoDayOfWeek)Enum.Parse(typeof(IsoDayOfWeek), x))
                    .IsRequired();
                builder.Property(x => x.LocalStartTime)
                    .HasConversion(
                        x => x.TickOfDay,
                        x => LocalTime.FromTicksSinceMidnight(x))
                    .IsRequired();
                builder.Property(x => x.LocalEndTime)
                    .HasConversion(
                        x => x.TickOfDay,
                        x => LocalTime.FromTicksSinceMidnight(x))
                    .IsRequired();
                builder.Property("ChannelId")
                    .IsRequired();
            });
        }

        private void SetUpChannels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>(builder =>
                {
                    builder.Property(x => x.Name)
                        .IsRequired();
                    builder.Property(x => x.Uri)
                        .IsRequired();
                    builder.Property(x => x.TimeZoneId)
                        .IsRequired();
                    builder.Property(x => x.CountryCode)
                        .IsRequired();
                });
        }
    }
}
