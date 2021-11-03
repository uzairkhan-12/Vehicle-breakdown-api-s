using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleBreakdownAssistant.Models;
using VehicleBreakdownAssistant.ViewModels;
using System.Net;
using System.Net.Mail;

namespace VehicleBreakdownAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        public VehicleBreakdownContext context;
        public AccountController(VehicleBreakdownContext _context)
        {
            context = _context;
        }
        [HttpPost]
        [Route("add")]
        
        public IActionResult AddDetail([FromBody] Users user) {
            var userData = context.Users.FirstOrDefault(x => x.Email == user.Email);
            if (userData == null)
            {
            context.Users.Add(user);
                context.SaveChanges();
                return Ok(user);
            }
            else
            {
                return Unauthorized();
            }
            
        }
        [HttpGet]
        [Route("get-detail")]
        public IActionResult GetAllDetail()
        {
            return Ok(context.Users.ToList());
        }
        [HttpPost]
        [Route("is-email-exist")]

        public IActionResult IsEmailExistIsEmailExist([FromBody] EmailValidationViewModel model)
        {
            var user = context.Users.FirstOrDefault(x => x.Email ==model.EmailAddress );
            if(user == null)
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }
        }
        [HttpPost]
        [Route("forgot-password")]
        public IActionResult forgotPasswordEmail([FromBody] EmailValidationViewModel model)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == model.EmailAddress);
            if(user == null)
            {
                return Ok(false);
            }
            else
            {
                //return Ok(true);
                var securityKey = Guid.NewGuid().ToString();
                var secretKeysl = new SecretKeys()
                {
                    SecretKey = securityKey,
                    UserId = user.Id
                };
                context.SecretKeys.Add(secretKeysl);
                context.SaveChanges();

                var from = new MailAddress("noreply@gmail.com");
                var to = new MailAddress(model.EmailAddress);
                using (MailMessage mm = new MailMessage(from, to))
                {
                    mm.Subject = "Varify Your Email Address"; //model.Subject;
                   // mm.Body = "Email body";//model.Body;

                    var body = "<h2>";
                    body += "<h3>Varify Your Email Address</h3>";
                    body +="<p>To Varify Your Email Address, click on the below link</p>";
                    //body += "<a target='_blank' href='http://localhost:3000/reset-password/" + securityKey+"'>here</a>";
                    body += string.Format(@"<a href='http://localhost:3000/reset-password/{0}'>Here</a>", securityKey);
                    body += "<p> If you dont request this code,you can safely ignore this email </p>";
                    body += "<p> Someone else might have typed your email address by mistake</p>";
                    body += "<h4> Thanks,</h4>";
                    body += "<h4> Vehicle Breakdown Assistant</h4>";
                    body += "</h2>";

                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("pk.uzikhan@gmail.com", "pakkaya@12");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                        //       ViewBag.Message = "Email sent.";
                        return Ok(true);
                    }
                }
            }
        }

        [HttpPost]
        [Route("update-password")]
        public IActionResult updatePassword([FromBody] ResetPasswordViewModel resetPasswordViewModel)
        {
            var secretKey = context.SecretKeys.FirstOrDefault(x => x.SecretKey == resetPasswordViewModel.key);
            if(secretKey == null)
            {
                return Ok(false);
            }
            var user = context.Users.FirstOrDefault(x => x.Id == secretKey.UserId);
            if (user == null) BadRequest("user does not exist");
            user.Password = resetPasswordViewModel.newPassword;
            context.SecretKeys.Remove(secretKey);
            context.SaveChanges();
            return Ok(true);
        }

        [HttpPost]
        [Route("log-in")]
        public IActionResult isUserExist([FromBody] LogInViewModel logInModel)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == logInModel.EmailAddress && x.Password == logInModel.Password);
            if(user== null)
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }
        }
    }
}
