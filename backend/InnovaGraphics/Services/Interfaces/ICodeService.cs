
namespace InnovaGraphics.Services.Interfaces
{
    public interface ICodeService<TRequest, TResult>
    {
        Task<TResult> ProcessCodeAsync(TRequest request);
    }
}
