using Dal;
using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();

        Task<UserEntity> GetUserByIdAsync(int userId);

        Task<UserEntity> CreateNewUserAsync(string emailAddress, decimal monthlySalary, decimal monthlyExpenses);
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
    }
}