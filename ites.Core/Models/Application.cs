namespace ites.Core.Models
{
    public class Application
    {
        public Application(Guid id, Guid from, Guid to)
        {
            Id = id;
            From = from;
            For = to;
        }
        public Application() { }
        public Guid Id { get; private set; }
        public Guid For { get; private set; }
        public Guid From { get; private set; } 
    }
}
