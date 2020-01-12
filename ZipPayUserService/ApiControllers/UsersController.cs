using Dal.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZipPayUserService.ApiModels;
using ZipPayUserService.Mappers;

namespace ZipPayUserService.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        [HttpGet("/")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<User>>> ListUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users.Select(x => x.ToApiModel()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (id < 1)
            {
                return BadRequest("User Id needs to be more than 0");
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with id {id} not found");
            }

            return Ok(user.ToApiModel());
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var validation = await _userService.ValidateCreationInputsAsync(createUserRequest.EmailAddress);

            if (!validation.IsValid)
            {
                return BadRequest(
                    ApiErrorResponse.GetCustomBadRequest(
                        "One ore more business rules were not respected",
                        HttpContext.TraceIdentifier, 
                        new List<string> { validation.Error }));
            }

            var createdUser = await _userService.CreateNewUserAsync(createUserRequest.EmailAddress, createUserRequest.MonthlySalary, createUserRequest.MonthlyExpenses);

            return StatusCode(201, createdUser.ToApiModel());
        }
    }
}