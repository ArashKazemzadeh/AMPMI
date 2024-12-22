namespace AQS_Aplication.Dtos.IdentityServiceDto
{
    public class RegisterIdentityDTO
    {
        public required string Mobile { get; set; }
        public required string CompanyName { get; set; }
        public required string ManagerName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Address { get; set; }
    }
}
