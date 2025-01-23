using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using AQS_Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Banner;
using WebSite.EndPoint.Utility;
using static AQS_Common.Enums.FolderNamesEnum;

[Area("Admin")]
public class BannerController : Controller
{
    private readonly IBannerService _bannerService;
    private readonly IVideoService _videoService;
    private readonly IFileServices _fileServices;
    static string BannerFolder = FolderNamesEnum.GetFileName(FolderTypes.Banner);

    public BannerController(IBannerService bannerService, IVideoService videoService, IFileServices fileServices)
    {
        _bannerService = bannerService;
        _videoService = videoService;
        _fileServices = fileServices;
    }

    public async Task<IActionResult> BannerList()
    {
        ViewData["Message"] = TempData["Message"]?.ToString();
        var banners = await _bannerService.ReadAll();
        var viewModel = new MultiBannerViewModel()
        {
            rout1 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout1)?.Rout ?? string.Empty,
            rout2 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout2)?.Rout ?? string.Empty,
            rout3 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout3)?.Rout ?? string.Empty,
        };
        var banner1 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout1);
        if (banner1 != null)
            viewModel.type1 = banner1.Type;

        var banner2 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout2);
        if (banner2 != null)
            viewModel.type2 = banner2.Type;

        var banner3 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout3);
        if (banner3 != null)
            viewModel.type3 = banner3.Type;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveBanners(MultiBannerViewModel model)
    {
        try
        {
            if (model == null)
                return RedirectToAction(nameof(BannerList));
            if (model.Banner1 != null)
                await HandleFileReplacement(BannerIdEnum.rout1, model.Banner1);

            if (model.Banner2 != null)
                await HandleFileReplacement(BannerIdEnum.rout2, model.Banner2);

            if (model.Banner3 != null)
                await HandleFileReplacement(BannerIdEnum.rout3, model.Banner3);

            TempData["Message"] = "تمام فایل‌ها با موفقیت ذخیره شدند.";
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"خطا: {ex.Message}";
        }

        return RedirectToAction(nameof(BannerList));
    }

    public async Task<IActionResult> Delete(int bannerId)
    {
        var banner = await _bannerService.ReadById((BannerIdEnum)bannerId);
        if (banner != null)
        {
            bool fileDeleted = false;
            if(banner.Type == BannerTypeEnum.Video)
            {
                fileDeleted = await _videoService.DeleteVideo(banner.Rout);
            }
            else if (banner.Type == BannerTypeEnum.Image)
            {
                fileDeleted = await _fileServices.DeleteFile(banner.Rout);
            }
            if (fileDeleted)
            {
                string msg = string.Empty;
                if (await _bannerService.Update((BannerIdEnum)bannerId, string.Empty, 0))
                    msg = "بنر حذف شد";
                else
                    msg = "بنر حذف نشد";

                TempData["Message"] = msg;
            }
        }
        return RedirectToAction(nameof(BannerList));
    }

    private async Task HandleFileReplacement(BannerIdEnum bannerId, IFormFile picture)
    {
        var banner = await _bannerService.ReadById(bannerId);
        string newRoute = string.Empty;
        BannerTypeEnum bannerType = 0;

        if (picture.ContentType.StartsWith("video"))
        {
            if (banner != null && !string.IsNullOrEmpty(banner.Rout))
            {
                await _videoService.DeleteVideo(banner.Rout);
            }
            bannerType = BannerTypeEnum.Video;
            newRoute = await _videoService.SaveVideoAsync(picture, BannerFolder);
        }
        else if (picture.ContentType.StartsWith("image"))
        {
            if (banner != null && !string.IsNullOrEmpty(banner.Rout))
            {
                await _fileServices.DeleteFile(banner.Rout);
            }
            bannerType = BannerTypeEnum.Image;
            newRoute = await _fileServices.SaveFileAsync(picture, BannerFolder);
        }

        if (!string.IsNullOrEmpty(newRoute))
        {
            bool result = await _bannerService.Update(bannerId, newRoute, bannerType);

            if (!result)
            {
                throw new Exception("خطایی در به‌روزرسانی مسیر فایل در دیتابیس رخ داد.");
            }
        }
        else
        {
            throw new Exception("خطایی در ذخیره فایل جدید رخ داد.");
        }
    }
}
