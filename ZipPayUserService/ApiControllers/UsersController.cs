using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
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
            try
            {
                var users = await _userService.GetAllUsersAsync();

                return Ok(users.Select(x => x.ToApiModel()));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiErrorResponse.GetCustomInternalServerError(
                        "An unexpected error occured. Please contact API team.",
                        HttpContext.TraceIdentifier,
                        new List<string> { e.Message }));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (id < 1)
            {
                return BadRequest(
                    ApiErrorResponse.GetCustomBadRequest(
                        "One or more validation errors occurred.",
                        HttpContext.TraceIdentifier,
                        new List<string> { "User Id needs to be more than 0" }));
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(
                        ApiErrorResponse.GetCustomNotFound(
                            "Requested resource not found.",
                            HttpContext.TraceIdentifier,
                            new List<string> { $"User with id {id} not found" })
                    );
                }

                return Ok(user.ToApiModel());
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiErrorResponse.GetCustomInternalServerError(
                        "An unexpected error occured. Please contact API team.",
                        HttpContext.TraceIdentifier,
                        new List<string> { e.Message }));
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var validation = await _userService.ValidateCreationInputsAsync(createUserRequest.EmailAddress);

                if (!validation.IsValid)
                {
                    return BadRequest(
                        ApiErrorResponse.GetCustomBadRequest(
                            "One or more business rules were not respected.",
                            HttpContext.TraceIdentifier,
                            new List<string> { validation.Error }));
                }

                var createdUser = await _userService.CreateNewUserAsync(createUserRequest.EmailAddress, createUserRequest.MonthlySalary, createUserRequest.MonthlyExpenses);

                return StatusCode(201, createdUser.ToApiModel());
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiErrorResponse.GetCustomInternalServerError(
                        "An unexpected error occured. Please contact API team.",
                        HttpContext.TraceIdentifier,
                        new List<string> { e.Message }));
            }
        }
    }
}