using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Banner;
using WebSite.EndPoint.Utility;

[Area("Admin")]
public class BannerController : Controller
{
    private readonly IBannerService _bannerService;
    private readonly IVideoService _videoService;
    private readonly string PictureFolder = "Banners";

    public BannerController(IBannerService bannerService, IVideoService videoService)
    {
        _bannerService = bannerService;
        _videoService = videoService;
    }

    public async Task<IActionResult> BannerList()
    {
        ViewData["Message"] = TempData["Message"]?.ToString();
        var banners = await _bannerService.ReadAll();
        var viewModel = new MultiBannerViewModel()
        {
            rout1 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout1)?.Rout ?? string.Empty,
            rout2 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout2)?.Rout ?? string.Empty,
            rout3 = banners.FirstOrDefault(b => b.Id == BannerIdEnum.rout3)?.Rout ?? string.Empty
        };
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
            if (await _videoService.DeleteVideo(banner.Rout))
            {
                string msg = string.Empty;
                if (await _bannerService.Update((BannerIdEnum)bannerId, string.Empty))
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

        if (banner != null && !string.IsNullOrEmpty(banner.Rout))
        {
            await _videoService.DeleteVideo(banner.Rout);
        }

        var newRout = await _videoService.SaveVideoAsync(picture, PictureFolder);

        if (!string.IsNullOrEmpty(newRout))
        {
            bool result = await _bannerService.Update(bannerId, newRout);

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
