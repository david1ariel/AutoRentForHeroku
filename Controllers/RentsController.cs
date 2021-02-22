using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeardMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RentsController : ControllerBase
    {
        private readonly RentsLogic rentsLogic;

        public RentsController(RentsLogic rentsLogic)
        {
            this.rentsLogic = rentsLogic;
        }

        [HttpGet]
        [Route("{carId}")]
        [Authorize(Roles ="manager")]
        public IActionResult GetRentsByCarId(string carId)
        {
            try
            {
                List<RentModel> rentModels = rentsLogic.GetRentsByCarId(carId);
                return Ok(rentModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="manager")]
        [HttpGet]
        public IActionResult GetAllRents()
        {
            try
            {
                return Ok(rentsLogic.GetAllRents());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("{carTypeId}/first")]
        [Authorize]
        public IActionResult RentFirstCarAvailable(int CarTypeId, RentModel rentModel)
        {
            try
            {
                return Ok(rentsLogic.RentFirstCarAvailable(CarTypeId, rentModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    
        [HttpPost]
        [Authorize]
        public IActionResult RentCar(RentModel rentModel)
        {
            try
            {
                return Ok(rentsLogic.RentCar(rentModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "manager, employee")]
        [HttpGet]
        [Route("{carId}/current")]
        public IActionResult GetCurrentRentOfCarId(string carId)
        {
            try
            {
                return Ok(rentsLogic.GetCurrentRentOfCarId(carId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="manager, employee")]
        [HttpPatch]
        [Route("{rentId}")]
        public IActionResult ReturnCar(int rentId, RentModel rentModel)
        {
            try
            {
                return Ok(rentsLogic.ReturnCar(rentId, rentModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{userId}/user-rents")]
        public IActionResult GetRentsOfUser(string userId)
        {
            try
            {
                return Ok(rentsLogic.GetRentsOfUser(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "manager")]
        [HttpPut]
        [Route("{rentId}")]
        public IActionResult UpdateRent(int rentId, RentModel rentModel)
        {
            try
            {
                return Ok(rentsLogic.UpdateRent(rentId, rentModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="manager")]
        [HttpDelete]
        [Route("{rentId}")]
        public IActionResult DeleteRent(int rentId)
        {
            try
            {
                rentsLogic.DeleteRent(rentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
