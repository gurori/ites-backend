namespace ites.DataAccess.Entites
{
    public sealed class ApplicationEntity
    {
        public Guid Id { get; set; }
        public Guid For { get; set; }
        public Guid From { get; set; }
    }
}
