using InnovaGraphics.Interactions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using InnovaGraphics.Utils.Strategy.Interfaces;

namespace InnovaGraphics.Utils.Strategy.Lab2.Implementations
{
    public class CodeExecutorLab2 : ICodeExecutor<CodeRequest, CodeResponse>
    {
        public async Task<CodeResponse> ExecuteAsync(CodeRequest request)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(request.SourceCode);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location),
            };

            var compilation = CSharpCompilation.Create(
                assemblyName: $"UserCode_{Guid.NewGuid()}", 
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                    .Where(diag => diag.Severity == DiagnosticSeverity.Error)
                    .Select(diag => diag.ToString())
                    .ToArray();

                return new CodeResponse
                {
                    OutputText = "Compilation Errors:\n" + string.Join("\n", errors),
                    CurvePoints = null, 
                    Success = false 
                };
            }

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(ms.ToArray());
            }
            catch (Exception ex)
            {
                return new CodeResponse
                {
                    OutputText = $"Помилка в завантаженні збірки: {ex.Message}",
                    CurvePoints = null,
                    Success = false 
                };
            }

            var entry = assembly.EntryPoint;
            if (entry == null)
            {
                return new CodeResponse
                {
                    OutputText = "Main не знайдено в скомпільованій збірці.",
                    CurvePoints = null,
                    Success = false 
                };
            }

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
                    if (line.TrimStart().StartsWith("[") && line.TrimEnd().EndsWith("]"))
                    {
                        var pointStr = line.Trim().Trim('[', ']');
                        try
                        {
                            var pointValues = pointStr.Split(',')
                                .Select(p => double.Parse(p.Trim(), System.Globalization.CultureInfo.InvariantCulture)) 
                                .ToArray();
                            curvePointsList.Add(pointValues);
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
