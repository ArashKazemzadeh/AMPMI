namespace WebSite.EndPoint.Utility
{
    /// <summary>
    /// اینترفیس مدیریت فایل‌های ویدیو شامل عملیات ذخیره، حذف، بازیابی و لیست کردن فایل‌ها.
    /// </summary>
    public interface IVideoService
    {
        /// <summary>
        /// ذخیره یک فایل ویدیو در پوشه مشخص شده.
        /// </summary>
        /// <param name="video">فایل ویدیو برای ذخیره.</param>
        /// <param name="folderName">نام پوشه برای ذخیره ویدیو. مقدار پیش‌فرض "videos" است.</param>
        /// <returns>مسیر نسبی فایل ذخیره شده.</returns>
        Task<string> SaveVideoAsync(IFormFile video, string folderName = "videos");

        /// <summary>
        /// حذف فایل ویدیو با استفاده از مسیر نسبی آن.
        /// </summary>
        /// <param name="relativePath">مسیر نسبی فایل ویدیو برای حذف.</param>
        /// <returns>اگر فایل با موفقیت حذف شد، مقدار true برمی‌گرداند و در غیر این صورت false.</returns>
        bool DeleteVideo(string relativePath);

        /// <summary>
        /// دریافت مسیر کامل فیزیکی فایل ویدیو با استفاده از مسیر نسبی آن.
        /// </summary>
        /// <param name="relativePath">مسیر نسبی فایل ویدیو.</param>
        /// <returns>مسیر کامل فیزیکی فایل ویدیو.</returns>
        string GetVideoPath(string relativePath);

        /// <summary>
        /// ذخیره چندین فایل ویدیو در پوشه مشخص شده.
        /// </summary>
        /// <param name="videos">مجموعه‌ای از فایل‌های ویدیو برای ذخیره.</param>
        /// <param name="folderName">نام پوشه برای ذخیره ویدیوها. مقدار پیش‌فرض "videos" است.</param>
        /// <returns>لیستی از مسیرهای نسبی فایل‌های ذخیره شده.</returns>
        Task<List<string>> SaveMultipleVideosAsync(IEnumerable<IFormFile> videos, string folderName = "videos");

        /// <summary>
        /// لیست تمام فایل‌های ویدیو در پوشه مشخص شده.
        /// </summary>
        /// <param name="folderName">نام پوشه برای لیست کردن ویدیوها. مقدار پیش‌فرض "videos" است.</param>
        /// <returns>لیستی از مسیرهای نسبی فایل‌های ویدیو.</returns>
        List<string> ListVideos(string folderName = "videos");
    }

    public class VideoService : IVideoService
    {
        private readonly IWebHostEnvironment _env;

        public VideoService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <inheritdoc/>
        public async Task<string> SaveVideoAsync(IFormFile video, string folderName = "videos")
        {
            if (video == null || video.Length == 0)
                throw new ArgumentException("فایل ویدیو خالی یا نامعتبر است.");

            string uploadPath = Path.Combine(_env.WebRootPath, folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(video.FileName)}";
            string videoPath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(videoPath, FileMode.Create))
            {
                await video.CopyToAsync(stream);
            }

            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

        /// <inheritdoc/>
        public async Task<List<string>> SaveMultipleVideosAsync(IEnumerable<IFormFile> videos, string folderName = "videos")
        {
            if (videos == null || !videos.Any())
                throw new ArgumentException("هیچ فایل ویدیویی ارائه نشده است.");

            var savedPaths = new List<string>();

            foreach (var video in videos)
            {
                string savedPath = await SaveVideoAsync(video, folderName);
                savedPaths.Add(savedPath);
            }

            return savedPaths;
        }

        /// <inheritdoc/>
        public bool DeleteVideo(string relativePath)
        {
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public string GetVideoPath(string relativePath)
        {
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
                return fullPath;

            throw new FileNotFoundException("فایل ویدیو یافت نشد.", relativePath);
        }

        /// <inheritdoc/>
        public List<string> ListVideos(string folderName = "videos")
        {
            string folderPath = Path.Combine(_env.WebRootPath, folderName);

            if (!Directory.Exists(folderPath))
                return new List<string>();

            return Directory.GetFiles(folderPath)
                            .Select(f => Path.Combine(folderName, Path.GetFileName(f)).Replace("\\", "/"))
                            .ToList();
        }
    }
}
