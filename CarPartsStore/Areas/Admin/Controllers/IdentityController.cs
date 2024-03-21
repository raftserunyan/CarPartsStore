using CarPartsStore.Models;
using Microsoft.AspNetCore.Mvc;
using CarPartsStore.Repositories.Interfaces;
using System.Collections.Generic;
using CarPartsStore.ViewModels;
using System.Security.Claims;
using CarPartsStore.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.RegularExpressions;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IdentityController : Controller
    {
        private readonly IAdminRepository _AdminRepo;
        private readonly IAdminCandidateRepository _CandidateRepo;
        private readonly ILogger<IdentityController> _logger;
        private readonly IStringLocalizer<IdentityController> _localizer;

        public IdentityController(IAdminRepository adminRepo,
                                    IAdminCandidateRepository candidateRepo,
                                    ILogger<IdentityController> logger,
                                    IStringLocalizer<IdentityController> localizer)
        {
            _AdminRepo = adminRepo;
            _CandidateRepo = candidateRepo;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("/Identity/Login")]
        public IActionResult Login()
        {
            try
            {
                return View(new LoginViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Admin/Identity/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AdminUser user;

                    try
                    {
                        user = _AdminRepo.Get(model.Email);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("InvalidEmail", _localizer["*Invalid Email"]);
                        model.Password = "";
                        return View(model);
                    }

                    if (PasswordService.VerifyHashedPassword(user.PasswordHash, model.Password))
                    {
                        await Authenticate(model.Email);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("InvalidPassword", _localizer["*Invalid password"]);
                        model.Password = "";
                        return View(model);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        [HttpGet]
        [Route("/Identity/SignUp")]
        public IActionResult SignUp()
        {
            try
            {
                return View(new SignUpViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Admin/Identity/SignUp")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_AdminRepo.Exists(model.Email))
                    {
                        ModelState.AddModelError("UserAlreadyExists", _localizer["*This email is already registered"]);
                        return View(model);
                    }
                    if (_CandidateRepo.Exists(model.Email))
                    {
                        ModelState.AddModelError("PendingRequest", _localizer["*A request for this email is already pending"]);
                        return View(model);
                    }

                    string errorMessage;
                    if(!ValidatePassword(model.Password, out errorMessage))
                    {
                        ModelState.AddModelError("PasswordError", errorMessage);
                        return View(model);
                    }

                    var candidate = new AdminCandidate
                    {
                        GuID = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password
                    };                    

                    _CandidateRepo.Add(candidate);

                    //Send confirmation email
                    SendEmail(candidate, Request.Host.Value);

                    return View("RequestSent");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/Identity/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect("/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [Route("/Identity/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string id)
        {
            try
            {
                AdminCandidate cand = _CandidateRepo.Get(id);
                AdminUser admin = new AdminUser
                {
                    UserName = cand.Name,
                    Email = cand.Email,
                    PasswordHash = PasswordService.HashPassword(cand.Password)
                };

                _CandidateRepo.Delete(cand.GuID);
                _AdminRepo.Add(admin);

                SendNotification(admin.UserName, admin.Email, Request.Host.Value, true);

                ViewData["IsRequestAccepted"] = true;
                return View("RequestResponse", cand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [Route("/Identity/DeclineEmail")]
        public async Task<IActionResult> DeclineEmail(string id)
        {
            try
            {
                AdminCandidate cand;
                if (_CandidateRepo.ExistsId(id))
                {
                    cand = _CandidateRepo.Get(id);
                }
                else
                {
                    throw new Exception($"Candidate with id {id} does not exist");
                }

                _CandidateRepo.Delete(cand.GuID);

                SendNotification(cand.Name, cand.Email, Request.Host.Value, false);

                ViewData["IsRequestAccepted"] = false;
                return View("RequestResponse", cand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        private async Task Authenticate(string userName)
        {
            try
            {
                // создаем один claim
                var claims = new List<Claim>
                {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
                };
                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendEmail(AdminCandidate model, string domain)
        {
            try
            {
                string msg = "<div style=\"width: 100%; display: flex; align-items: center; justify-content: center; background-image: url(https://www.muralswallpaper.com/app/uploads/Green-Tropical-Plant-Wallpaper-Mural-Plain-820x532.jpg)\">"
                                + "<div style =\"width: 80%; display: flex; flex-direction: column; align-items: center; justify-content: center; font-family: sans-serif; font-weight: 600; color: #fff; background-color: rgba(0,0,0,.8); padding: 1em;\">"
                                        + "<h3> User registration request!</h3>"
                                        + "<div style =\"display: flex; flex-direction: column; align-items: center; justify-content: center;\">"
                                            + $"<p style =\"width: 80%; text-align: center;\">User <b>{model.Name}</b> wants to create an account in CarPartsStore.</p>"
                                            + $"<p> Email: {model.Email}</p>"
                                            + "<p style =\"color: gray; font-weight: 700;\">Do you want to allow him/her create the account?</p>"
                                        + "</div>"
                                        + "<div style =\"width: 100%; display: flex; align-items: center; justify-content: center;\">"
                                            + "<div style =\"display: flex; flex-direction: row; align-items: center; justify-content: space-around; width: 30%;\">"
                                                + $"<a href =\"http://{domain}/Identity/ConfirmEmail?id={model.GuID}\" style=\"text-decoration: none; width: 90%; height: 8vh; background-color: #fed136; color: #fff; text-transform: uppercase; font-weight: 800; display: flex; align-items: center; justify-content: center; border-radius: 0.7em; border: 2px solid #fff; margin-right: 0.5em;\">Confirm</a>"
                                                + $"<a href =\"http://{domain}/Identity/DeclineEmail?id={model.GuID}\" style=\"text-decoration: none; width: 90%; height: 8vh; background-color: gray; color: #fff; text-transform: uppercase; font-weight: 800; display: flex; align-items: center; justify-content: center; border-radius: 0.7em; border: 2px solid #fff; margin-left: 0.5em;\">  Deny  </a>"
                                            + "</div>"
                                        + "</div>"
                                + "</div>"
                            + "</div>";                
                var receiverEmail = new MailAddress("rafarman11@gmail.com", "Raf Arman");
                var senderEmail = new MailAddress("carpartsstore.mailer@gmail.com", "Car Parts Store");
                var password = "CarPartsPwd34";
                var subject = "Confirm user registration.";
                var body = msg;
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
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendNotification(string name, string email, string domain, bool isAccepted)
        {
            try
            {
                var receiverEmail = new MailAddress($"{email}", $"{email}");
                var senderEmail = new MailAddress("carpartsstore.mailer@gmail.com", "Car Parts Store");
                var password = "CarPartsPwd34";
                string subject;
                string body;

                if (isAccepted)
                {
                    subject = "Your account has been created!";
                    body = "<p>Hi there! Your request about creating an account in CarPartsStore has been <span style=\"color: green;\">accepted!</span></p>"
                    + "<p>To log into your account,simply click the button below...</p>"
                    + "<a href =\"http://{domain}/Identity/Login\" style=\"text-decoration: none; width: 20vw; height: 8vh; background-color: #fed136; color: #fff; text-transform: uppercase; font-weight: 800; border-radius: 0.7em; border: 2px solid #fff;\">Log In</a>";
                }
                else
                {
                    subject = "Your account has not been created.";
                    body = "<p>Hi there! Unfortunately, your request about creating an account in CarPartsStore has been <span style=\"color: red;\">denied</span>.</p>"
                    + "<p>We're sorry about that, but you can try signing up again by clicking the button below...</p>"
                    + "<a href =\"http://{domain}/Identity/SignUp\" style=\"text-decoration: none; width: 20vw; height: 8vh; background-color: #fed136; color: #fff; text-transform: uppercase; font-weight: 800; border-radius: 0.7em; border: 2px solid #fff;\">Sign Up</a>";
                }

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
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "*Password must contain at least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "*Password must contain at least one upper case letter";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "*Password should not be less than 8 or greater than 15 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "*Password should contain At least one numeric value";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
