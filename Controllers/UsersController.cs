using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeardMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersLogic usersLogic;

        public UsersController(UsersLogic usersLogic)
        {
            this.usersLogic = usersLogic;
        }


        [Authorize(Roles = "manager")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(usersLogic.GetAllUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        

        [HttpPost]
        public IActionResult AddUser([FromForm]UserModel userModel)
        {
            try
            {
                UserModel addedUser = usersLogic.AddUser(userModel);
                return Created("https://localhost:44306/api/users/" + addedUser.UserId, addedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromForm]UserModel userModel)
        {
            try
            {
                return Ok(await usersLogic.UpdateUser(userModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                usersLogic.DeleteUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="manager, employee")]
        [HttpGet]
        [Route("renting-client/{rentId}")]
        public IActionResult GetRentingClient(int rentId)
        {
            try
            {
                return Ok(usersLogic.GetRentingClient(rentId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public void Dispose()
        {
            usersLogic.Dispose();
        }
    }
}
