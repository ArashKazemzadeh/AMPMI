namespace WebSite.EndPoint.Areas.Admin.Models.Category
{
    public class CategoryEditVM
    {
        public IFormFile Picture { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PreviousPictureRout { get; set; }
        public bool IsPictureChanged { get; set; } = false;
    }
}
