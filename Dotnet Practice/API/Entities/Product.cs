namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string productName  { get; set; } 
        public int Price { get; set; }
        public string Description { get; set; }
        public string Category {get; set;}
        public double Rating {get; set;}

        public int userId {get; set;}
    }
}