using System.ComponentModel.DataAnnotations;

namespace DevChatter.DevStreams.Core.Model
{
    public abstract class DataEntity
    {
        [Key]
        public int Id { get; set; }
    }
}