﻿using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutusController : Controller
    {
        public IActionResult ShowAboutUs()
        {
            return View();
        }
    }
}