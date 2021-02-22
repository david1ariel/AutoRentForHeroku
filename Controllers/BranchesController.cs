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
    //[EnableCors]
    public class BranchesController : ControllerBase, IDisposable
    {
        private readonly BranchesLogic branchesLogic;

        public BranchesController(BranchesLogic branchesLogic)
        {
            this.branchesLogic = branchesLogic;
        }



        [HttpGet]
        public IActionResult GetAllBranches()
        {
            try
            {
                return Ok(branchesLogic.GetAllBranches());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "manager")]
        [HttpPost]
        public IActionResult AddBranch(BranchModel branchModel)
        {
            try
            {
                BranchModel addedBranch = branchesLogic.AddBranch(branchModel);
                return Created("https://localhost:44306/api/branches/" + branchModel.BranchId, branchModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "manager")]
        [HttpPut]
        [Route("{branchId}")]
        public IActionResult UpdateBranch(int branchId, BranchModel branchModel)
        {
            try
            {
                branchModel.BranchId = branchId;
                BranchModel updatedBranch = branchesLogic.UpdateBranch(branchModel);
                if (updatedBranch == null)
                    return NotFound($"id {branchId} not found");
                return Ok(updatedBranch);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "manager")]
        [HttpDelete]
        [Route("{branchId}")]
        public IActionResult DeleteBranch(int branchId)
        {
            try
            {
                branchesLogic.DeleteBranch(branchId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public void Dispose()
        {
            branchesLogic.Dispose();
        }

    }
}
