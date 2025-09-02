using InnovaGraphics.Interactions;
using InnovaGraphics.Utils.Strategy.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;
using InnovaGraphics.Structs;
using System.Globalization;
using System.Text;

namespace InnovaGraphics.Utils.Strategy.Lab5.Implementations
{
    public class CodeExecutorLab5 : ICodeExecutor<CodeRequest5, CodeResponse5>
    {
        public async Task<CodeResponse5> ExecuteAsync(CodeRequest5 request)
        {

            var triangleInitCode = GenerateTriangleInitCode(request.InputPoints);
            var expectedPoints = CalculateExpectedPoints(request.InputPoints);
            var syntaxTree = CSharpSyntaxTree.ParseText(request.SourceCode);
            var root = await syntaxTree.GetRootAsync();

            var mainMethod = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text == "Main" &&
                                     m.Modifiers.Any(SyntaxKind.StaticKeyword));

            if (mainMethod == null)
            {
                return new CodeResponse5
                {
                    Success = false,
                    OutputText = "Метод Main не знайдено.",
                    RealPoints = new List<double[]>(),
                    ExpectedPoints = new List<double[]>(),
                    LooksAsExpected = false
                };
            }

            var mainBody = mainMethod.Body;
            // Вставляємо ініціалізацію на початок Main
            var insertStatement = SyntaxFactory.ParseStatement(triangleInitCode + "\n");
            var newStatements = mainBody.Statements.Insert(0, insertStatement);
            var updatedMainBody = mainBody.WithStatements(newStatements);

            var newRoot = root.ReplaceNode(mainBody, updatedMainBody);
            var updatedCode = newRoot.NormalizeWhitespace().ToFullString();

            // Створюємо нове дерево синтаксису з оновленим кодом
            var updatedSyntaxTree = CSharpSyntaxTree.ParseText(updatedCode);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)

            };

            var compilation = CSharpCompilation.Create(
                assemblyName: $"UserCode_{Guid.NewGuid()}",
                syntaxTrees: new[] { updatedSyntaxTree },
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

                return new CodeResponse5
                {
                    Success = false,
                    OutputText = "Помилка компіляції:\n" + string.Join("\n", errors),
                    RealPoints = new List<double[]>(),
                    ExpectedPoints = new List<double[]>(),
                    LooksAsExpected = false
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
                return new CodeResponse5
                {
                    Success = false,
                    OutputText = $"Помилка в завантаженні збірки: {ex.Message}",
                    RealPoints = new List<double[]>(),
                    ExpectedPoints = new List<double[]>(),
                    LooksAsExpected = false
                };
            }

            var entry = assembly.EntryPoint;
            if (entry == null)
            {
                return new CodeResponse5
                {
                    Success = false,
                    OutputText = $"Main не знайдено в скомпільованій збірці.",
                    RealPoints = new List<double[]>(),
                    ExpectedPoints = new List<double[]>(),
                    LooksAsExpected = false
                };
            }

            using var writer = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(writer);

            List<double[]> pointsList = new List<double[]>();
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
                    var pointStr = line.Trim();
                    try
                    {
                        var coords = pointStr.Split(','); 
                        if (coords.Length != 2)
                            throw new FormatException($"Неправильний формат точки: '{pointStr}'. Очікується 'X,Y'.");

                        double x = double.Parse(coords[0].Trim(), CultureInfo.InvariantCulture);
                        double y = double.Parse(coords[1].Trim(), CultureInfo.InvariantCulture);
                        pointsList.Add(new double[] { x, y });
                    }
                    catch (FormatException ex)
                    {
                        errorMessages.Add($"Помилка парсингу точки '{line}': {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add($"Неочікувана помилка при обробці точки '{line}': {ex.Message}");
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

            var Epsilon = 0.1;
            bool pointsMatch = ComparePoints(pointsList, expectedPoints, Epsilon);

            bool overallSuccess = !runtimeErrorOccurred;

            return new CodeResponse5
            {
                Success = overallSuccess,
                OutputText = output,
                ExpectedPoints = expectedPoints,
                RealPoints = pointsList,
                LooksAsExpected = pointsMatch
            };
        }

        private bool ComparePoints(List<double[]> real, List<double[]> expected, double epsilon)
        {
            if (real == null || expected == null)
                return false;

            if (real.Count != expected.Count)
            {
                Console.WriteLine($"Кількість точок не збігається: Очікувано {expected.Count}, Отримано {real.Count}.");
                return false;
            }

            for (int i = 0; i < real.Count; i++)
            {
                if (Math.Abs(real[i][0] - expected[i][0]) > epsilon ||
                    Math.Abs(real[i][1] - expected[i][1]) > epsilon)
                {
                    Console.WriteLine($"Точка {i + 1} не збігається. Очікувано: ({expected[i][0]:F3}, {expected[i][1]:F3}), Отримано: ({real[i][0]:F3}, {real[i][1]:F3}).");
                    return false;
                }
            }
            return true;
        }

        private string GenerateTriangleInitCode(List<double[]> points)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Program.triangle = new List<Point2D>");
            sb.AppendLine("{");
            foreach (var pt in points)
            {
                sb.AppendLine($"    new Point2D({pt[0].ToString(CultureInfo.InvariantCulture)}, {pt[1].ToString(CultureInfo.InvariantCulture)}),");
            }
            sb.AppendLine("};");
            return sb.ToString();
        }

        private List<double[]> CalculateExpectedPoints(List<double[]> input)
        {
            if (input == null || input.Count == 0)
                return new List<double[]>();

            double centerX = 0, centerY = 0;
            foreach (var p in input)
            {
                centerX += p[0]; 
                centerY += p[1]; 
            }
            centerX /= input.Count;
            centerY /= input.Count;

            double angle = 180 * Math.PI / 180.0;

            double[,] toOrigin = {
                { 1, 0, -centerX },
                { 0, 1, -centerY },
                { 0, 0, 1 }
            };

            double[,] rotate = {
                { Math.Cos(angle), -Math.Sin(angle), 0 },
                { Math.Sin(angle),  Math.Cos(angle), 0 },
                { 0, 0, 1 }
            };

            double[,] scale = {
                { 2, 0, 0 },
                { 0, 2, 0 },
                { 0, 0, 1 }
            };

            double[,] backFromOrigin = {
                { 1, 0, centerX },
                { 0, 1, centerY },
                { 0, 0, 1 }
            };

            double[,] translate = {
                { 1, 0, 50 },
                { 0, 1, 50 },
                { 0, 0, 1 }
            };

            var resultMatrix = Multiply(translate,
                                 Multiply(backFromOrigin,
                                     Multiply(scale,
                                         Multiply(rotate, toOrigin))));

            List<double[]> result = new List<double[]>();
            foreach (var p in input)
            {
                var vec = new double[] { p[0], p[1], 1 };
                var transformed = ApplyMatrix(resultMatrix, vec);
                result.Add(new double[] { transformed[0], transformed[1] });
            }

            return result;
        }

        private static double[,] Multiply(double[,] A, double[,] B)
        {
            int rows = A.GetLength(0);
            int cols = B.GetLength(1);
            int common = A.GetLength(1);
            var result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    for (int k = 0; k < common; k++)
                        result[i, j] += A[i, k] * B[k, j];

            return result;
        }
        private static double[] ApplyMatrix(double[,] matrix, double[] vector)
        {
            double[] result = new double[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] = 0;
                for (int j = 0; j < 3; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }
            return result;
        }
    }
}
