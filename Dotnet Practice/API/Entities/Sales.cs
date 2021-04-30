namespace API.Entities
{
    public class Sales
    {
        public int Id { get; set; }
        public string productId  { get; set; } 
        public int userId { get; set; }
        public int price { get; set; }
        public int amount { get; set; }

    }
}