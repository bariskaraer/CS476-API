namespace API.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int NotificationId  { get; set; } 
        public int UserId  { get; set; } 
        public int Seen { get; set; }
    }
}