namespace InnovaGraphics.Services.Interfaces
{
    public interface IExistenceChecker<TEntity, TId>
    {
        Task<bool> ExistsAsync(TId id);
        Task<bool> ExistsAsync(string name);
    }
}
