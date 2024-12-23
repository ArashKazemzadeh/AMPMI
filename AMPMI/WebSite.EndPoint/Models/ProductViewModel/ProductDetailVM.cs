﻿
namespace WebSite.EndPoint.Models.ProductViewModel
{
    public class ProductDetailVM
    {
        //public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid? PictureFileName { get; set; }

        public long? CompanyId { get; set; }

        public int SubCategoryId { get; set; }
        public string? CompanyName { get; set; }
        public Guid? CompanyLogoRout { get; set; }

    }
}
