using BlazorProject.Server.Models;
using BlazorProject.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Departments : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;

        public Departments(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>>GetDepartment(int id)
        {
            try
            {
                var result = await departmentRepository.GetDepartment(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving the data.");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetDepartments()
        {
            try
            {
                return Ok(await departmentRepository.GetDepartments());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving the data.");
            }
        }
    }
}
