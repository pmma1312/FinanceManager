using FinanceManager.Infrastructure.Context;
using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetByUsername(string username);
        public Task<User> GetByEmail(string email);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository(FinanceManagerContext context) : base(context) { }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.Where(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        }

    }
}
