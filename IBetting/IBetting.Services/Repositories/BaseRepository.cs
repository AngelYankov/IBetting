using IBetting.DataAccess;

namespace IBetting.Services.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IBettingDbContext dbContext;

        protected BaseRepository(IBettingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
