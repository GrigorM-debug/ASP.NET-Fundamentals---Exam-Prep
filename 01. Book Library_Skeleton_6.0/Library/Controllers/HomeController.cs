﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(User?.Identity?.IsAuthenticated == true)
                return RedirectToAction("All", "Books");
            return View();
        }
    }
}