namespace ites.Core.Entities
{
    public sealed class ApplicationEntity : BaseEntity
    {
        public Guid For { get; set; }
        public Guid From { get; set; }
    }
}
