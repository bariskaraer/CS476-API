namespace API.Entities
{
    public class Campaign
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
        public int startDate { get; set; }
        public int endDate { get; set; }
        public string description { get; set; }
        public int percentage {get;set;}
    }
}