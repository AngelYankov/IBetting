namespace IBetting.DataAccess.Repositories
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
