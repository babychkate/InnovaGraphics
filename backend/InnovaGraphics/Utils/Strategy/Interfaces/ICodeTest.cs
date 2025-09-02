using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Interfaces
{
    public interface ICodeTest<TRequest, TResponse>
    {
        TResponse TestCompiled(Assembly assembly, TRequest request);
        string TestName { get; }
    }
}
