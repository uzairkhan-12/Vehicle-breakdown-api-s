using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleBreakdownAssistant.Models;
using VehicleBreakdownAssistant.ViewModels;

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

        public IActionResult IsEmailExist([FromBody] EmailValidationViewModel model)
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
