using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeardMan
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("EntireWorld")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelper jwtHelper;
        private readonly UsersLogic usersLogic;


        public AuthController(JwtHelper jwtHelper, UsersLogic usersLogic)
        {
            this.jwtHelper = jwtHelper;
            this.usersLogic = usersLogic;
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromForm] UserModel user)
        {
            if (usersLogic.isUserNameExists(user.Username))
                return BadRequest("user name allready exists");

            user = usersLogic.AddUser(user);

            user.JwtToken = jwtHelper.GetWjtToken(user.Username, user.Role);

            user = (UserModel)user.Clone();
            user.Password = null;

            return Created("api/users/" + user.UserId, user);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(CredentialsModel credentials)
        {

            UserModel user = usersLogic.GetUserByCredentials(credentials);

            if (user == null)
                return Unauthorized("incorrect username or password");

            user.JwtToken = jwtHelper.GetWjtToken(user.Username, user.Role);

            user = (UserModel)user.Clone();
            user.Password = null;

            return Ok(user);
        }
    }
}
