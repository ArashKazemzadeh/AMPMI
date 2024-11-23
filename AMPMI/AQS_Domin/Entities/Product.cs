
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AQS_Domin.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required string Description { get; set; }
        public long Price { get; set; }
        public string ImageAdress { get; set; }
        public bool Displayed { get; set; } = true;
    }
}


