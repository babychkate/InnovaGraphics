using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab3.Implementations
{
    public class FractalTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "FractalTest";

        public TestResult TestCompiled(Assembly assembly, IEnumerable<Case> testCases)
        {
            var overallResult = new TestResult
            {
                Success = true,
                ListErrors = new List<string>(),
                CaseResults = new Dictionary<Guid, bool>()
            };

            if (testCases == null || !testCases.Any())
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Тест не може бути виконаний: Відсутні тестові кейси.");
                return overallResult;
            }

            // 1. Перевірка наявності класу Program
            Type? userCodeType = assembly.GetTypes()
                .FirstOrDefault(t => t.IsPublic && t.Name == "Program");

            if (userCodeType == null)
            {
                AddErrorAndFailAllCases(overallResult, testCases,
                    "Не вдалося знайти загальнодоступний клас \"Program\" у скомпільованій збірці.");
                return overallResult;
            }

            // 2. Отримуємо тип Point2D зі збірки користувача
            Type? point2DType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == "Point2D");
            if (point2DType == null)
            {
                AddErrorAndFailAllCases(overallResult, testCases,
                    "Не вдалося знайти тип \"Point2D\" у скомпільованій збірці.");
                return overallResult;
            }

            // Перевірка конструктора Point2D
            bool isConstructorCorrect = CheckPoint2DConstructor(point2DType, overallResult.ListErrors);
            if (!isConstructorCorrect)
            {
                overallResult.Success = false;
                foreach (var caseItem in testCases)
                    overallResult.CaseResults[caseItem.Id] = false;

                return overallResult;
            }

            // Створюємо тип List<Point2D> динамічно
            var listType = typeof(List<>).MakeGenericType(point2DType);

            // 3. Перевірка методу DrawTree
            MethodInfo? drawTreeMethod = userCodeType.GetMethod(
                "DrawTree",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { listType, point2DType, typeof(double), typeof(double), typeof(int) },
                null);

            if (drawTreeMethod == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не знайдено публічний статичний метод DrawTree з потрібною сигнатурою.");
                return overallResult;
            }

            if (drawTreeMethod.ReturnType != typeof(void))
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Метод DrawTree має повертати void.");
                return overallResult;
            }

            // 4. Перевірка: при depth=0 метод не додає точок
            var pointsInstance = Activator.CreateInstance(listType) as System.Collections.IList;
            var pointCtor = point2DType.GetConstructor(new[] { typeof(double), typeof(double) });
            if (pointsInstance == null || pointCtor == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося створити екземпляр List<Point2D> або конструктор Point2D.");
                return overallResult;
            }
            var startPoint = pointCtor?.Invoke(new object[] { 0.0, 0.0 });

            drawTreeMethod.Invoke(null, new object[] { pointsInstance, startPoint, 100.0, 90.0, 0 });
            if (pointsInstance.Count != 0)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("При depth=0 метод повинен не додавати жодної точки, але список точок не порожній.");
            }

            // 5. Перевірка кількості точок при depth=1
            pointsInstance = Activator.CreateInstance(listType) as System.Collections.IList;
            drawTreeMethod.Invoke(null, new object[] { pointsInstance, startPoint, 100.0, 90.0, 1 });
            if (pointsInstance.Count != 2)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"При depth=1 очікується 2 точки, але отримано {pointsInstance.Count}.");
            }

            // 6. Перевірка координат початкової та кінцевої точок при depth=1
            pointsInstance = Activator.CreateInstance(listType) as System.Collections.IList;
            double length = 100.0;
            double angle = 0.0; // горизонтально вправо
            int depth = 1;

            drawTreeMethod.Invoke(null, new object[] { pointsInstance, startPoint, length, angle, depth });

            if (pointsInstance.Count == 2)
            {
                var startPointInstance = pointsInstance[0];
                var endPointInstance = pointsInstance[1];

                if (!IsPointsEqual(startPointInstance, startPoint, point2DType))
                {
                    overallResult.Success = false;
                    overallResult.ListErrors.Add("Початкова точка не співпадає з очікуваною.");
                }

                // Отримуємо X, Y початкової точки
                double startX = (double)point2DType.GetProperty("X")!.GetValue(startPoint)!;
                double startY = (double)point2DType.GetProperty("Y")!.GetValue(startPoint)!;

                // Обчислюємо очікувану кінцеву точку
                var expectedEnd = pointCtor.Invoke(new object[] { startX + length, startY });

                if (!IsPointsEqual(endPointInstance, expectedEnd, point2DType))
                {
                    double expectedX = (double)point2DType.GetProperty("X")!.GetValue(expectedEnd)!;
                    double expectedY = (double)point2DType.GetProperty("Y")!.GetValue(expectedEnd)!;
                    double actualX = (double)point2DType.GetProperty("X")!.GetValue(endPointInstance)!;
                    double actualY = (double)point2DType.GetProperty("Y")!.GetValue(endPointInstance)!;

                    overallResult.Success = false;
                    overallResult.ListErrors.Add(
                        $"Кінцева точка не співпадає з очікуваною. Очікувалось X={expectedX}, Y={expectedY}, " +
                        $"а отримано X={actualX}, Y={actualY}");
                }
            }
            else
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Очікується 2 точки для depth=1, але їх інша кількість.");
            }

            // 9. Додаткові перевірки через CheckDrawTree
            var collectedFixedErrors = new List<string>();
            bool allRequiredChecksPassed = CheckDrawTree(userCodeType, point2DType, collectedFixedErrors);

            overallResult.ListErrors.AddRange(collectedFixedErrors);

            foreach (var caseItem in testCases)
            {
                overallResult.CaseResults[caseItem.Id] = allRequiredChecksPassed;
            }

            if (!allRequiredChecksPassed)
            {
                overallResult.Success = false;
            }

            return overallResult;
        }

        private bool CheckDrawTree(Type userCodeType, Type point2DType, List<string> errors)
        {
            bool passed = true;
            var listType = typeof(List<>).MakeGenericType(point2DType);

            MethodInfo? drawTreeMethod = userCodeType.GetMethod(
                "DrawTree",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { listType, point2DType, typeof(double), typeof(double), typeof(int) },
                null);

            if (drawTreeMethod == null)
            {
                passed = false;
                errors.Add("Не вдалося знайти статичний метод DrawTree(List<Point2D>, Point2D, double, double, int).");
                return passed;
            }

            try
            {
                var listInstance = Activator.CreateInstance(listType) as System.Collections.IList;
                var pointCtor = point2DType.GetConstructor(new[] { typeof(double), typeof(double) });
                var startPoint = pointCtor?.Invoke(new object[] { 0.0, 0.0 });

                drawTreeMethod.Invoke(null, new object[] { listInstance, startPoint, 10.0, 90.0, 2 });

                int count = listInstance?.Count ?? 0;
                if (count != 6)
                {
                    passed = false;
                    errors.Add($"DrawTree: Очікується 6 точок при depth=2, але отримано {count}.");
                }
            }
            catch (Exception ex)
            {
                passed = false;
                errors.Add($"DrawTree: Виняток під час виклику: {ex.Message}");
            }

            return passed;
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

        // Оновлена версія IsPointsEqual для роботи з об'єктами, які динамічно визначаються через рефлексію
        private bool IsPointsEqual(object p1, object p2, Type point2DType, double tolerance = 1e-9)
        {
            var propX = point2DType.GetProperty("X");
            var propY = point2DType.GetProperty("Y");
            if (propX == null || propY == null)
                return false;

            double x1 = Convert.ToDouble(propX.GetValue(p1));
            double y1 = Convert.ToDouble(propY.GetValue(p2));
            double x2 = Convert.ToDouble(propX.GetValue(p2));
            double y2 = Convert.ToDouble(propY.GetValue(p2));

            return Math.Abs(x1 - x2) < tolerance && Math.Abs(y1 - y2) < tolerance;
        }

        // Перевірка конструктора Point2D(double x, double y)
        private bool CheckPoint2DConstructor(Type point2DType, List<string> errors)
        {
            var ctor = point2DType.GetConstructor(new[] { typeof(double), typeof(double) });
            if (ctor == null)
            {
                errors.Add("Тип Point2D має містити конструктор з параметрами (double x, double y).");
                return false;
            }
            return true;
        }
    }
}