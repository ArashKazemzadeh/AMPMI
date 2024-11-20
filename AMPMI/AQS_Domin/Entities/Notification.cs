using System.ComponentModel.DataAnnotations;
namespace AQS_Domin.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool Displayed { get; set; }
    }
}


