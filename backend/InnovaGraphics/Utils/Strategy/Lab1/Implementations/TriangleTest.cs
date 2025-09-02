using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab1.Implementations
{
    public class TriangleTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "TriangleDrawing";

        private bool ComparePointLists(System.Collections.IList actual, System.Collections.IList expected, Type pointType)
        {
            if (actual == null || expected == null || actual.Count != expected.Count)
            {
                return false;
            }

            for (int i = 0; i < actual.Count; i++)
            {
                var actualPoint = actual[i];
                var expectedPoint = expected[i];

                if (actualPoint == null || expectedPoint == null || actualPoint.GetType() != pointType || expectedPoint.GetType() != pointType)
                {
                    return false;
                }

                var actualXProperty = pointType.GetProperty("X");
                var actualYProperty = pointType.GetProperty("Y");
                var expectedXProperty = pointType.GetProperty("X");
                var expectedYProperty = pointType.GetProperty("Y");

                if (actualXProperty == null || actualYProperty == null || expectedXProperty == null || expectedYProperty == null)
                {
                    return false;
                }

                if (Math.Abs((double)actualXProperty.GetValue(actualPoint) - (double)expectedXProperty.GetValue(expectedPoint)) > 1e-9 ||
                    Math.Abs((double)actualYProperty.GetValue(actualPoint) - (double)expectedYProperty.GetValue(expectedPoint)) > 1e-9)
                {
                    return false;
                }
            }

            return true;
        }

        public TestResult TestCompiled(Assembly assembly, IEnumerable<Case> testCases)
        {
            var overallResult = new TestResult { Success = true, CaseResults = new Dictionary<Guid, bool>(), ListErrors = new List<string>() };

            if (testCases == null || !testCases.Any())
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Тест не може бути виконаний: відсутні тестові кейси.");
                return overallResult;
            }

            Type? userCodeType = assembly.GetTypes().FirstOrDefault(t => t.IsPublic && t.Name == "Program");
            if (userCodeType == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося знайти публічний клас \"Program\" у скомпільованій збірці.");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            Type? point2DType = assembly.GetType("UserCode.Point2D");
            if (point2DType == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося знайти тип \"UserCode.Point2D\" у скомпільованій збірці.");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            MethodInfo? getPointsMethod = userCodeType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m => m.Name == "GetPointsToDraw" &&
                                     m.GetParameters().Length == 3 &&
                                     m.GetParameters()[0].ParameterType == point2DType &&
                                     m.GetParameters()[1].ParameterType == point2DType &&
                                     m.GetParameters()[2].ParameterType == typeof(double));

            if (getPointsMethod == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося знайти публічний статичний метод \"GetPointsToDraw(Point2D, Point2D, double)\" у класі \"Program\".");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            var specificTestCases = new List<(object p1, object p2, double h, List<object> expectedTriangle, List<object> expectedBoundingBox)>
            {
                (Activator.CreateInstance(point2DType, new object[] { 0.0, 0.0 }), Activator.CreateInstance(point2DType, new object[] { 10.0, 0.0 }), 5.0,
                    new List<object> { Activator.CreateInstance(point2DType, new object[] { 0.0, 0.0 }), Activator.CreateInstance(point2DType, new object[] { 10.0, 0.0 }), Activator.CreateInstance(point2DType, new object[] { 5.0, 5.0 }) },
                    new List<object> { Activator.CreateInstance(point2DType, new object[] { 0.0, 5.0 }), Activator.CreateInstance(point2DType, new object[] { 10.0, 5.0 }), Activator.CreateInstance(point2DType, new object[] { 10.0, 0.0 }), Activator.CreateInstance(point2DType, new object[] { 0.0, 0.0 }) }),
                (Activator.CreateInstance(point2DType, new object[] { 20.0, 30.0 }), Activator.CreateInstance(point2DType, new object[] { 120.0, 30.0 }), 80.0,
                    new List<object> { Activator.CreateInstance(point2DType, new object[] { 20.0, 30.0 }), Activator.CreateInstance(point2DType, new object[] { 120.0, 30.0 }), Activator.CreateInstance(point2DType, new object[] { 70.0, 110.0 }) },
                    new List<object> { Activator.CreateInstance(point2DType, new object[] { 20.0, 110.0 }), Activator.CreateInstance(point2DType, new object[] { 120.0, 110.0 }), Activator.CreateInstance(point2DType, new object[] { 120.0, 30.0 }), Activator.CreateInstance(point2DType, new object[] { 20.0, 30.0 }) }),
            };

            int testCaseIndex = 0;
            foreach (var testCaseMarker in testCases)
            {
                if (testCaseIndex >= specificTestCases.Count)
                {
                    overallResult.CaseResults[testCaseMarker.Id] = false;
                    overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Недостатньо визначених тестових даних для цього маркера.");
                    overallResult.Success = false;
                    break;
                }

                var (p1Base, p2Base, height, expectedTriangle, expectedBoundingBox) = specificTestCases[testCaseIndex];

                try
                {
                    Console.WriteLine($"Виклик GetPointsToDraw з параметрами: p1Base={p1Base}, p2Base={p2Base}, height={height}");

                    var rawResult = getPointsMethod.Invoke(null, new object[] { p1Base, p2Base, height });

                    Console.WriteLine($"Результат виклику GetPointsToDraw: {rawResult}");

                    if (rawResult != null)
                    {
                        var fields = rawResult.GetType().GetFields();
                        object? actualTriangle = null;
                        object? actualBoundingBox = null;

                        foreach (var field in fields)
                        {
                            if (field.Name.Contains("Item1"))
                            {
                                actualTriangle = field.GetValue(rawResult) as System.Collections.IList;
                            }
                            else if (field.Name.Contains("Item2"))
                            {
                                actualBoundingBox = field.GetValue(rawResult) as System.Collections.IList;
                            }
                        }

                        if (actualTriangle != null && actualBoundingBox != null)
                        {
                            bool triangleMatch = ComparePointLists((System.Collections.IList)actualTriangle, expectedTriangle, point2DType);
                            bool boundingBoxMatch = ComparePointLists((System.Collections.IList)actualBoundingBox, expectedBoundingBox, point2DType);

                            if (triangleMatch && boundingBoxMatch)
                            {
                                overallResult.CaseResults[testCaseMarker.Id] = true;
                            }
                            else
                            {
                                overallResult.CaseResults[testCaseMarker.Id] = false;
                                if (!triangleMatch)
                                {
                                    overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Отримані точки трикутника не відповідають очікуваним.");
                                }
                                if (!boundingBoxMatch)
                                {
                                    overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Отримані точки охоплюючого прямокутника не відповідають очікуваним.");
                                }
                                overallResult.Success = false;
                            }
                        }
                        else
                        {
                            overallResult.CaseResults[testCaseMarker.Id] = false;
                            overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Не вдалося отримати списки точок з результату.");
                            overallResult.Success = false;
                        }
                    }
                    else
                    {
                        overallResult.CaseResults[testCaseMarker.Id] = false;
                        overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Результат виклику методу є null.");
                        overallResult.Success = false;
                    }
                }
                catch (TargetInvocationException tie)
                {
                    overallResult.CaseResults[testCaseMarker.Id] = false;
                    if (tie.InnerException != null)
                    {
                        overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Виняток під час виконання коду користувача (TargetInvocationException): {tie.InnerException.GetType().FullName}: {tie.InnerException.Message}\n{tie.InnerException.StackTrace}");
                    }
                    else
                    {
                        overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Виняток під час виконання коду користувача (TargetInvocationException без InnerException): {tie.Message}\n{tie.StackTrace}");
                    }
                    overallResult.Success = false;
                }
                catch (Exception ex)
                {
                    overallResult.CaseResults[testCaseMarker.Id] = false;
                    overallResult.ListErrors.Add($"Тестовий кейс {testCaseMarker.Id}: Неочікувана помилка під час виклику методу користувача: {ex.Message}");
                    overallResult.Success = false;
                }

                testCaseIndex++;
            }

            return overallResult;
        }
    }
}