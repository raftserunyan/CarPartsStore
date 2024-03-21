using CarPartsStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CarPartsStore.Repositories.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Localization;

namespace CarPartsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        private readonly IContactRepository _ContactRepo;
        private readonly IHeadSettingRepository _HeadSettingsRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly ITimelineRepository _TimelineRepo;
        private readonly ITeamMemberRepository _TeamMemberRepo;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IProductRepository productRepo,
                                IContactRepository contactRepo,
                                IHeadSettingRepository headRepo,
                                ICategoryRepository categoryRepo,
                                ITimelineRepository timelineRepo,
                                ITeamMemberRepository teamRepo,
                                ILogger<HomeController> logger,
                                IStringLocalizer<HomeController> localizer)
        {
            _ProductRepo = productRepo;
            _ContactRepo = contactRepo;
            _HeadSettingsRepo = headRepo;
            _CategoryRepo = categoryRepo;
            _TimelineRepo = timelineRepo;
            _TeamMemberRepo = teamRepo;
            _logger = logger;
            _localizer = localizer;
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
                ViewBag.TimelineModels = _TimelineRepo.GetAll();
                ViewBag.TeamMembers = _TeamMemberRepo.GetAll();
                return View(new ContactUsModel());
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

        [HttpPost]
        public IActionResult Contact(ContactUsModel con)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ContactRepo.Add(con);

                    try
                    {
                        SendEmail(con);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }

                    TempData["MsgSent"] = _localizer["Message sent! Thank you"].ToString();
                    return Redirect("/#contact");
                }

                TempData["MsgSent"] = null;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private void SendEmail(ContactUsModel cmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var receiverEmail = new MailAddress("rafiktserunyan@gmail.com", "Rafik Tserunyan");
                    var senderEmail = new MailAddress("carpartsstore.mailer@gmail.com", "Car Parts Store");
                    var password = "CarPartsPwd34";
                    var subject = "User message from Car Parts Store website.";
                    var body = $"Sender: {cmodel.Name} ({cmodel.Email}) \nMessage: {cmodel.Message}\nPhone: {cmodel.Phone}";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
