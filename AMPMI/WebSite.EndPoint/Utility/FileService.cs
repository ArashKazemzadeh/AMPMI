using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebSite.EndPoint.Utility
{
    public interface IFileServices
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFile(string relativePath);
        Task<string> GetFilePath(string relativePath);
    }
    public class FileService : IFileServices
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName = "uploads")
        {

            if (file == null || file.Length == 0)
                throw new ArgumentException("عکس بارگذاری نشده است");
            if ((file.Length / 1000) > 200)
                throw new ArgumentException("حجم عکس نباید از 200 کیلوبایت بیشتر باشد");
            string uploadPath = Path.Combine(_env.WebRootPath, folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            folderName = "/"+folderName;
            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

        public Task<bool> DeleteFile(string relativePath)
        {
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<string> GetFilePath(string relativePath)
        {
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
                return Task.FromResult(fullPath);

            throw new FileNotFoundException("عکس قبلی جهت جایگزینی پیدا نشد", relativePath);
        }
    }
}
