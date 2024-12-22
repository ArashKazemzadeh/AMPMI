using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Admin.Models.Notification
{
    internal class NotificationSaveVM
    {
        [Required(ErrorMessage = "موضوع عنوان الزامی است")]
        public required string Subject { get; set; }
        [Required(ErrorMessage = "متن عنوان الزامی است")]
        public required string Description { get; set; }
    }
}
