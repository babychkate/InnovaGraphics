namespace InnovaGraphics.Utils.Validators.Interfaces
{
    public interface IValidator<T, TId>
    {
        Task<Dictionary<string, List<string>>> ValidateAsync(T entity, TId id);
    }
}
