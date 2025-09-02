using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab2.Implementations
{
    public class BezierMatrixTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "BezierMatrixTest";

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

            TestResult getCoefficientMatrixResult = TestGetCoefficientMatrix(userCodeType);
            TestResult multiplyVectorMatrixResult = TestMultiplyVectorMatrix(userCodeType);

            if (getCoefficientMatrixResult.ListErrors != null)
            {
                overallResult.ListErrors.AddRange(getCoefficientMatrixResult.ListErrors);
            }
            if (multiplyVectorMatrixResult.ListErrors != null)
            {
                overallResult.ListErrors.AddRange(multiplyVectorMatrixResult.ListErrors);
            }


            bool fixedChecksOverallSuccess = getCoefficientMatrixResult.Success && multiplyVectorMatrixResult.Success;

            foreach (var caseItem in testCases)
            {
                overallResult.CaseResults[caseItem.Id] = fixedChecksOverallSuccess;

                if (!fixedChecksOverallSuccess)
                {
                    overallResult.Success = false;
                }
            }


            return overallResult; 
        }

        private TestResult TestGetCoefficientMatrix(Type userCodeType)
        {
            var result = new TestResult { Success = true, ListErrors = new List<string>() }; 

            MethodInfo? getCoefficientMatrixMethod = userCodeType.GetMethod("GetCoefficientMatrix",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(int) },
                null);

            if (getCoefficientMatrixMethod == null)
            {
                result.Success = false;
                result.ListErrors.Add("Не вдалося знайти публічний статичний метод 'GetCoefficientMatrix(int)' у класі  'Program'.");
                return result;
            }

            var orderTestCases = new[]
            {
                new { order = 3, Expected = new double[,] { { -1, 3, -3, 1 }, { 3, -6, 3, 0 }, { -3, 3, 0, 0 }, { 1, 0, 0, 0 } } },
            };

            double tolerance = 1e-9; 

            foreach (var orderTestCase in orderTestCases)
            {
                try
                {
                    object? resultValue = getCoefficientMatrixMethod.Invoke(null, new object[] { orderTestCase.order });

                    if (resultValue is double[,] matrix)
                    {
                        int expectedRows = orderTestCase.Expected.GetLength(0);
                        int expectedCols = orderTestCase.Expected.GetLength(1);
                        int actualRows = matrix.GetLength(0);
                        int actualCols = matrix.GetLength(1);

                        if (expectedRows != actualRows || expectedCols != actualCols)
                        {
                            result.Success = false;
                            result.ListErrors.Add($"GetCoefficientMatrix({orderTestCase.order}) - Розміри матриці не збігаються. Очікуваний {expectedRows}x{expectedCols}, але отримано {actualRows}x{actualCols}.");
                        }
                        else
                        {
                            bool matricesAreEqual = true;
                            List<string> mismatchDetails = new List<string>();

                            for (int i = 0; i < expectedRows; i++)
                            {
                                for (int j = 0; j < expectedCols; j++)
                                {
                                    if (Math.Abs(matrix[i, j] - orderTestCase.Expected[i, j]) > tolerance)
                                    {
                                        matricesAreEqual = false;
                                        mismatchDetails.Add($"Невідповідність при ({i},{j}): очікувано {orderTestCase.Expected[i, j]}, отримано {matrix[i, j]}");
                                    }
                                }
                            }

                            if (!matricesAreEqual)
                            {
                                result.Success = false;
                                result.ListErrors.Add($"GetCoefficientMatrix({orderTestCase.order}) - Очікувана матриця не відповідає результату.");
                                result.ListErrors.AddRange(mismatchDetails);
                            }
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.ListErrors.Add($"Неочікуваний тип повернення від методу GetCoefficientMatrix для order={orderTestCase.order}. Очікувано double[,].");
                    }
                }
                catch (TargetInvocationException tie)
                {
                    result.Success = false;
                    result.ListErrors.Add($"Виняток під час виконання методу GetCoefficientMatrix (order={orderTestCase.order}): {tie.InnerException?.Message ?? tie.Message}");
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ListErrors.Add($"Помилка виклику методу GetCoefficientMatrix (order={orderTestCase.order}): {ex.Message}");
                }
            }

            return result;
        }

        private TestResult TestMultiplyVectorMatrix(Type userCodeType)
        {
            var result = new TestResult { Success = true, ListErrors = new List<string>() }; 

            MethodInfo? multiplyVectorMatrixMethod = userCodeType.GetMethod("MultiplyVectorMatrix",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(double[]), typeof(double[,]) },
                null);

            if (multiplyVectorMatrixMethod == null)
            {
                result.Success = false;
                result.ListErrors.Add("Помилка перевірки: не вдалося знайти публічний статичний метод 'MultiplyVectorMatrix(double[], double[,])' у класі 'Program'.");
                return result;
            }

            int order = 3;
            double[,] M = {
                { -1, 3, -3, 1 },
                { 3, -6, 3, 0 },
                { -3, 3, 0, 0 },
                { 1, 0, 0, 0 }
            };

            double t1 = 0.5;
            double[] T1 = { Math.Pow(t1, 3), Math.Pow(t1, 2), t1, 1 };

            double[] expectedTM1 = { 0.125, 0.375, 0.375, 0.125 };


            var multiplyTestCases = new[]
            {
                new { Vector = T1, Matrix = M, Expected = expectedTM1 }
            };

            double tolerance = 1e-9; 

            foreach (var testCaseData in multiplyTestCases) 
            {
                try
                {
                    object? resultValue = multiplyVectorMatrixMethod.Invoke(null, new object[] { testCaseData.Vector, testCaseData.Matrix });

                    if (resultValue is double[] resultVector)
                    {
                        if (resultVector.Length != testCaseData.Expected.Length)
                        {
                            result.Success = false;
                            result.ListErrors.Add($"MultiplyVectorMatrix – довжина вектора результату не збігається. Очікувано {testCaseData.Expected.Length}, але отримано {resultVector.Length}.");
                        }
                        else
                        {
                            bool vectorsAreEqual = true;
                            List<string> mismatchDetails = new List<string>();

                            for (int i = 0; i < resultVector.Length; i++)
                            {
                                if (Math.Abs(resultVector[i] - testCaseData.Expected[i]) > tolerance)
                                {
                                    vectorsAreEqual = false;
                                    mismatchDetails.Add($"Невідповідність індексу {i}: очікувано {testCaseData.Expected[i]}, отримано {resultVector[i]}");
                                }
                            }

                            if (!vectorsAreEqual)
                            {
                                result.Success = false;
                                result.ListErrors.Add($"MultiplyVectorMatrix - Result vector не відповідає очікуваному vector.");
                                result.ListErrors.AddRange(mismatchDetails);
                            }
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.ListErrors.Add("Неочікуваний тип повернення від методу MultiplyVectorMatrix. Очікувано double[].");
                    }
                }
                catch (TargetInvocationException tie)
                {
                    result.Success = false;
                    result.ListErrors.Add($"Виняток під час виконання методу MultiplyVectorMatrix: {tie.InnerException?.Message ?? tie.Message}");
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ListErrors.Add($"Test failed: Error invoking MultiplyVectorMatrix method: {ex.Message}");
                }
            }

            return result;
        }
    }
}
