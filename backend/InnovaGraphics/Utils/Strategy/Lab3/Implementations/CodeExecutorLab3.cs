using InnovaGraphics.Interactions;
using InnovaGraphics.Utils.Strategy.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Text.Json;

namespace InnovaGraphics.Utils.Strategy.Lab3.Implementations
{
    public class CodeExecutorLab3 : ICodeExecutor<CodeRequest, CodeResponse>
    {
        public async Task<CodeResponse> ExecuteAsync(CodeRequest request)
        {
            // Синтаксичне дерево для виконання компіляції
            var syntaxTree = CSharpSyntaxTree.ParseText(request.SourceCode);
            // Перевірка наявності методу DrawTree та перевірка depth
            var root = await syntaxTree.GetRootAsync();
            var drawTreeInvocations = root
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(inv => inv.Expression is IdentifierNameSyntax identifier &&
                              identifier.Identifier.Text == "DrawTree");

            bool drawTreeNotCalled = false;
            bool depthValid = false;

            foreach (var invocation in drawTreeInvocations)
            {
                drawTreeNotCalled = false;
                var arguments = invocation.ArgumentList.Arguments;

                if (arguments.Count >= 2)
                {
                    var depthArg = arguments[1].Expression;

                    if (depthArg is LiteralExpressionSyntax literal &&
                        literal.IsKind(SyntaxKind.NumericLiteralExpression))
                    {
                        int depthValue = (int)literal.Token.Value!;
                        if (depthValue > 19)
                        {
                            depthValid = true;
                            break;
                        }
                    }
                }
            }

            if (drawTreeNotCalled)
            {
                return new CodeResponse
                {
                    OutputText = "Помилка: Метод DrawTree не викликано.",
                    CurvePoints = new List<double[]>(),
                    Success = false
                };
            }

            if (depthValid)
            {
                return new CodeResponse
                {
                    OutputText = "Помилка: Значення глибини для DrawTree перевищує 19.",
                    CurvePoints = new List<double[]>(),
                    Success = false
                };
            }

            // Список бібліотек
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)

            };

            // Створення збірки з опціями
            var compilation = CSharpCompilation.Create(
                assemblyName: $"UserCode_{Guid.NewGuid()}",
                syntaxTrees: [syntaxTree],
                references: references,
                // Створити exe файл із точкою входу Main()
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            // Записуємо скомпільований файл у пам'ять
            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            // Діагностика
            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                    .Where(diag => diag.Severity == DiagnosticSeverity.Error)
                    .Select(diag => diag.ToString())
                    .ToArray();

                return new CodeResponse
                {
                    OutputText = "Помилка компіляції:\n" + string.Join("\n", errors),
                    CurvePoints = new List<double[]>(),
                    Success = false
                };
            }

            // Переходимо на початок збірки
            ms.Seek(0, SeekOrigin.Begin);

            Assembly assembly;
            try
            {
                // Завантажуємо збірку
                assembly = Assembly.Load(ms.ToArray());
            }
            catch (Exception ex)
            {
                return new CodeResponse
                {
                    OutputText = $"Помилка в завантаженні збірки: {ex.Message}",
                    CurvePoints = new List<double[]>(),
                    Success = false
                };
            }

            // Шукаємо точку входу Main
            var entry = assembly.EntryPoint;
            if (entry == null)
            {
                return new CodeResponse
                {
                    OutputText = "Main не знайдено в скомпільованій збірці.",
                    CurvePoints = new List<double[]>(),
                    Success = false
                };
            }

            // Записуємо код у пам'ять
            using var writer = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(writer);

            List<double[]> curvePointsList = new List<double[]>();
            string output = "";
            bool runtimeErrorOccurred = false;

            try
            {
                var parameters = entry.GetParameters().Length == 1 ? new object[] { new string[0] } : null;
                var resultObj = entry.Invoke(null, parameters);

                if (resultObj is Task task)
                    await task;

                output = writer.ToString();

                var errorMessages = new List<string>();
                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var pointStr = line.Trim(); // Просто видаляємо пробіли на початку та в кінці
                    try
                    {
                        var pointValues = pointStr.Split(',')
                            .Select(p => double.Parse(p.Trim(), System.Globalization.CultureInfo.InvariantCulture))
                            .ToArray();
                        if (pointValues.Length == 2)
                        {
                            curvePointsList.Add(pointValues);
                        }
                        else
                        {
                            errorMessages.Add($"Неправильний формат точки: '{line}'. Очікується два значення.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        errorMessages.Add($"Помилка аналізу точки '{line}': {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add($"Неочікувана помилка аналізу точки '{line}': {ex.Message}");
                    }
                }

                if (errorMessages.Count > 0)
                {
                    output += "\n\n--- Помилки під час обробки точок ---\n" + string.Join("\n", errorMessages);
                }

            }
            catch (TargetInvocationException tie)
            {
                output = writer.ToString() + "\n\n--- Runtime Error ---\n" + (tie.InnerException?.Message ?? tie.Message);
                runtimeErrorOccurred = true;
            }
            catch (Exception ex)
            {
                output = writer.ToString() + "\n\n--- Runtime Error ---\n" + ex.Message;
                runtimeErrorOccurred = true;
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            bool overallSuccess = !runtimeErrorOccurred && curvePointsList.Count > 0;


            return new CodeResponse
            {
                OutputText = output,
                CurvePoints = curvePointsList,
                Success = overallSuccess
            };
        }
    }
}
