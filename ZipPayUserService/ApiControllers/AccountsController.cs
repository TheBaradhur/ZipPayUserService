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
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("")]
        [HttpGet("/")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Account>>> ListUsers()
        {
            var users = await _accountService.GetAllAccountsAsync();

            return Ok(users.Select(x => x.ToApiModel()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAccount(int id)
        {
            if (id < 1)
            {
                return BadRequest(
                    ApiErrorResponse.GetCustomBadRequest(
                        "One or more validation errors occurred.",
                        HttpContext.TraceIdentifier,
                        new List<string> { "Account Id needs to be more than 0" }));
            }

            var account = await _accountService.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound(
                    ApiErrorResponse.GetCustomNotFound(
                        "Requested resource not found.",
                        HttpContext.TraceIdentifier,
                        new List<string> { $"Account with id {id} not found" })
                    );
            }

            return Ok(account.ToApiModel());
        }

        [HttpPost("create")]
        public async Task<ActionResult<Account>> CreateAccount([FromBody] CreateAccountRequest createAccountRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var validation = await _accountService.ValidateCreationInputsAsync(createAccountRequest.UserId, createAccountRequest.RequestedCreditAmount);

            if (!validation.IsValid)
            {
                return BadRequest(
                    ApiErrorResponse.GetCustomBadRequest(
                        "One or more business rules were not respected.",
                        HttpContext.TraceIdentifier,
                        new List<string> { validation.Error }));
            }

            var createdAccount = await _accountService.CreateNewAccountAsync(createAccountRequest.UserId, createAccountRequest.RequestedCreditAmount);

            return StatusCode(201, createdAccount.ToApiModel());
        }
    }
}