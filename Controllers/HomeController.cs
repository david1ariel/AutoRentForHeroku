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
    public class HomeController : ControllerBase
    {
        private readonly HomeLogic homeLogic;

        public HomeController(HomeLogic homeLogic)
        {
            this.homeLogic = homeLogic;
        }



        [HttpGet]
        public IActionResult GetHomeObject()
        {
            try
            {
                HomeModel homeModel = homeLogic.GetHomeObject();
                return Ok(homeModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles="manager")]
        [HttpPatch]
        public IActionResult UpdateHome([FromForm] HomeModel homeModel)
        {
            try
            {
                return Ok(homeLogic.UpdateHome(homeModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        

        public void Dispose()
        {
            homeLogic.Dispose();
        }
    }
}
