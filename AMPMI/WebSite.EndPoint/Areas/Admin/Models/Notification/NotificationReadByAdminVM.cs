using AQS_Application.Dtos.BaseServiceDto.NotificationDtos;

namespace WebSite.EndPoint.Areas.Admin.Models.Notification
{
    internal class NotificationReadByAdminVM
    {
        public long Id { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; } 
        public DateTime CreateAt { get; set; }

        internal static List<NotificationReadByAdminVM> ConvertToModel(List<NotificationReadAdminDto> dto)
        {
            var result = new List<NotificationReadByAdminVM>();

            foreach (var item in dto)
            {
                result.Add(new NotificationReadByAdminVM
                {
                    Id = item.Id,
                    Subject = item.Subject,
                    Description = item.Description,
                    CreateAt = item.CreateAt.ToPersianDate()
                });
            }

            return result;
        }
    }
}

