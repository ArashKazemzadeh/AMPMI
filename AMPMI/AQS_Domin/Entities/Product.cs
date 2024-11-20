
using System.ComponentModel.DataAnnotations;
namespace AQS_Domin.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required string Description { get; set; }
        public long Price { get; set; }
        public long ImageAdress { get; set; }
    }
}


