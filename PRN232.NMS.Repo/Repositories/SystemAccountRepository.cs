using EVCMS.Repositories.BinhLS.Basic;
using Microsoft.EntityFrameworkCore;
using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Repo.EntityModels;

namespace Repositories.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>
    {
        public SystemAccountRepository() { }
        public SystemAccountRepository(Prn312classDbContext context) : base(context) { }

        public async Task<SystemAccount> GetUsernameAsync(string username, string password)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(ua => ua.AccountName == username
                                                                && ua.AccountPassword == password);
        }

        public async Task<SystemAccount> GetAllUserAsync()
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync();
        }
    }
}
