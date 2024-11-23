using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AQS_Domin.Entities
{
    [Table("Notifications")]

    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool Displayed { get; set; }
    }
}


