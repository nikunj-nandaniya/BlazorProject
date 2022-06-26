using BlazorProject.Server.Models;
using BlazorProject.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employees : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public Employees(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
            {
                var result = await employeeRepository.Search(name, gender);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving the data.");
            }
        }

        [HttpGet]
        public async Task<ActionResult>GetEmployess()
        {
            try
            {
                return Ok(await employeeRepository.GetEmployees());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving the date");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>>GetEmployee(int id)
        {
            try
            {
                var result = await employeeRepository.GetEmployee(id);

                if (result == null)
                { 
                    return NotFound();
                }

                return Ok(result);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving the date");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>>CreateEmployee(Employee employee)
        {
            try
            {                
                if(employee == null)
                    return BadRequest();

                var emp = await employeeRepository.GetEmployeeByEmail(employee.Email);

                if (emp != null)
                {
                    ModelState.AddModelError("Email", "Employee email is already in use.");
                    return BadRequest(ModelState);
                }

                var createdemployee = await employeeRepository.AddEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee),
                       new { id = createdemployee.EmployeeId },createdemployee);                               
                
            }
            catch (Exception)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the employee.");             
            }
        }        

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>>UpdateEmployee(int id,Employee employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();

                if (id != employee.EmployeeId)
                    return BadRequest("EmployeeId mismatch");

                var employeeToUpdate = await employeeRepository.GetEmployee(id);

                if (employeeToUpdate == null)
                {
                    return NotFound($"Employee with id = {id} not found.");
                }

                return await employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the employee.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult>DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await employeeRepository.GetEmployee(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with id = {id} not found.");
                }

                await employeeRepository.DeleteEmployee(id);

                return Ok($"Employee with id = {id} deleted.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the employee.");
            }
        }
    }
}
