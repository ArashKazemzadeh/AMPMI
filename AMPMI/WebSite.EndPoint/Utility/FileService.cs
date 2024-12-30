namespace WebSite.EndPoint.Utility
{
    public interface IFileServices
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        bool DeleteFile(string relativePath);
        string GetFilePath(string relativePath);
    }
    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName = "uploads")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty");

            string uploadPath = Path.Combine(_env.WebRootPath, folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

        public bool DeleteFile(string relativePath)
        {
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }

        public string GetFilePath(string relativePath)
        {
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
                return fullPath;

            throw new FileNotFoundException("File not found", relativePath);
        }
    }
}
