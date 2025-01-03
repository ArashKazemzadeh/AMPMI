using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;

namespace WebSite.EndPoint.Areas.Admin.Models.Category
{
    internal class CategoryReadVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureFileName { get; set; }

        internal static List<CategoryReadVM> ConvertToModel(List<CategoryReadDto> dto)
        {
            var result = new List<CategoryReadVM>();

            foreach (var item in dto)
            {
                result.Add(new CategoryReadVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    PictureFileName = item.PictureFileName,
                });
            }

            return result;
        }
        internal static List<CategoryReadVM> Seed()
        {
            return new List<CategoryReadVM>
            {
                new CategoryReadVM
                {   Id = 1,
                    Name = "گروه مهم",
                },
                new CategoryReadVM
                {   Id = 2,
                    Name = "گروه سرود",
                },
                new CategoryReadVM
               {   Id = 3,
                    Name = "گروه اشغال",
                },
            };
        }

    }
}
