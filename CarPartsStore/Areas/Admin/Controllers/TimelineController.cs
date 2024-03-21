using Microsoft.AspNetCore.Mvc;
using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TimelineController : Controller
    {
        private readonly ITimelineRepository _TimelineRepo;
        private readonly ILogger<TimelineController> _logger;

        public TimelineController(ITimelineRepository timelineRepo,
                                    ILogger<TimelineController> logger)
        {
            _TimelineRepo = timelineRepo;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TimelineModel tmModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(tmModel.ImageUrl))
                    {
                        tmModel.ImageUrl = "~/img/noimage.png";
                    }

                    _TimelineRepo.Add(tmModel);

                    return Redirect("/Admin/Home/Index#about");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TimelineModel tmModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(tmModel.ImageUrl))
                        tmModel.ImageUrl = "~/img/noimage.png";

                    _TimelineRepo.Update(tmModel);
                    return Redirect("/Admin/Home/Index#about");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        
        public IActionResult Delete(int tmId)
        {
            try
            {
                _TimelineRepo.Delete(tmId);

                return Redirect("/Admin/Home/Index#about");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
