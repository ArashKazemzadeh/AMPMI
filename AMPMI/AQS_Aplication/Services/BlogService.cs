using AQS_Application.Dtos.BaseServiceDto.BlogDtos;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IDbAmpmiContext _context;

        public BlogService(IDbAmpmiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// متد برای ایجاد یک بلاگ جدید
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public async Task<int> Create(Blog blog)
        {
            blog.CreateUpdateAt = DateTime.Now;

            var row = _context.Blogs.Add(blog);
            int result = await _context.SaveChangesAsync();

            return result > 0 ? row.Entity.Id : -1;
        }

        /// <summary>
        /// متد برای حذف بلاگ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultOutPutMethodEnum> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }

        /// <summary>
        /// متد برای خواندن تمامی بلاگ‌ها
        /// </summary>
        /// <returns></returns>
        public async Task<List<BlogReadAdminDto>> Read()
        {
            var blogs = await _context.Blogs
                .Include(b => b.BlogPictures)
                .Select(b => new BlogReadAdminDto
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Description = b.Description,
                    CreateUpdateAt = b.CreateUpdateAt,
                    VideoFileName = b.VideoFileName,
                    HeaderPictureFileName = b.HeaderPictureFileName,
                    BlogPictures = b.BlogPictures.Select(bp => new BlogPicture
                    {
                        Id = bp.Id,
                        BlogId = bp.BlogId,
                        Route = bp.Route
                    }).ToList()
                })
                .ToListAsync();

            return blogs ?? new List<BlogReadAdminDto>();
        }


        public async Task<List<BlogReadHomeDto>> ReadTop3()
        {
            // TODO : Description Must be summarize
            var blogs = await _context.Blogs
                .OrderBy(x => x.CreateUpdateAt)
                .Take(3)
                .Select(x => new BlogReadHomeDto()
                {
                    Id = x.Id,
                    CreateUpdateAt = x.CreateUpdateAt,
                    Description = x.Description,
                    HeaderPictureFileName = x.HeaderPictureFileName,
                    Subject = x.Subject,
                    VideoFileName = x.VideoFileName
                })
                .ToListAsync();
            return blogs ?? new List<BlogReadHomeDto>();
        }

        /// <summary>
        /// متد برای خواندن بلاگ با شناسه
        /// این متد منسوخ شده است و بهتر است از متد ReadByIdAsync استفاده شود
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Blog?> ReadById(int id)
        {
            return await _context.Blogs.Include(b => b.BlogPictures)
                                       .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<BlogReadAdminDto?> ReadByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.BlogPictures)
                .Where(b => b.Id == id)
                .Select(b => new BlogReadAdminDto
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Description = b.Description,
                    CreateUpdateAt = b.CreateUpdateAt,
                    VideoFileName = b.VideoFileName,
                    HeaderPictureFileName = b.HeaderPictureFileName,
                    BlogPictures = b.BlogPictures.Select(bp => new BlogPicture
                    {
                        Id = bp.Id,
                        BlogId = bp.BlogId,
                        Route = bp.Route
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// متد برای به‌روزرسانی بلاگ
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public async Task<ResultOutPutMethodEnum> Update(Blog blog)
        {
            var existingBlog = await _context.Blogs.FindAsync(blog.Id);

            if (existingBlog == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (blog.Subject != null && existingBlog.Subject != blog.Subject)
                existingBlog.Subject = blog.Subject;

            if (blog.Description != null && existingBlog.Description != blog.Description)
                existingBlog.Description = blog.Description;

            existingBlog.CreateUpdateAt = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0 ?
                ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }

        /// <summary>
        /// متد برای به‌روزرسانی تصویر هدر
        /// </summary>
        /// <param name="id"></param>
        /// <param name="headerPictureFileName"></param>
        /// <returns></returns>
        public async Task<ResultOutPutMethodEnum> UpdateHeaderPicture(int id, string headerPictureFileName)
        {
            var existingBlog = await _context.Blogs.FindAsync(id);
            if (existingBlog == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingBlog.HeaderPictureFileName = headerPictureFileName;
            existingBlog.CreateUpdateAt = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }

        /// <summary>
        /// متد برای به‌روزرسانی فایل ویدیو
        /// </summary>
        /// <param name="id"></param>
        /// <param name="videoFileName"></param>
        /// <returns></returns>
        public async Task<ResultOutPutMethodEnum> UpdateVideoFile(int id, string videoFileName)
        {
            var existingBlog = await _context.Blogs.FindAsync(id);
            if (existingBlog == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingBlog.VideoFileName = videoFileName;
            existingBlog.CreateUpdateAt = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<ResultOutPutMethodEnum> UpdatePictureRout(int blogId, string rout)
        {
            BlogPicture blogPicture = new()
            {
                BlogId = blogId,
                Route = rout
            };
            await _context.BlogPictures.AddAsync(blogPicture);

            int result = await _context.SaveChangesAsync();
            return result > 0 ?
               ResultOutPutMethodEnum.savechanged :
               ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<ResultOutPutMethodEnum> DeleteBlogPicture(int pictureId)
        {
            var picture = await _context.BlogPictures.FindAsync(pictureId);
            if (picture != null)
            {
                _context.BlogPictures.Remove(picture);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }
    }
}
