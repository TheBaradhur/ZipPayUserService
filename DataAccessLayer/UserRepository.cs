using Dal.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
    }

    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QueryAsync<User>(Sql.GetAll);
            }
        }

        private static class Sql
        {
            public static readonly string GetAll = $@"

                SELECT id, emailaddress, monthlysalary, Monthlyexpenses
                FROM public.user;

            ";
        }
    }
}