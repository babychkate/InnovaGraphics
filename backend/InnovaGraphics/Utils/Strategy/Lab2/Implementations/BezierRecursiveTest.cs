using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Structs;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab2.Implementations
{
    public class BezierRecursiveTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "RecursiveBezierTest";

        public TestResult TestCompiled(Assembly assembly, IEnumerable<Case> testCases)
        {
            var overallResult = new TestResult(); 
            overallResult.Success = true; 
            overallResult.CaseResults = new Dictionary<Guid, bool>(); 

            if (testCases == null || !testCases.Any())
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Тест не може бути виконаний: Відсутні тестові кейси.");
                return overallResult;
            }

            Type? userCodeType = assembly.GetTypes()
                 .FirstOrDefault(t => t.IsPublic && t.Name == "Program");

            if (userCodeType == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося знайти загальнодоступний клас \"Program\" у скомпільованій збірці.");
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
                return overallResult;
            }

            Type? userPoint2DType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == nameof(Point2D) && (t.IsValueType || t.IsClass));
           
            if (userPoint2DType == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Не вдалося знайти public struct/class '{nameof(Point2D)}' у скомпільованій збірці.");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            bool hasXYMembers = userPoint2DType.GetMembers(BindingFlags.Public | BindingFlags.Instance).Any(m =>
                (m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field) &&
                (m.Name == "X" || m.Name == "Y") &&
                (m is PropertyInfo pi && (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(float)) ||
                 m is FieldInfo fi && (fi.FieldType == typeof(double) || fi.FieldType == typeof(float)))
                );

            if (!hasXYMembers)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Знайдено '{nameof(Point2D)}', але він не має публічних відкритих елементів X і Y (поля або властивості типу double або float).");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            Type userListOfPoint2DType = typeof(List<>).MakeGenericType(userPoint2DType);

            MethodInfo? bezierMethod = userCodeType.GetMethod("CalculateRecursiveBezier",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { userListOfPoint2DType, typeof(double) },
                null);

            if (bezierMethod == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Не вдалося знайти загальнодоступний статичний метод \"CalculateRecursiveBezier(List<{{nameof(Point2D)}}>, double)\" у класі \"Program\". Переконайтеся, що список чітко набраний вашим Point2D.");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            if (bezierMethod.ReturnType != userPoint2DType)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Метод 'CalculateRecursiveBezier' не повертає тип '{nameof(Point2D)}'. Фактичний тип повернення: {bezierMethod.ReturnType.Name}");
                foreach (var caseItem in testCases) { overallResult.CaseResults[caseItem.Id] = false; }
                return overallResult;
            }

            foreach (var currentCase in testCases)
            {
                var caseRunResult = new TestResult { Success = true }; 

                Guid currentCaseId = currentCase.Id; 

                try
                {
                    var controlPoints = new List<Point2D>
                    {
                        new Point2D(0, 0),
                        new Point2D(1, 1),
                        new Point2D(2, 0)
                    };
                    double t = 0.5;
                    Point2D expected = new Point2D(1.0, 0.5);
                    double tolerance = 1e-9; 


                    object? userControlPointsList = Activator.CreateInstance(userListOfPoint2DType);
                    MethodInfo? addMethod = userListOfPoint2DType.GetMethod("Add", new Type[] { userPoint2DType });

                    if (userControlPointsList == null || addMethod == null)
                    {
                        caseRunResult.Success = false;
                        caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Внутрішня помилка тестування: не вдалося створити список точок по яких потрібно намалювати криву List<{nameof(Point2D)}> або знайти Add метод.");
                    }
                    else
                    {
                        foreach (var cp in controlPoints)
                        {
                            object? userPoint;
                            try
                            {
                                ConstructorInfo? constructor = userPoint2DType.GetConstructor(new[] { typeof(double), typeof(double) });
                                if (constructor != null) { userPoint = constructor.Invoke(new object[] { cp.X, cp.Y }); }
                                else
                                {
                                    userPoint = Activator.CreateInstance(userPoint2DType);
                                    if (userPoint != null)
                                    {
                                        var xMember = userPoint2DType.GetProperty("X") ?? (MemberInfo)userPoint2DType.GetField("X");
                                        var yMember = userPoint2DType.GetProperty("Y") ?? (MemberInfo)userPoint2DType.GetField("Y");
                                        if (xMember is PropertyInfo xProp) xProp.SetValue(userPoint, cp.X); else if (xMember is FieldInfo xField) xField.SetValue(userPoint, cp.X); else throw new InvalidOperationException("Не вдалося знайти властивість/поле X.");
                                        if (yMember is PropertyInfo yProp) yProp.SetValue(userPoint, cp.Y); else if (yMember is FieldInfo yField) yField.SetValue(userPoint, cp.Y); else throw new InvalidOperationException("Не вдалося знайти властивість/поле Y.");
                                    }
                                }
                                if (userPoint == null) throw new InvalidOperationException("Activator.CreateInstance повернув null.");
                            }
                            catch (Exception ex)
                            {
                                caseRunResult.Success = false;
                                caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Внутрішня помилка тестування: не вдалося створити та заповнити екземпляр типу Point2D користувача: {ex.Message}");
                                break; 
                            }
                            addMethod.Invoke(userControlPointsList, new object[] { userPoint });
                        }
                    }


                    if (caseRunResult.Success)
                    {
                        object? actualResult = bezierMethod.Invoke(null, new object[] { userControlPointsList, t });

                        if (actualResult == null)
                        {
                            caseRunResult.Success = false;
                            caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Метод CalculateRecursiveBezier повернув значення null.");
                        }
                        else
                        {
                            double actualX, actualY;
                            try
                            {
                                actualX = (double)(userPoint2DType.GetProperty("X")?.GetValue(actualResult) ?? userPoint2DType.GetField("X")?.GetValue(actualResult) ?? throw new InvalidOperationException("Could not get X from user Point2D"));
                                actualY = (double)(userPoint2DType.GetProperty("Y")?.GetValue(actualResult) ?? userPoint2DType.GetField("Y")?.GetValue(actualResult) ?? throw new InvalidOperationException("Could not get Y from user Point2D"));
                            }
                            catch (Exception ex)
                            {
                                caseRunResult.Success = false;
                                caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Помилка доступу до елемента X або Y у повернутому об’єкті Point2D: {ex.Message}");
                                return overallResult; 
                            }

                            if (Math.Abs(actualX - expected.X) > tolerance || Math.Abs(actualY - expected.Y) > tolerance)
                            {
                                caseRunResult.Success = false;
                                caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Помилка! Очікувані точки: ({expected.X}, {expected.Y}), але отримано ({actualX}, {actualY}). Різниця ({Math.Abs(actualX - expected.X):F6}, {Math.Abs(actualY - expected.Y):F6}).");
                            }
                        }
                    }
                }
                catch (TargetInvocationException tie)
                {
                    caseRunResult.Success = false;
                    caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Виняток під час виконання коду користувача: {tie.InnerException?.Message ?? tie.Message}");
                }
                catch (Exception ex)
                {
                    caseRunResult.Success = false;
                    caseRunResult.ListErrors.Add($"Кейс {currentCaseId}: Тест не вдався через неочікуваний виняток: {ex.Message}");
                }

                overallResult.CaseResults[currentCaseId] = caseRunResult.Success;
                overallResult.ListErrors.AddRange(caseRunResult.ListErrors);

                if (!caseRunResult.Success)
                {
                    overallResult.Success = false;
                }

            } 
            return overallResult;

        } 
    }
}
