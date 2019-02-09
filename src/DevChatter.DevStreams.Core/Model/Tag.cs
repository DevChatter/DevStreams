using Dapper.Contrib.Extensions;

namespace DevChatter.DevStreams.Core.Model
{
    [Table("Tags")]
    public class Tag : DataEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}