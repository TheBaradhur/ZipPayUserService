using Dal.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountEntity>> GetAllAsync();

        Task<AccountEntity> GetByIdAsync(int userId);

        Task<IEnumerable<AccountEntity>> GetByUserIdAsync(int userId);

        Task<AccountEntity> InsertAsync(int userId, decimal CreditAmount);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public AccountRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AccountEntity>> GetAllAsync()
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QueryAsync<AccountEntity>(Sql.GetAll);
            }
        }

        public async Task<AccountEntity> GetByIdAsync(int accountId)
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QuerySingleOrDefaultAsync<AccountEntity>(Sql.GetById, new { id = accountId });
            }
        }

        public async Task<IEnumerable<AccountEntity>> GetByUserIdAsync(int userId)
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QueryAsync<AccountEntity>(Sql.GetByUserId, new { userid = userId });
            }
        }

        public async Task<AccountEntity> InsertAsync(int userId, decimal CreditAmount)
        {
            using (var db = _connectionFactory.Create())
            {
                return await db.QuerySingleOrDefaultAsync<AccountEntity>(Sql.Insert, new
                {
                    userid = userId,
                    originalcreditamount = CreditAmount,
                    currentbalance = CreditAmount
                });
            }
        }

        private static class Sql
        {
            public static readonly string GetAll = $@"

                SELECT id, userid, creditcreationdate, originalcreditamount, currentbalance
                FROM public.account;

            ";

            public static readonly string GetById = $@"

                SELECT id, userid, creditcreationdate, originalcreditamount, currentbalance
                FROM public.account
                WHERE id = @id;

            ";

            public static readonly string GetByUserId = $@"

                SELECT id, userid, creditcreationdate, originalcreditamount, currentbalance
                FROM public.account
                WHERE userid = @userid;

            ";

            public static readonly string Insert = $@"

                INSERT INTO public.account (userid, creditcreationdate, originalcreditamount, currentbalance)
                VALUES(@userid, CURRENT_TIMESTAMP, @originalcreditamount, @currentbalance)

                RETURNING id, userid, creditcreationdate, originalcreditamount, currentbalance;

            ";
        }
    }
}