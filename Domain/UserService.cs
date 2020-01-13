using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.User.Domain.Models;
using ZipPay.User.Infrastructure;
using ZipPay.User.Infrastructure.Models;

namespace ZipPay.User.Domain
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();

        Task<UserEntity> GetUserByIdAsync(int userId);

        Task<UserEntity> CreateNewUserAsync(string emailAddress, decimal monthlySalary, decimal monthlyExpenses);

        Task<BusinessRulesValidation> ValidateCreationInputsAsync(string emailAddress);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllAsync();
        }

        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            return await _userRepo.GetByIdAsync(userId);
        }

        public async Task<UserEntity> CreateNewUserAsync(string emailAddress, decimal monthlySalary, decimal monthlyExpenses)
        {
            return await _userRepo.InsertAsync(emailAddress, monthlySalary, monthlyExpenses);
        }

        public async Task<BusinessRulesValidation> ValidateCreationInputsAsync(string emailAddress)
        {
            var validation = new BusinessRulesValidation(true);

            var emailAlreadyExist = await _userRepo.DoesEmailAlreadyExistsAsync(emailAddress);

            if (emailAlreadyExist)
            {
                validation.IsValid = false;
                validation.ErrorCode = "EmailExists";
                validation.Error = "This email is already used.";
            }

            return validation;
        }
    }
}