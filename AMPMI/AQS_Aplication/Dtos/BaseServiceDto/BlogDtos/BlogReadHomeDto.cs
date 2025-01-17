using Domin.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Application.Dtos.BaseServiceDto.BlogDtos
{
    public class BlogReadHomeDto
    {
        public int Id { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime? CreateUpdateAt { get; set; }

        public string? VideoFileName { get; set; }
        public string? HeaderPictureFileName { get; set; }
    }
    public class BlogReadAdminDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public DateTime? CreateUpdateAt { get; set; }
        public string? VideoFileName { get; set; }
        public string? HeaderPictureFileName { get; set; }
        public List<IFormFile>? PictureFileName { get; set; }
        public virtual List<BlogPicture>? BlogPictures { get; set; } = new List<BlogPicture>();
    }
}
