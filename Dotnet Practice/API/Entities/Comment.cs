namespace API.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ProductID { get; set; }
        public string CommentDescription { get; set; }
        public int Rating { get; set; }
        public int ApprovedStatus { get; set; }

        public string AddedDate{get;set;}
    }
}