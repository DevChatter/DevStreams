using Dapper;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper
{
    public class DapperCrudRepository : ICrudRepository
    {
        protected readonly DatabaseSettings _dbSettings;

        public DapperCrudRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public async Task<int?> Insert<T>(T model) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.InsertAsync(model);
            }
        }

        public async Task<T> Get<T>(int id) where T : DataEntity
        {
            string tableName = GetTableName<T>();
            string sql = $"SELECT * FROM {tableName} WHERE Id = @id";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.QuerySingleAsync<T>(sql, new {id});
            }
        }

        public async Task<List<T>> GetAll<T>()
        {
            string tableName = GetTableName<T>();
            string sql = $"SELECT * FROM {tableName}";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return (await connection.QueryAsync<T>(sql)).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">WARNING: Should never come from user input!</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAll<T>(string filter, object args)
        {
            string tableName = GetTableName<T>();
            string sql = $"SELECT * FROM {tableName} WHERE {filter}";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return (await connection.QueryAsync<T>(sql, args)).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">WARNING: Should never come from user input!</param>
        /// <param name="orderBy">WARNING: Should never come from user input!</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAll<T>(string filter, string orderBy, object args)
        {
            string tableName = GetTableName<T>();
            string sql = $"SELECT * FROM {tableName} WHERE {filter} ORDER BY {orderBy}";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return (await connection.QueryAsync<T>(sql, args)).ToList();
            }
        }

        /// <summary>
        /// Returns all the channel data for a channel object.
        /// </summary>
        /// <returns>All Channel Info in a List<Channel> Object </Channel></returns>
        public async Task<List<Channel>> GetAllChannelInfo()
        {
            string sql = "SELECT * FROM Channels;";
            string sql2 = "SELECT * FROM TwitchChannels;";

            string combinedSQL = sql + sql2;

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var dbQuery = await connection.QueryMultipleAsync(combinedSQL);
                List<Channel> channels = dbQuery.Read<Channel>().ToList();
                var twitchChannels = dbQuery.Read<TwitchChannel>();

                foreach (var channel in channels)
                {
                    if (twitchChannels.Where(x => x?.ChannelId == channel?.Id).Any())
                    {
                        channel.Twitch = twitchChannels.Where(x => x?.ChannelId == channel?.Id).First();
                    }
                }

                return channels;
            }

        }

        public async Task<int> Update<T>(T model) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.UpdateAsync(model);
            }
        }

        public async Task<int> Delete<T>(int id) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.DeleteAsync(id);
            }
        }

        public async Task<int> Delete<T>(T model) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.DeleteAsync(model);
            }
        }

        public async Task<bool> Exists<T>(int id) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.RecordCountAsync<T>("WHERE Id=@id", new {id}) > 0;
            }
        }

        private static string GetTableName<T>()
        {
            var tableAttrib = typeof(T).GetCustomAttributes(true)
                .SingleOrDefault(attr => attr.GetType().Name == typeof(TableAttribute).Name) as dynamic;
            string tableName = tableAttrib?.Name ?? typeof(T).Name + "s";
            return tableName;
        }

        protected async Task<List<T>> QueryAsync<T>(string sql, object args)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return (await connection.QueryAsync<T>(sql, args)).ToList();
            }
        }
    }
}