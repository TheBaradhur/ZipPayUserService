using Dal.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllAsync();

        Task<UserEntity> GetByIdAsync(int userId);

        Task<UserEntity> InsertAsync(string emailAddress, decimal monthlySalary, decimal monthlyExpenses);

        Task<bool> DoesEmailAlreadyExistsAsync(string emailToCheck);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QueryAsync<UserEntity>(Sql.GetAll);
            }
        }

        public async Task<UserEntity> GetByIdAsync(int userId)
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QuerySingleOrDefaultAsync<UserEntity>(Sql.GetById, new { id = userId });
            }
        }

        public async Task<UserEntity> InsertAsync(string emailAddress, decimal monthlySalary, decimal monthlyExpenses)
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QuerySingleOrDefaultAsync<UserEntity>(Sql.Insert, new { emailaddress = emailAddress, monthlysalary = monthlySalary, monthlyexpenses = monthlyExpenses} );
            }
        }

        public async Task<bool> DoesEmailAlreadyExistsAsync(string emailToCheck)
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.ExecuteScalarAsync<bool>(Sql.CheckEmailAlreadyExist, new { emailaddress = emailToCheck });
            }
        }

        private static class Sql
        {
            public static readonly string GetAll = $@"

                SELECT id, emailaddress, monthlysalary, Monthlyexpenses
                FROM public.user;

            ";

            public static readonly string GetById = $@"

                SELECT id, emailaddress, monthlysalary, Monthlyexpenses
                FROM public.user
                WHERE id = @id;

            ";

            public static readonly string Insert = $@"

                INSERT INTO public.user (emailaddress, monthlysalary, Monthlyexpenses)
                VALUES (@emailaddress, @monthlysalary, @monthlyexpenses)

                RETURNING id, emailaddress, monthlysalary, Monthlyexpenses;

            ";

            public static readonly string CheckEmailAlreadyExist = $@"

                SELECT EXISTS(
                    SELECT 1 
                    FROM public.user 
                    WHERE emailaddress = @emailaddress);

            ";
        }
    }
}