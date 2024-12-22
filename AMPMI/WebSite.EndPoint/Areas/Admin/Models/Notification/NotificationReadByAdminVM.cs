using AQS_Aplication.Dtos.BaseServiceDto.NotificationDtos;

namespace WebSite.EndPoint.Areas.Admin.Models.Notification
{
    internal class NotificationReadByAdminVM
    {
        public long Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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
                    CreateAt = item.CreateAt
                });
            }

            return result;
        }


    }

}

