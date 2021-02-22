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
    public class CarTypesController : ControllerBase
    {
        private readonly CarTypesLogic carTypesLogic;

        public CarTypesController(CarTypesLogic carTypesLogic)
        {
            this.carTypesLogic = carTypesLogic;
        }



        [HttpGet]
        public IActionResult GetAllCarTypes()
        {
            try
            {
                return Ok(carTypesLogic.GetAllCarTypes());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public IActionResult AddCarType([FromForm] CarTypeModel carTypeModel)
        {
            try
            {
                CarTypeModel addedCarType = carTypesLogic.AddNewCarType(carTypeModel);
                return Created("api/cartypes/" + addedCarType.CarTypeId, addedCarType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "manager")]
        public IActionResult UpdateCarType(int id, [FromForm] CarTypeModel carTypeModel)
        {
            try
            {
                return Ok(carTypesLogic.UpdateCarType(carTypeModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpPatch]
        //[Route("update-many")]
        //[Authorize(Roles = "manager")]
        //public IActionResult UpdateManyCarTypes([FromForm] List<CarTypeModel> carTypeModels)
        //{
        //    try
        //    {
        //        return Ok(carTypesLogic.UpdateManyCarTypes(carTypeModels));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("images/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            try
            {
                // Open a stream to the file: 
                FileStream fileStream = System.IO.File.OpenRead("Uploads/" + fileName);

                // Send back the stream to the client: 
                return File(fileStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("available")]
        public IActionResult GetAllAvailableCarTypes(RentModel rentModel)
        {
            try
            {
                return Ok(carTypesLogic.GetAllAvailableCarTypes(rentModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "manager")]
        public IActionResult DeleteCarType(int id)
        {
            try
            {
                carTypesLogic.DeleteCarType(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public void Dispose()
        {
            carTypesLogic.Dispose();
        }
    }
}
