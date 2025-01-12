using AQS_Application.Dtos.BaseServiceDto.BlogDtos;
using AQS_Application.Interfaces.IServices.BaseServices;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.Blog;

namespace WebSite.EndPoint.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            this._blogService = blogService;
        }
        public async Task<IActionResult> ShowBlog(int id)
        {
            var blog = await _blogService.ReadById(id);
            if (blog != null)
            {
                BlogDetailVM blogDetail = new BlogDetailVM()
                {
                    VideoFileName = blog.VideoFileName,
                    Id = id,
                    CreateUpdateAt = DateTime.Now,
                    Description = blog.Description,
                    HeaderPictureFileName = blog.HeaderPictureFileName,
                    Subject = blog.Subject,
                    BlogPictures = blog.BlogPictures.Select(x=>x.Route).ToList()
                };
                return View(blogDetail);
            }
            return View(new BlogDetailVM());
        }
    }
}

