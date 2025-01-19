using AQS_Application.Dtos.BaseServiceDto.BlogDtos;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IFileServices _fileServices;
        private readonly IVideoService _videoServices;

        const string PictureFolder = "BlogPicturesFiles";
        const string VideoFolder = "BlogVideoFiles";
        public BlogController(IBlogService blogService, IFileServices fileServices, IVideoService videoServices)
        {
            _blogService = blogService;
            _fileServices = fileServices;
            _videoServices = videoServices;
        }

        public async Task<IActionResult> BlogList()
        {
            List<BlogReadAdminDto> blogs = await _blogService.Read();
            return View(blogs);
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogReadAdminDto dto, string content, IFormFile blogvid)
        {
            if (dto.Id > 0)
                return await EditBlog(dto, content, blogvid);
            else
                return await CreateBlog(dto, content, blogvid);
        }
        public async Task<IActionResult> CreateBlog()
        {
            return View("EditBlog", new BlogReadAdminDto());
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogReadAdminDto dto, string content, IFormFile blogvid)//todo
        {
            Blog newBlog = new Blog()
            {
                Subject = dto.Subject,
                Description = content,
            };
            try
            {
                int id = await _blogService.Create(newBlog);
                if (id > 0)
                {
                    if (dto.HeaderPictureFileName != null)
                    {
                        string newPic = await _fileServices.SaveFileAsync(dto.HeaderPictureFileName, PictureFolder);
                        if (string.IsNullOrEmpty(newPic))
                        {
                            ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                            return View("EditBlog", dto);
                        }
                        else
                        {
                            var result = await _blogService.UpdateHeaderPicture(id, newPic);
                            if (result != ResultOutPutMethodEnum.savechanged)
                            {
                                ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                return View("EditBlog", dto);
                            }
                        }
                    }
                    if (blogvid != null)
                    {
                        string newvid = await _videoServices.SaveVideoAsync(blogvid, VideoFolder);
                        if (string.IsNullOrEmpty(newvid))
                        {
                            ViewData["error"] = "خطایی در هنگام ثبت ویدیو رخ داد";
                            return View("EditBlog", dto);
                        }
                        else
                        {
                            var result = await _blogService.UpdateVideoFile(id, newvid);
                            if (result != ResultOutPutMethodEnum.savechanged)
                            {
                                ViewData["error"] = "خطایی در هنگام ثبت ویدیو رخ داد";
                                return View("EditBlog", dto);
                            }
                        }
                    }
                    if (dto.PictureFileName != null)
                    {
                        foreach (var item in dto.PictureFileName)
                        {
                            string newPicture = await _fileServices.SaveFileAsync(item, PictureFolder);
                            if (string.IsNullOrEmpty(newPicture))
                            {
                                ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                return View("EditBlog", dto);
                            }
                            else
                            {
                                var result = await _blogService.UpdatePictureRout(id, newPicture);
                                if (result != ResultOutPutMethodEnum.savechanged)
                                {
                                    ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                    return View("EditBlog", dto);
                                }
                            }
                        }
                    }

                    return RedirectToAction(nameof(BlogList));
                }
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                    return View("EditBlog", dto);
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
                return View("EditBlog", dto);
            }
        }
        public async Task<IActionResult> EditBlog(int id)
        {
            var blog = await _blogService.ReadByIdAsync(id);
            if (blog != null)
            {
                if (blog.Description != null)
                    ViewBag.BlogContent = blog.Description;
                return View("EditBlog", new BlogReadAdminDto()
                {
                    Id = blog.Id,
                    Subject = blog.Subject,
                    Description = blog.Description,
                    BlogPictures = blog.BlogPictures,
                    VideoFileName = blog.VideoFileName,
                });

            }
            else
            {
                TempData["error"] = "خبر مورد نظر یافت نشد";
                return RedirectToAction(nameof(BlogList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBlog(BlogReadAdminDto dto, string content, IFormFile blogvid)
        {
            Blog existBlog = new Blog()
            {
                Id = dto.Id,
                Description = content
            };
            try
            {
                if (blogvid != null)
                {
                    string newvid = await _videoServices.SaveVideoAsync(blogvid, VideoFolder);
                    if (string.IsNullOrEmpty(newvid))
                    {
                        ViewData["error"] = "خطایی در هنگام ثبت ویدیو رخ داد";
                        return View("EditBlog", dto);
                    }
                    else
                    {
                        var resultvid = await _blogService.UpdateVideoFile(dto.Id, newvid);
                        if (resultvid != ResultOutPutMethodEnum.savechanged)
                        {
                            ViewData["error"] = "خطایی در هنگام ثبت ویدیو رخ داد";
                            return View("EditBlog", dto);
                        }
                    }
                }
                if (dto.PictureFileName != null)
                {
                    foreach (var item in dto.PictureFileName)
                    {
                        string newPicture = await _fileServices.SaveFileAsync(item, PictureFolder);
                        if (string.IsNullOrEmpty(newPicture))
                        {
                            ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                            return View("EditProduct", dto);
                        }
                        else
                        {
                            var resultPic = await _blogService.UpdatePictureRout(existBlog.Id, newPicture);
                            if (resultPic != ResultOutPutMethodEnum.savechanged)
                            {
                                ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                return View("EditProduct", dto);
                            }
                        }
                    }
                }

                var result = await _blogService.Update(existBlog);
                if (result == ResultOutPutMethodEnum.savechanged || result == ResultOutPutMethodEnum.dontSaved)
                    return RedirectToAction(nameof(BlogList));
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت خبر رخ داد";
                    return View(dto);
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
                return View(dto);
            }
        }
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogService.ReadByIdAsync(id);
            if (blog != null)
            {
                var blogPictures = new List<BlogPicture>();
                blogPictures.AddRange(blog.BlogPictures);
                foreach (var item in blogPictures)
                {
                    if (await _fileServices.DeleteFile(item.Route))
                    {
                        await _blogService.DeleteBlogPicture(item.Id);
                    }
                }
                var result = await _blogService.Delete(id);
                if (result == ResultOutPutMethodEnum.savechanged)
                    await _videoServices.DeleteVideo(blog.VideoFileName);
                if (result != ResultOutPutMethodEnum.savechanged)
                    TempData["error"] = "خطایی در هنگام حذف خبر رخ داد";
            }
            else
            {
                TempData["error"] = "محصول مورد نظر یافت نشد";
            }
            return RedirectToAction(nameof(BlogList));
        }
        public async Task<IActionResult> DeletePicture(int pictureId, int blogId)
        {
            var result = await _blogService.DeleteBlogPicture(pictureId);
            if (result != ResultOutPutMethodEnum.savechanged)
                TempData["error"] = "خطا در هنگام حذف تصویر محصول";

            return RedirectToAction(nameof(EditBlog), new { id = blogId });
        }
    }
}
