using InnovaGraphics.Interactions;

namespace InnovaGraphics.Utils.Strategy.Interfaces
{
    public interface ICodeExecutor<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
