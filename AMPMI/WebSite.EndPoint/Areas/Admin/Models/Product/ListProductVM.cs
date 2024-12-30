namespace WebSite.EndPoint.Areas.Admin.Models.Product
{
    public class ListProductVM
    {
        public long Id { get; set; }    
        public int RowNum { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? PictureFileSrc { get; set; }

        public bool IsConfirmed { get; set; }

        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }

        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
    }
}
