using Dal;
using Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllAsync();
        }
    }
}