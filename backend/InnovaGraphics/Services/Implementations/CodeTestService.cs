using InnovaGraphics.Interactions;
using InnovaGraphics.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Strategy.Interfaces;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Lab2.Implementations;
using System.Drawing;

namespace InnovaGraphics.Services.Implementations
{
    public class CodeTestService : ICodeTestService
    {
        private readonly IEnumerable<ICodeTest<IEnumerable<Case>, TestResult>> _tests;
        private readonly ICaseRepository _caseRepository;
        
        public CodeTestService(IEnumerable<ICodeTest<IEnumerable<Case>, TestResult>> tests, ICaseRepository caseRepository)
        {
            _tests = tests ?? throw new ArgumentNullException(nameof(tests));
            _caseRepository = caseRepository ?? throw new ArgumentNullException(nameof(caseRepository));
        }

        public async Task<TestResult> CompileAndRunTest(string sourceCode, string testType, Guid exerciseId)
        {
            //Встановлюємо, що поки що все ок і помилок нема
            var overallResult = new TestResult(); 
            overallResult.Success = true; 
            overallResult.CaseResults = new Dictionary<Guid, bool>();
            overallResult.ListErrors = new List<string>(); 

            // Перевіряємо, чи не порожнє id у вправи
            if (exerciseId == Guid.Empty)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Помилка: Не вказано ID вправи (exerciseId є порожнім).");
                return overallResult;
            }

            // Отримуємо тесткейси із бд за ід нашого завдання - то буде перелік об'єктів типу Case
            var testCases = await _caseRepository.GetByExerciseIdAsync(exerciseId);

            // Якщо ми не отримали тестів із бд - помилка
            if (testCases == null || !testCases.Any())
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Помилка: Не знайдено тестових кейсів для вправи з ID {exerciseId} у базі даних.");
                return overallResult;
            }

            // Якщо порожній код - помилка
            if (string.IsNullOrEmpty(sourceCode))
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Помилка: Вихідний код користувача порожній.");
                return overallResult;
            }

            // Парсимо код в дерево виразу для аналізу типу ми компілятор - це з Roslyn
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            // Ці фігні дуже важливі. Коли наш компілятор буде проходитися кодом йому треба знати буквально все. Де що лежить
            // Отже ми задаємо посилання на збірку, де є наша функція для кривої Безьє наприклад
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Globalization.CultureInfo).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(InnovaGraphics.Structs.Point2D).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(BezierParametricTest).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(BezierMatrixTest).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(BezierRecursiveTest).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(InnovaGraphics.Models.Case).Assembly.Location),

                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Bitmap).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(System.Drawing.Color).Assembly.Location),
            };

            // Підвантажуємо опції для компілювання - dll для виконання методів, оптимізація під реліз))
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release);

            // Це для елементів Windows потипу вінформ, апішок і так далі
            var privateWindowsCoreAssembly = Assembly.Load("System.Private.Windows.Core");
            references.Add(MetadataReference.CreateFromFile(privateWindowsCoreAssembly.Location));

            // Створюємо об'єкт компіляції для якого задаємо назву збірки, запарсине дерево, опсилання на інші збірки та опції компіляції
            var compilation = CSharpCompilation.Create(
                $"UserTest_{Guid.NewGuid()}",
                new[] { syntaxTree },
                references,
                compilationOptions
            );

            // Створюємо потік у пам'яті, куди скомпілюється код
            using var ms = new MemoryStream();
            // Отримуємо результат компіляції (успіх/неуспіх)
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Помилки компіляції:");
                var compilationErrors = emitResult.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString())
                    .ToList();
                overallResult.ListErrors.AddRange(compilationErrors);
                return overallResult;
            }

            // Після компіляції вказівник у пам'яті на кінці, тому маємо перенести на початок
            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly;
            try
            {
                // Завантажуємо збірку із пам'яті в об'єкт типу Asembly
                assembly = Assembly.Load(ms.ToArray());
            }
            catch (BadImageFormatException bife)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Помилка завантаження скомпільованої збірки: {bife.Message}. Можливо, збірка пошкоджена або має неправильний формат.");
                return overallResult;
            }
            catch (Exception ex)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Неочікувана помилка при завантаженні скомпільованої збірки: {ex.Message}");
                return overallResult;
            }

            // Беремо тест, який потрібно виконати за назвою
            var testToRun = _tests.FirstOrDefault(t => t.TestName.Equals(testType, StringComparison.OrdinalIgnoreCase));

            // Тест не знайдено
            if (testToRun == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Помилка: Тип тесту '{testType}' не знайдено серед доступних тестів.");
                return overallResult;
            }

            // Отримуємо результати тесту
            TestResult testRunResult;
            try
            {
                // testRun - тип ICode<IEnumerable<Case>, TestResult>
                // Тому тут виконуємо метод TestCompiled
                testRunResult = testToRun.TestCompiled(assembly, testCases);
            }
            catch (Exception ex)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Неочікуваний виняток під час виконання тесту '{testType}': {ex.Message}");
                if (overallResult.CaseResults == null || !overallResult.CaseResults.Any())
                {
                    overallResult.CaseResults = new Dictionary<Guid, bool>();
                    foreach (var caseItem in testCases)
                    {
                        overallResult.CaseResults[caseItem.Id] = false;
                    }
                }
                return overallResult;
            }


            overallResult.Success = overallResult.Success && testRunResult.Success;
            overallResult.ListErrors.AddRange(testRunResult.ListErrors ?? new List<string>());

            if (testRunResult.CaseResults == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Тест '{testType}' повернув null для результатів кейсів.");
                overallResult.CaseResults = new Dictionary<Guid, bool>();
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
            }
            else
            {
                overallResult.CaseResults = testRunResult.CaseResults;
                if (overallResult.CaseResults.Count != testCases.Count())
                {
                    overallResult.Success = false;
                    overallResult.ListErrors.Add($"Тест '{testType}' повернув {overallResult.CaseResults.Count} результатів кейсів, очікувалося {testCases.Count()}.");
                    foreach (var caseItem in testCases)
                    {
                        if (!overallResult.CaseResults.ContainsKey(caseItem.Id))
                        {
                            overallResult.CaseResults[caseItem.Id] = false;
                        }
                    }
                }
            }

            if (overallResult.Success && overallResult.CaseResults.Any() && overallResult.CaseResults.Values.Any(r => !r))
            {
                overallResult.Success = false; 
            }


            return overallResult;
        }
    }
}