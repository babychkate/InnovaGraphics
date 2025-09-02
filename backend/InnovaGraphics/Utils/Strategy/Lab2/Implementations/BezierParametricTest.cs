using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab2.Implementations
{
    public class BezierParametricTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "BezierParametricTest";

        public TestResult TestCompiled(Assembly assembly, IEnumerable<Case> testCases)
        {
            var overallResult = new TestResult();
            overallResult.Success = true; 
            overallResult.CaseResults = new Dictionary<Guid, bool>();
            overallResult.ListErrors = new List<string>();

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

            List<string> collectedFixedErrors = new List<string>(); 
            double tolerance = 1e-9; 

            bool binomialCheckPassed = CheckBinomialCoefficient(userCodeType, tolerance, collectedFixedErrors);

            bool bernsteinCheckPassed = CheckBernsteinPolynomial(userCodeType, tolerance, collectedFixedErrors);

            bool allRequiredChecksPassed = binomialCheckPassed && bernsteinCheckPassed;

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

        private bool CheckBinomialCoefficient(Type userCodeType, double tolerance, List<string> errors)
        {
            bool passed = true; 

            MethodInfo? binomialMethod = userCodeType.GetMethod("BinomialCoefficient",
                BindingFlags.NonPublic | BindingFlags.Static, 
                null,
                new Type[] { typeof(int), typeof(int) },
                null);

            if (binomialMethod == null)
            {
                passed = false;
                errors.Add($"Фіксована перевірка: Не вдалося знайти приватний статичний метод \"BinomialCoefficient(int, int)\".");
            }
            else if (binomialMethod.ReturnType != typeof(double))
            {
                passed = false;
                errors.Add($"Фіксована перевірка: Метод 'BinomialCoefficient' не повертає 'double'. Фактичний тип повернення: {binomialMethod.ReturnType.Name}");
            }
            else
            {
                var binomialTestCasesData = new[]
                {
                    new { n = 6, k = 2, expected = 15.0 },
                };

                foreach (var testCaseData in binomialTestCasesData)
                {
                    try
                    {
                        object? methodResult = binomialMethod.Invoke(null, new object[] { testCaseData.n, testCaseData.k });

                        if (methodResult is double coefficient)
                        {
                            if (Math.Abs(coefficient - testCaseData.expected) > tolerance)
                            {
                                passed = false;
                                errors.Add($"Фіксована перевірка: BinomialCoefficient({testCaseData.n}, {testCaseData.k}) повертає {coefficient}, очікувано {testCaseData.expected}. Різниця: {Math.Abs(coefficient - testCaseData.expected):F6}");
                            }
                        }
                        else
                        {
                            passed = false; 
                            errors.Add($"Фіксована перевірка: Неочікуваний тип повернення від методу BinomialCoefficient для n={testCaseData.n}, k={testCaseData.k}.");
                        }
                    }
                    catch (TargetInvocationException tie) 
                    {
                        passed = false;
                        errors.Add($"Фіксована перевірка: Виняток під час виклику BinomialCoefficient ({testCaseData.n}, {testCaseData.k}): {tie.InnerException?.Message ?? tie.Message}");
                    }
                    catch (Exception ex) 
                    {
                        passed = false;
                        errors.Add($"Фіксована перевірка: Неочікувана помилка під час виклику BinomialCoefficient ({testCaseData.n}, {testCaseData.k}): {ex.Message}");
                    }
                }
            }

            return passed; 
        }

       private bool CheckBernsteinPolynomial(Type userCodeType, double tolerance, List<string> errors)
        {
            bool passed = true; 

            MethodInfo? bernsteinMethod = userCodeType.GetMethod("BernsteinPolynomial",
                BindingFlags.Public | BindingFlags.Static, 
                null,
                new Type[] { typeof(int), typeof(int), typeof(double) }, 
                null);

            if (bernsteinMethod == null)
            {
                passed = false;
                errors.Add($"Фіксована перевірка: Не вдалося знайти публічний статичний метод 'BernsteinPolynomial(int, int, double)'.");
            }
            else if (bernsteinMethod.ReturnType != typeof(double))
            {
                passed = false;
                errors.Add($"Фіксована перевірка: Метод 'BernsteinPolynomial' не повертає тип 'double'. Фактичний тип повернення: {bernsteinMethod.ReturnType.Name}");
            }
            else
            {
                double tSumCheck = 0.5; 
                double sum = 0.0;
                int nDegreeSumCheck = 3; 
                bool bernsteinSumCalculationSuccessful = true; 

                for (int i = 0; i <= nDegreeSumCheck; i++)
                {
                    try
                    {
                        object? methodResult = bernsteinMethod.Invoke(null, new object[] { i, nDegreeSumCheck, tSumCheck });

                        if (methodResult is double bValue)
                        {
                            sum += bValue;
                        }
                        else
                        {
                            bernsteinSumCalculationSuccessful = false;
                            errors.Add($"Фіксована перевірка: Неочікуваний тип повернення від BernsteinPolynomial для i={i}, n={nDegreeSumCheck}, t={tSumCheck} під час обчислення суми.");
                            break; 
                        }
                    }
                    catch (TargetInvocationException tie)
                    {
                        bernsteinSumCalculationSuccessful = false;
                        errors.Add($"Фіксована перевірка: Виняток під час виклику BernsteinPolynomial ({i}, {nDegreeSumCheck}, {tSumCheck}) під час обчислення суми: {tie.InnerException?.Message ?? tie.Message}");
                        break; 
                    }
                    catch (Exception ex)
                    {
                        bernsteinSumCalculationSuccessful = false;
                        errors.Add($"Фіксована перевірка: Неочікувана помилка під час виклику BernsteinPolynomial ({i}, {nDegreeSumCheck}, {tSumCheck}) під час обчислення суми: {ex.Message}");
                        break; 
                    }
                }

                if (!bernsteinSumCalculationSuccessful)
                {
                    passed = false;
                }
                else if (Math.Abs(sum - 1.0) > tolerance)
                {
                    passed = false; 
                    errors.Add($"Фіксована перевірка: Помилка суми поліномів Бернштейна. Очікувана сума близька до 1, але отримана {sum}. Різниця є {Math.Abs(sum - 1.0):F6}.");
                }
            }

            return passed; 
        }
    }
}