using Microsoft.AspNetCore.Mvc;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Models;
using System.Net.Mail;
using System.Net;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactRepository _ContactRepo;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactRepository contactRepo,
                                    ILogger<ContactController> logger)
        {
            _logger = logger;
            _ContactRepo = contactRepo;
        }

        public IActionResult Delete(int messageId)
        {
            try
            {
                _ContactRepo.Delete(messageId);

                return Redirect("/Admin/Home/Index#contact");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public IActionResult ReplyToMail(ContactUsModel cModel, string replyMessage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var receiverEmail = new MailAddress(cModel.Email, cModel.Name);
                    var senderEmail = new MailAddress("carpartsstore.mailer@gmail.com", "Car Parts Store");
                    var password = "CarPartsPwd34";
                    var subject = "Reply to your message from Car Parts Store";
                    var body = $"Replying to you message: '{cModel.Message}' \n {replyMessage}";
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

                    return Redirect("/Admin/Home/Index#contact");
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }        
    }
}
