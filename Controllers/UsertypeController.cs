using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleBreakdownAssistant.Models;

namespace VehicleBreakdownAssistant.Controllers
{
    [ApiController]
    [Route("usertype-controller")]
    public class UserTypeController : ControllerBase
    {
       
            public VehicleBreakdownContext context;
            public UserTypeController(VehicleBreakdownContext _context)
            {
                context = _context;
            }
            [HttpPost]
            [Route("add-user-type")]
            public IActionResult AddUserType([FromBody] UserType usertype)
            {

            var userData = context.UserTypes.FirstOrDefault(x => x.UserTypeName == usertype.UserTypeName);
            if (userData == null)
            {
                context.UserTypes.Add(usertype);
                context.SaveChanges();
                return Ok(usertype);
            }
            else
            {
                return Unauthorized();
            }
}
        //post request to post again data by id to toggle the things
        
        [HttpPost]
        [Route("data-toggle")]
        public IActionResult AddDataToggle([FromBody] UserType usertype)
        {
            var updateBoolean = context.UserTypes.FirstOrDefault(x => x.Id == usertype.Id);
            updateBoolean.IsActive = !updateBoolean.IsActive;
            context.SaveChanges();
                return Ok(usertype);
        }


        [HttpGet]
        [Route("get-user-type")]
        public IActionResult GetUserType()
        {
            return Ok(context.UserTypes.ToList());
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromBody] UserType usertype)
        {
            var row = context.UserTypes.FirstOrDefault(x => x.Id == usertype.Id);
            if (row == null) return BadRequest("recored not found");
            var isUserTypeExist = context.UserTypes.FirstOrDefault(x => x.UserTypeName.Equals(usertype.UserTypeName) && x.Id != row.Id);
           /// Console.WriteLine(isUserTypeExist.UserTypeName);
            if (isUserTypeExist != null) return BadRequest("User Type already exist");
            row.UserTypeName = usertype.UserTypeName;
            row.IsActive = usertype.IsActive;
            context.SaveChanges();
            return Ok(usertype);
        }
        [HttpGet]
        [Route("get-active-users")]
        public IActionResult getActiveUsers()
        {
            var result = context.UserTypes.Where(x => x.IsActive == true);
            //result.ToList();
            return Ok(result);

        }
        [HttpGet]
        [Route("get-row-by-id")]
        public IActionResult getRowById(int selectedUserId)
        {
            var row = context.UserTypes.FirstOrDefault(x => x.Id == selectedUserId);
            return Ok(row);
        }
        [HttpDelete]
         [Route("delete")]
         public IActionResult Delete(int userId)
        {
            var row = context.UserTypes.FirstOrDefault(x => x.Id == userId);
            if (row == null) return BadRequest("Record not fount");
            context.UserTypes.Remove(row);
            context.SaveChanges();
            return Ok(row);
        }
        }    
}
