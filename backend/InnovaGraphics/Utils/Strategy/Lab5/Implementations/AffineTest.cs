using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab5.Implementations
{
    public class AffineTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "AffineTest";

        public TestResult TestCompiled(Assembly assembly, IEnumerable<Case> testCases)
        {
            var overallResult = new TestResult
            {
                Success = true,
                ListErrors = new List<string>(),
                CaseResults = new Dictionary<Guid, bool>(),
            };

            // 1. Ініціалізація CaseResults для вхідних кейсів
            if (testCases == null || !testCases.Any())
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Тест не може бути виконаний: Відсутні тестові кейси.");
                return overallResult;
            }
            else
            {
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = true;
                }
            }

            Type? userCodeType = assembly.GetTypes().FirstOrDefault(t => t.IsPublic && t.Name == "Program");
            if (userCodeType == null) { AddErrorAndFailAllCases(overallResult, testCases, "Не знайдено публічний клас Program."); return overallResult; }

            Type? point2DType = assembly.GetTypes().FirstOrDefault(t => t.Name == "Point2D");
            if (point2DType == null) { AddErrorAndFailAllCases(overallResult, testCases, "Не знайдено тип Point2D."); return overallResult; }

            ConstructorInfo? pointCtor = point2DType.GetConstructor(new[] { typeof(double), typeof(double) });
            if (!CheckPoint2DProperties(point2DType, overallResult.ListErrors) ||
                !CheckPoint2DConstructor(point2DType, overallResult.ListErrors) ||
                !CheckPoint2DToString(point2DType, overallResult.ListErrors)) // Додана перевірка ToString
            {
                overallResult.Success = false;
                AddErrorsToAllCases(overallResult, testCases, "Помилки у структурі Point2D.");
                return overallResult;
            }

            MethodInfo? applyMatrixMethod = userCodeType.GetMethod("ApplyMatrix", BindingFlags.Public | BindingFlags.Static, null, new Type[] { point2DType, typeof(double[,]) }, null);
            if (applyMatrixMethod == null || applyMatrixMethod.ReturnType != point2DType) { AddErrorAndFailAllCases(overallResult, testCases, "Не знайдено ApplyMatrix."); return overallResult; }

            MethodInfo? multiplyMatricesMethod = userCodeType.GetMethod("MultiplyMatrices", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(double[,]), typeof(double[,]) }, null);
            if (multiplyMatricesMethod == null || multiplyMatricesMethod.ReturnType != typeof(double[,])) { AddErrorAndFailAllCases(overallResult, testCases, "Не знайдено MultiplyMatrices."); return overallResult; }

            if (!CheckMainMethod(userCodeType, overallResult, testCases))
            {
                return overallResult; 
            }

            FieldInfo? triangleField = userCodeType.GetField("triangle", BindingFlags.Public | BindingFlags.Static);
            if (triangleField == null || !triangleField.FieldType.IsGenericType ||
                triangleField.FieldType.GetGenericTypeDefinition() != typeof(List<>) ||
                triangleField.FieldType.GetGenericArguments()[0] != point2DType)
            { AddErrorAndFailAllCases(overallResult, testCases, "Не знайдено поле 'triangle' типу List<Point2D>."); return overallResult; }


            return overallResult;
        }
        private void AddErrorAndFailAllCases(TestResult result, IEnumerable<Case> testCases, string errorMessage)
        {
            result.Success = false;
            result.ListErrors.Add(errorMessage);
            foreach (var caseItem in testCases)
            {
                result.CaseResults[caseItem.Id] = false;
            }
        }

        private void AddErrorsToAllCases(TestResult result, IEnumerable<Case> testCases, string errorMessage)
        {
            foreach (var caseItem in testCases)
            {
                if (!result.CaseResults.ContainsKey(caseItem.Id) || result.CaseResults[caseItem.Id])
                {
                    result.CaseResults[caseItem.Id] = false;
                }
            }
            result.ListErrors.Add(errorMessage);
            result.Success = false;
        }
        private bool CheckMainMethod(Type userCodeType, TestResult overallResult, IEnumerable<Case> testCases)
        {
            MethodInfo? mainMethod = userCodeType.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (mainMethod == null)
            {
                AddErrorAndFailAllCases(overallResult, testCases, "Не знайдено метод 'Main' у класі Program.");
                return false;
            }

            if (!mainMethod.IsPublic)
            {
                AddErrorAndFailAllCases(overallResult, testCases, "Метод 'Main' повинен бути публічним (public).");
                return false;
            }

            if (!mainMethod.IsStatic)
            {
                AddErrorAndFailAllCases(overallResult, testCases, "Метод 'Main' повинен бути статичним (static).");
                return false;
            }

            if (mainMethod.ReturnType != typeof(void))
            {
                AddErrorAndFailAllCases(overallResult, testCases, "Метод 'Main' повинен повертати 'void'.");
                return false;
            }

            ParameterInfo[] parameters = mainMethod.GetParameters();
            if (parameters.Length > 0)
            {
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string[]))
                {
                    AddErrorAndFailAllCases(overallResult, testCases, "Метод 'Main' не повинен приймати параметрів (або Main(string[] args) не очікується для цього завдання).");
                    return false;
                }
                else
                {
                    AddErrorAndFailAllCases(overallResult, testCases, "Метод 'Main' повинен бути без параметрів.");
                    return false;
                }
            }
            return true;
        }

        private bool CheckPoint2DProperties(Type point2DType, List<string> errors)
        {
            if (!point2DType.IsValueType)
            {
                errors.Add("Тип Point2D повинен бути структурою (struct).");
                return false;
            }
            bool hasX = point2DType.GetProperty("X")?.PropertyType == typeof(double) && point2DType.GetProperty("X")?.CanRead == true && point2DType.GetProperty("X")?.CanWrite == true;
            bool hasY = point2DType.GetProperty("Y")?.PropertyType == typeof(double) && point2DType.GetProperty("Y")?.CanRead == true && point2DType.GetProperty("Y")?.CanWrite == true;
            if (!hasX || !hasY)
            {
                errors.Add("Структура Point2D повинна містити публічні властивості X та Y типу double з getter та setter.");
                return false;
            }
            return true;
        }
        private bool CheckPoint2DConstructor(Type point2DType, List<string> errors)
        {
            var ctor = point2DType.GetConstructor(new[] { typeof(double), typeof(double) });
            if (ctor == null)
            {
                errors.Add("Структура Point2D має містити публічний конструктор з параметрами (double x, double y).");
                return false;
            }
            return true;
        }
        private bool CheckPoint2DToString(Type point2DType, List<string> errors)
        {
            MethodInfo? toStringMethod = point2DType.GetMethod(
                "ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

            if (toStringMethod == null || toStringMethod.ReturnType != typeof(string) ||
                !toStringMethod.DeclaringType.Equals(point2DType)) 
            {
                errors.Add("Структура Point2D має перевизначати публічний метод ToString(), що повертає string.");
                return false;
            }
            return true;
        } 
    }
}