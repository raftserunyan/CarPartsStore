using CarPartsStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CarPartsStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        private readonly IContactRepository _ContactRepo;
        private readonly IHeadSettingRepository _HeadSettingsRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly ITimelineRepository _TimelineRepo;
        private readonly ITeamMemberRepository _TeamMemberRepo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductRepository productRepo,
                                IContactRepository contactRepo,
                                IHeadSettingRepository headRepo,
                                ICategoryRepository categoryRepo,
                                ITimelineRepository timelineRepo,
                                ITeamMemberRepository teamRepo,
                                ILogger<HomeController> logger)
        {
            _ProductRepo = productRepo;
            _ContactRepo = contactRepo;
            _HeadSettingsRepo = headRepo;
            _CategoryRepo = categoryRepo;
            _TimelineRepo = timelineRepo;
            _TeamMemberRepo = teamRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                IEnumerable<Product> prods = _ProductRepo.GetAll();
                foreach (Product product in prods)
                {
                    product.Category = _CategoryRepo.Get(product.CategoryId);
                }

                ViewBag.HeadSettings = _HeadSettingsRepo.Get();
                ViewBag.Products = prods;
                ViewBag.CategorySelect = new SelectList(_CategoryRepo.GetAll(), "Id", "Name");
                ViewBag.TimelineModels = _TimelineRepo.GetAll();
                ViewBag.TeamMembers = _TeamMemberRepo.GetAll();

                IEnumerable<ContactUsModel> contacts = _ContactRepo.GetAll();

                return View(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult EditHeadSection(int id, string imgUrl, string txt1, string txt2, string btnTxt, string ftColor, string stColor)
        {
            try
            {
                HeadSetting hs = new HeadSetting
                {
                    Id = id,
                    ImageUrl = imgUrl,
                    FirstText = txt1,
                    FirstTextColor = ftColor,
                    SecondText = txt2,
                    SecondTextColor = stColor,
                    ButtonText = btnTxt
                };

                _HeadSettingsRepo.Update(hs);

                return Redirect("/Admin/Home/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public IActionResult About()
        {
            try
            {
                ViewData["Message"] = "Your application description page.";

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
