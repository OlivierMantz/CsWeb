namespace BackEnd.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long AuthorId { get; set; }

    }
}
