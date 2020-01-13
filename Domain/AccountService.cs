using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZipPay.User.Domain.Models;
using ZipPay.User.Infrastructure;
using ZipPay.User.Infrastructure.Models;

namespace ZipPay.User.Domain
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountEntity>> GetAllAccountsAsync();

        Task<AccountEntity> GetAccountByIdAsync(int userId);

        Task<IEnumerable<AccountEntity>> GetAccountsByUserIdAsync(int userId);

        Task<AccountEntity> CreateNewAccountAsync(int userId, decimal TotalCreditAmount);

        Task<BusinessRulesValidation> ValidateCreationInputsAsync(int userId, decimal requestedCreditAmount);
    }

    public class AccountService : IAccountService
    {
        public const decimal MinimumCreditAllowance = 1000m;

        public const decimal MaximumCreditValue = 1000m;

        public const decimal MinimumCreditValue = 10m;

        private readonly IAccountRepository _accountRepo;

        private readonly IUserService _userService;

        public AccountService(IAccountRepository accountRepo, IUserService userService)
        {
            _accountRepo = accountRepo;
            _userService = userService;
        }

        public async Task<IEnumerable<AccountEntity>> GetAllAccountsAsync()
        {
            return await _accountRepo.GetAllAsync();
        }

        public async Task<AccountEntity> GetAccountByIdAsync(int accountId)
        {
            return await _accountRepo.GetByIdAsync(accountId);
        }

        public async Task<IEnumerable<AccountEntity>> GetAccountsByUserIdAsync(int userId)
        {
            return await _accountRepo.GetByUserIdAsync(userId);
        }

        public async Task<AccountEntity> CreateNewAccountAsync(int userId, decimal TotalCreditAmount)
        {
            return await _accountRepo.InsertAsync(userId, TotalCreditAmount);
        }

        public async Task<BusinessRulesValidation> ValidateCreationInputsAsync(int userId, decimal requestedCreditAmount)
        {
            if (requestedCreditAmount < MinimumCreditValue || requestedCreditAmount > MaximumCreditValue)
            {
                return new BusinessRulesValidation(false, "CreditAmountInvalid", "The credit can only be between 10 and 1000 AUD (included).");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new BusinessRulesValidation(false, "UserNotFound", $"The user with id {userId} does not exist.");
            }

            if (user.OverallCreditAllowance < MinimumCreditAllowance)
            {
                return new BusinessRulesValidation(
                    false,
                    "NotEnoughCreditAllowance",
                    $"The user allowance should be at least {MinimumCreditAllowance.ToString("G")} AUD, currently at {user.OverallCreditAllowance.ToString("G")}");
            }

            var otherOpenAccounts = await GetAccountsByUserIdAsync(userId);
            var sumOfOpenAccountBalance = otherOpenAccounts.Select(x => x.CurrentBalance).Sum();
            if (user.OverallCreditAllowance - sumOfOpenAccountBalance < MinimumCreditAllowance)
            {
                return new BusinessRulesValidation(
                    false,
                    "NotEnoughCreditAllowance",
                    $"The user already has open accounts for a total of {sumOfOpenAccountBalance.ToString("G")} AUD, " +
                       $"which places him under the minimum credit allowance of {MinimumCreditAllowance.ToString("G")} AUD.");
            }

            return new BusinessRulesValidation(true);
        }
    }
}