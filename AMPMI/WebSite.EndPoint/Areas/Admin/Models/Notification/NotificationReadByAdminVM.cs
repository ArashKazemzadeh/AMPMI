using AQS_Aplication.Dtos.BaseServiceDto.NotificationDtos;

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
        internal static List<NotificationReadByAdminVM> Seed()
        {

            return new List<NotificationReadByAdminVM>
            {
                new NotificationReadByAdminVM
                {   Id = 1,
                    Subject = "اطلاعیه مهم",
                    Description = "به اطلاع می‌رساند سرویس جدید اضافه شده است.",
                    CreateAt = DateTime.Now
                },
                new NotificationReadByAdminVM
                {
                    Id = 2,
                    Subject = "هشدار سیستم",
                    Description = "عملکرد سیستم با موفقیت انجام شد.",
                    CreateAt = DateTime.Now.AddDays(-1)
                },
                new NotificationReadByAdminVM
                {
                    Id=3,
                    Subject = "بروزرسانی",
                    Description = "نسخه جدید سیستم منتشر شده است.",
                    CreateAt = DateTime.Now.AddDays(-2)
                }
            };
        }

    }
}

