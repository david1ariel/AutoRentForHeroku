using System;
using System.Collections.Generic;
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
    
    public class CarsController : ControllerBase
    {
        private readonly CarsLogic carsLogic;

        public CarsController(CarsLogic carsLogic)
        {
            this.carsLogic = carsLogic;
        }



        [HttpGet]
        public IActionResult GetAllCars()
        {
            try
            {
                return Ok(carsLogic.GetAllCars());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("available")]
        public IActionResult GetAllAvailableCars(RentModel rentModel)
        {
            try
            {
                List<CarModel> availableCars = carsLogic.GetAllAvailableCars(rentModel);
                return Ok(availableCars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{carId}")]
        public IActionResult GetSingleCar(string carId)
        {
            try
            {
                return Ok(carsLogic.GetSingleCar(carId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("by-cartype/{carTypeId}")]
        public IActionResult GetCarsByCarType(int carTypeId)
        {
            try
            {
                return Ok(carsLogic.GetCarsByCarTypeId(carTypeId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "manager")]
        [HttpPost]
        [Authorize]
        public IActionResult AddCar(CarModel carModel)
        {
            try
            {
                return Ok(carsLogic.AddCar(carModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{carId}")]
        [Authorize(Roles = "manager")]
        public IActionResult UpdateCar(string carId, CarModel carModel)
        {
            try
            {
                carModel.CarId = carId;
                CarModel updatedCar = carsLogic.UpdateCar(carModel);
                //if (updatedCar == null)
                //    return NotFound($"id {carId} not found");
                return Ok(updatedCar); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{carId}")]
        [Authorize(Roles = "manager")]
        public IActionResult DeleteCar(string carId)
        {
            try
            {
                carsLogic.DeleteCar(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        

        public void Dispose()
        {
            carsLogic.Dispose();
        }
    }
}
