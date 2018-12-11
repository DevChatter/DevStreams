namespace DevChatter.DevStreams.Core.Model
{
    public class Location
    {
        public Location(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}