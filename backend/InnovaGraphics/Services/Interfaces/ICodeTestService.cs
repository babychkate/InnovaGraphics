using InnovaGraphics.Interactions;

namespace InnovaGraphics.Services.Interfaces
{
    public interface ICodeTestService
    {
        Task<TestResult> CompileAndRunTest(string sourceCode, string testType, Guid caseId);
    }
}
