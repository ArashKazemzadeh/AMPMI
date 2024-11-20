using System.ComponentModel.DataAnnotations;

namespace AQS_Domin.Entities
{
    /// <summary>
    /// just use for test before install Identity
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
