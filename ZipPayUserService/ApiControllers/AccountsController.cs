using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZipPay.User.Domain;
using ZipPay.User.Web.ApiModels;
using ZipPay.User.Web.Mappers;

namespace ZipPay.User.Web.ApiControllers
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
            try
            {
                var users = await _accountService.GetAllAccountsAsync();

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
        public async Task<ActionResult<ApiModels.User>> GetAccount(int id)
        {
            if (id < 1)
            {
                return BadRequest(
                    ApiErrorResponse.GetCustomBadRequest(
                        "One or more validation errors occurred.",
                        HttpContext.TraceIdentifier,
                        new List<string> { "Account Id needs to be more than 0" }));
            }

            try
            {
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
        public async Task<ActionResult<Account>> CreateAccount([FromBody] CreateAccountRequest createAccountRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
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