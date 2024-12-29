using Domin.Entities;

namespace WebSite.EndPoint.Areas.Company.Models.Product
{
    public class ProductVM
    {
        public long Id { get; set; }
        public int RowNum { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid? PictureFileName { get; set; }

        public bool IsConfirmed { get; set; }

        public long? CompanyId { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Category> Categories { get; set; }
    }
}
