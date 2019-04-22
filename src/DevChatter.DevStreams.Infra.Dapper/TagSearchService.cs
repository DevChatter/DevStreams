using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Tagging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper
{
    public class TagSearchService : ITagSearchService
    {
        private readonly DatabaseSettings _dbSettings;

        public TagSearchService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }


        public async Task<List<TagWithCount>> FindTagsWithCount(string filter)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var args = new { Search = $"%{filter}%" };
                const string sql = 
                    @"SELECT t.*, count(*) as [Count]
                    FROM[Tags] t
                        INNER JOIN[ChannelTags] ct on ct.TagId = t.Id
                    WHERE[Name] LIKE @Search
                    GROUP BY  t.Id, t.Name, t.Description
                ";
                List<TagWithCount> tagsWithCounts = 
                    (await connection.QueryAsync<Tag, int, TagWithCount>(
                        sql,
                        (tag, count) => new TagWithCount { Tag = tag, Count = count },
                        args,
                        splitOn:"Count")).ToList();
                return tagsWithCounts;
            }
        }
    }
}