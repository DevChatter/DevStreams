
namespace DevChatter.DevStreams.Infra.Dapper.Crud
{
  using global::Dapper;
  using NodaTime;

  using System.Data;
  using System.Linq;
  using System.Threading.Tasks;

  static partial class Crud
  {
    public class ScheduledStreams
    {
      public static async Task Create(
          IDbConnection connection
        , int id
        , int channelId
        , string timeZoneId
        , IsoDayOfWeek dayOfWeek
        , LocalTime localStartTime
        , LocalTime localEndTime
        )
      {
        var sql = @"INSERT INTO ScheduledStreams(Id, ChannelId, TimeZoneId, DayOfWeek, LocalStartTime, LocalEndTime) VALUES(@Id, @ChannelId, @TimeZoneId, @DayOfWeek, @LocalStartTime, @LocalEndTime)";
        await connection.ExecuteAsync(
            sql
          , new ScheduledStreams
            {
              Id = id,
              ChannelId = channelId,
              TimeZoneId = timeZoneId,
              DayOfWeek = dayOfWeek,
              LocalStartTime = localStartTime,
              LocalEndTime = localEndTime,
            });
      }

      public static async Task<ScheduledStreams[]> Read(
          IDbConnection connection
        , int id
        )
      {
        var sql = @"SELECT Id, ChannelId, TimeZoneId, DayOfWeek, LocalStartTime, LocalEndTime FROM ScheduledStreams WHERE Id = @Id";
        return (await connection.QueryAsync<ScheduledStreams>(
            sql
          , new 
            {
              Id = id,
            })).ToArray();
      }

      public static async Task Update(
          IDbConnection connection
        , int id
        , int channelId
        , string timeZoneId
        , IsoDayOfWeek dayOfWeek
        , LocalTime localStartTime
        , LocalTime localEndTime
        )
      {
        var sql = @"UPDATE ScheduledStreams SET ChannelId = @ChannelId, TimeZoneId = @TimeZoneId, DayOfWeek = @DayOfWeek, LocalStartTime = @LocalStartTime, LocalEndTime = @LocalEndTime WHERE Id = @Id";
        await connection.ExecuteAsync(
            sql
          , new ScheduledStreams
            {
              Id = id,
              ChannelId = channelId,
              TimeZoneId = timeZoneId,
              DayOfWeek = dayOfWeek,
              LocalStartTime = localStartTime,
              LocalEndTime = localEndTime,
            });
      }

      public static async Task Delete(
          IDbConnection connection
        , int id
        )
      {
        var sql = @"DELETE ScheduledStreams WHERE Id = @Id";
        await connection.ExecuteAsync(
            sql
          , new 
            {
              Id = id,
            });
      }

      public int Id { get; set; }
      public int ChannelId { get; set; }
      public string TimeZoneId { get; set; }
      public IsoDayOfWeek DayOfWeek { get; set; }
      public LocalTime LocalStartTime { get; set; }
      public LocalTime LocalEndTime { get; set; }
    }

    public class StreamSession
    {
      public static async Task Create(
          IDbConnection connection
        , int id
        , int channelId
        , int scheduledStreamId
        , Instant utcStartTime
        , Instant utcEndTime
        , string tzdbVersionId
        )
      {
        var sql = @"INSERT INTO StreamSession(Id, ChannelId, ScheduledStreamId, UtcStartTime, UtcEndTime, TzdbVersionId) VALUES(@Id, @ChannelId, @ScheduledStreamId, @UtcStartTime, @UtcEndTime, @TzdbVersionId)";
        await connection.ExecuteAsync(
            sql
          , new StreamSession
            {
              Id = id,
              ChannelId = channelId,
              ScheduledStreamId = scheduledStreamId,
              UtcStartTime = utcStartTime,
              UtcEndTime = utcEndTime,
              TzdbVersionId = tzdbVersionId,
            });
      }

      public static async Task<StreamSession[]> Read(
          IDbConnection connection
        , int id
        )
      {
        var sql = @"SELECT Id, ChannelId, ScheduledStreamId, UtcStartTime, UtcEndTime, TzdbVersionId FROM StreamSession WHERE Id = @Id";
        return (await connection.QueryAsync<StreamSession>(
            sql
          , new 
            {
              Id = id,
            })).ToArray();
      }

      public static async Task Update(
          IDbConnection connection
        , int id
        , int channelId
        , int scheduledStreamId
        , Instant utcStartTime
        , Instant utcEndTime
        , string tzdbVersionId
        )
      {
        var sql = @"UPDATE StreamSession SET ChannelId = @ChannelId, ScheduledStreamId = @ScheduledStreamId, UtcStartTime = @UtcStartTime, UtcEndTime = @UtcEndTime, TzdbVersionId = @TzdbVersionId WHERE Id = @Id";
        await connection.ExecuteAsync(
            sql
          , new StreamSession
            {
              Id = id,
              ChannelId = channelId,
              ScheduledStreamId = scheduledStreamId,
              UtcStartTime = utcStartTime,
              UtcEndTime = utcEndTime,
              TzdbVersionId = tzdbVersionId,
            });
      }

      public static async Task Delete(
          IDbConnection connection
        , int id
        )
      {
        var sql = @"DELETE StreamSession WHERE Id = @Id";
        await connection.ExecuteAsync(
            sql
          , new 
            {
              Id = id,
            });
      }

      public int Id { get; set; }
      public int ChannelId { get; set; }
      public int ScheduledStreamId { get; set; }
      public Instant UtcStartTime { get; set; }
      public Instant UtcEndTime { get; set; }
      public string TzdbVersionId { get; set; }
    }

  }
}

