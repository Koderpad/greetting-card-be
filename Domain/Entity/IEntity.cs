namespace Domain.Entity
{
    public interface IEntity
    {
        public string Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
