using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Aplication.Services
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
        public async Task<ResultServiceMethods> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
            }
            return ResultServiceMethods.recordNotFounded;
        }

        /// <summary>
        /// متد برای خواندن تمامی بلاگ‌ها
        /// </summary>
        /// <returns></returns>
        public async Task<List<Blog>> Read()
        {
            var blogs = await _context.Blogs.Include(b => b.BlogPictures).AsNoTracking().ToListAsync();
            return blogs ?? new List<Blog>();
        }

        /// <summary>
        /// متد برای خواندن بلاگ با شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Blog?> ReadById(int id)
        {
            return await _context.Blogs.Include(b => b.BlogPictures)
                                       .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// متد برای به‌روزرسانی بلاگ
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public async Task<ResultServiceMethods> Update(Blog blog)
        {
            var existingBlog = await _context.Blogs.FindAsync(blog.Id);

            if (existingBlog == null)
                return ResultServiceMethods.recordNotFounded;

            if (blog.Subject != null && existingBlog.Subject != blog.Subject)
                existingBlog.Subject = blog.Subject;

            if (blog.Description != null  && existingBlog.Description != blog.Description)
                existingBlog.Description = blog.Description;

            existingBlog.CreateUpdateAt = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0 ?
                ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
        }

        /// <summary>
        /// متد برای به‌روزرسانی تصویر هدر
        /// </summary>
        /// <param name="id"></param>
        /// <param name="headerPictureFileName"></param>
        /// <returns></returns>
        public async Task<ResultServiceMethods> UpdateHeaderPicture(int id, Guid headerPictureFileName)
        {
            var existingBlog = await _context.Blogs.FindAsync(id);
            if (existingBlog == null)
                return ResultServiceMethods.recordNotFounded;

            existingBlog.HeaderPictureFileName = headerPictureFileName;
            existingBlog.CreateUpdateAt = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
        }

        /// <summary>
        /// متد برای به‌روزرسانی فایل ویدیو
        /// </summary>
        /// <param name="id"></param>
        /// <param name="videoFileName"></param>
        /// <returns></returns>
        public async Task<ResultServiceMethods> UpdateVideoFile(int id, Guid videoFileName)
        {
            var existingBlog = await _context.Blogs.FindAsync(id);
            if (existingBlog == null)
                return ResultServiceMethods.recordNotFounded;

            existingBlog.VideoFileName = videoFileName;
            existingBlog.CreateUpdateAt = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
        }
    }
}
