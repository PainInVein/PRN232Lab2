using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Repo.Repositories;

namespace Repositories
{
    public interface IUnitOfWork
    {
        SystemAccountRepository SystemUserAccountRepository { get; }
        TagRepository TagRepository { get; }
        NewsArticleRepository NewsArticleRepository { get; }

        int SaveChangeWithTransaction();
        Task<int> SaveChangeWithTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly Prn312classDbContext _context;
        private SystemAccountRepository _systemAccountRepository;
        private TagRepository _tagRepository;
        private NewsArticleRepository _newsArticleRepository;
        public UnitOfWork() => _context ??= new Prn312classDbContext();

        public SystemAccountRepository SystemUserAccountRepository
        {
            get
            {
                return _systemAccountRepository ??= new SystemAccountRepository(_context);
            }
        }

        public TagRepository TagRepository
        {
            get
            {
                return _tagRepository ??= new TagRepository(_context);
            }
        }

        public NewsArticleRepository NewsArticleRepository
        {
            get { return _newsArticleRepository ??= new NewsArticleRepository(_context); }
        }

        public int SaveChangeWithTransaction()
        {
            int result = -1;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }
            return result;
        }

        public async Task<int> SaveChangeWithTransactionAsync()
        {
            int result = -1;
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    result = -1;
                    await dbContextTransaction.RollbackAsync();
                }
            }
            return result;
        }
    }
}
