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
    public class TeamMemberController : Controller
    {
        private readonly ITeamMemberRepository _TeamMemberRepo;
        private readonly ILogger<TeamMemberController> _logger;

        public TeamMemberController(ITeamMemberRepository teamRepo,
                                    ILogger<TeamMemberController> logger)
        {
            _TeamMemberRepo = teamRepo;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TeamMember member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(member.ImageUrl))
                    {
                        member.ImageUrl = "~/img/noimage.png";
                    }
                    if (string.IsNullOrEmpty(member.FacebookLink))
                        member.FacebookLink = "#";
                    if (string.IsNullOrEmpty(member.TwitterLink))
                        member.TwitterLink = "#";
                    if (string.IsNullOrEmpty(member.LinkedinLink))
                        member.LinkedinLink = "#";

                    _TeamMemberRepo.Add(member);

                    return Redirect("/Admin/Home/Index#team");
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
        public IActionResult Edit(TeamMember member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(member.ImageUrl))
                        member.ImageUrl = "~/img/noimage.png";
                    if (string.IsNullOrEmpty(member.FacebookLink))
                        member.FacebookLink = "#";
                    if (string.IsNullOrEmpty(member.TwitterLink))
                        member.TwitterLink = "#";
                    if (string.IsNullOrEmpty(member.LinkedinLink))
                        member.LinkedinLink = "#";

                    _TeamMemberRepo.Update(member);
                    return Redirect("/Admin/Home/Index#team");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public IActionResult Delete(int memId)
        {
            try
            {
                _TeamMemberRepo.Delete(memId);

                return Redirect("/Admin/Home/Index#team");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            
        }
    }
}

