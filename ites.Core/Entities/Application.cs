namespace ites.Core.Entities
{
    public sealed class Application : BaseEntity
    {
        public Guid For { get; set; }
        public Guid From { get; set; }
    }
}
