using InnovaGraphics.Interactions;
using InnovaGraphics.Utils.Strategy.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Loader;

namespace InnovaGraphics.Utils.Strategy.Lab4.Implementations
{
    public class CodeExecutorLab4 : ICodeExecutor<CodeRequest4, CodeResponse4>
    {
        public async Task<CodeResponse4> ExecuteAsync(CodeRequest4 request)
        {
            if (string.IsNullOrEmpty(request.SourceImageBase64))
            {
                return new CodeResponse4
                {
                    ErrorMessage = "Відсутнє вхідне зображення у запиті.",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }

            Bitmap inputBitmap;
            try
            {
                byte[] imageBytes = Convert.FromBase64String(request.SourceImageBase64);
                using var ms = new MemoryStream(imageBytes);
                inputBitmap = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                return new CodeResponse4
                {
                    ErrorMessage = $"Помилка декодування вхідного зображення: {ex.Message}",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }

            var syntaxTree = CSharpSyntaxTree.ParseText(request.SourceCode);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Bitmap).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(System.Drawing.Color).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Drawing.Common").Location), 
                MetadataReference.CreateFromFile(Assembly.Load("System.Drawing.Primitives").Location) 
            };

            var privateWindowsCoreAssembly = Assembly.Load("System.Private.Windows.Core");
            references.Add(MetadataReference.CreateFromFile(privateWindowsCoreAssembly.Location));


            var compilation = CSharpCompilation.Create(
                assemblyName: $"UserCode_Lab4_{Guid.NewGuid()}",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var compileMs = new MemoryStream();
            var emitResult = compilation.Emit(compileMs);

            if (!emitResult.Success)
            {
                inputBitmap.Dispose();
                var errors = emitResult.Diagnostics
                    .Where(diag => diag.Severity == DiagnosticSeverity.Error)
                    .Select(diag => diag.ToString())
                    .ToArray();

                return new CodeResponse4
                {
                    ErrorMessage = "Помилки компіляції коду користувача:\n" + string.Join("\n", errors),
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }

            compileMs.Seek(0, SeekOrigin.Begin);
            Assembly assembly;
            try
            {
                assembly = AssemblyLoadContext.Default.LoadFromStream(compileMs);
            }
            catch (Exception ex)
            {
                inputBitmap.Dispose();
                return new CodeResponse4
                {
                    ErrorMessage = $"Помилка завантаження скомпільованої збірки: {ex.Message}",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }

            Type programType = assembly.GetType("Program");
            if (programType == null)
            {
                inputBitmap.Dispose();
                return new CodeResponse4
                {
                    ErrorMessage = "Не знайдено клас Program у коді користувача.",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }

            MethodInfo processImageMethod = programType.GetMethod(
                "Convert",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(Bitmap) },
                null
            );

            if (processImageMethod == null)
            {
                inputBitmap.Dispose();
                return new CodeResponse4
                {
                    ErrorMessage = "Не знайдено публічний статичний метод 'Convert",
                    ConvertedImageBase64 = null
                };
            }

            Bitmap outputBitmap;
            try
            {
                object result = processImageMethod.Invoke(null, new object[] { inputBitmap });

                if (result is not Bitmap)
                {
                    inputBitmap.Dispose();
                    return new CodeResponse4
                    {
                        ErrorMessage = $"Метод 'Convert' повернув неочікуваний тип результату ({result?.GetType().Name ?? "null"}). Очікувався Bitmap.",
                        Success = false,
                        ConvertedImageBase64 = null
                    };
                }
                outputBitmap = (Bitmap)result;
            }
            catch (TargetInvocationException tie)
            {
                inputBitmap.Dispose();
                var innerEx = tie.InnerException;
                return new CodeResponse4
                {
                    ErrorMessage = $"Помилка виконання коду користувача: {innerEx?.Message ?? tie.Message}",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }
            catch (Exception ex)
            {
                inputBitmap.Dispose();
                return new CodeResponse4
                {
                    ErrorMessage = $"Помилка виклику методу обробки зображення: {ex.Message}",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }
            finally
            {
                inputBitmap.Dispose();
            }

            string outputImageBase64;
            try
            {
                using var outputMs = new MemoryStream();
                outputBitmap.Save(outputMs, ImageFormat.Png);
                byte[] outputBytes = outputMs.ToArray();
                outputImageBase64 = Convert.ToBase64String(outputBytes);
            }
            catch (Exception ex)
            {
               outputBitmap?.Dispose();
                return new CodeResponse4
                {
                    ErrorMessage = $"Помилка конвертації вихідного зображення в Base64: {ex.Message}",
                    Success = false,
                    ConvertedImageBase64 = null
                };
            }
            finally
            {
                outputBitmap?.Dispose();
            }

            return new CodeResponse4
            {
                Success = true,
                ErrorMessage = null,
                ConvertedImageBase64 = outputImageBase64
            };
        }
    }
}