namespace AQS_Aplication.Dtos.BaseServiceDto.NotificationDtos
{
    public class NotificationReadAdminDto
    {
        public long Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
    }
}
