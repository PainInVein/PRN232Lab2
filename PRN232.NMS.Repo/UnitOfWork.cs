using PRN232.NMS.Repo.DBContext;
using Repositories.Repositories;

namespace Repositories
{
    public interface IUnitOfWork
    {
        SystemAccountRepository SystemUserAccountRepository { get; }

        int SaveChangeWithTransaction();
        Task<int> SaveChangeWithTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly Prn312classDbContext _context;
        private SystemAccountRepository _systemAccountRepository;
        public UnitOfWork() => _context ??= new Prn312classDbContext();

        public SystemAccountRepository SystemUserAccountRepository
        {
            get
            {
                return _systemAccountRepository ?? new SystemAccountRepository(_context);
            }
        }


        public int SaveChangeWithTransaction()
        {
            int result = 0;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = 0;
                    dbContextTransaction.Rollback();
                }
            }
            return result;
        }

        public async Task<int> SaveChangeWithTransactionAsync()
        {
            int result = 0;

            using (var dbContextTransaction = _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    dbContextTransaction.Result.CommitAsync();
                }
                catch (Exception)
                {
                    result = 0;
                    dbContextTransaction.Result.RollbackAsync();
                }
            }

            return result;
        }
    }
}
