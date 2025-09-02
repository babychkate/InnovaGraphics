using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Utils.Strategy.Interfaces;
using System.Drawing;
using System.Reflection;

namespace InnovaGraphics.Utils.Strategy.Lab4.Implementations
{
    public struct HSBColor
    {
        public float H;
        public float S;
        public float B;
    }

    public class RGBtoHSBTest : ICodeTest<IEnumerable<Case>, TestResult>
    {
        public string TestName => "RGBtoHSBTest";

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

            Type? hsbColorType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == nameof(HSBColor) && t.IsValueType);

            if (hsbColorType == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Не вдалося знайти структуру \"HSBColor\" у скомпільованій збірці.");
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
                return overallResult;
            }

            MethodInfo? rgbToHsbMethod = userCodeType.GetMethod("RGBtoHSB",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(Color) },
                null);

            if (rgbToHsbMethod == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося знайти публічний статичний метод \"RGBtoHSB(Color)\" у класі \"Program\".");
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
                return overallResult;
            }
            else if (rgbToHsbMethod.ReturnType != hsbColorType)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Метод 'RGBtoHSB' не повертає тип '{hsbColorType.FullName}'. Фактичний тип повернення: {rgbToHsbMethod.ReturnType.FullName}");
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
                return overallResult;
            }

            MethodInfo? hsbToRgbMethod = userCodeType.GetMethod("HSBtoRGB",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(float), typeof(float), typeof(float) },
                null);

            if (hsbToRgbMethod == null)
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add("Не вдалося знайти публічний статичний метод \"HSBtoRGB(float, float, float)\" у класі \"Program\".");
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
                return overallResult;
            }
            else if (hsbToRgbMethod.ReturnType != typeof(Color))
            {
                overallResult.Success = false;
                overallResult.ListErrors.Add($"Метод 'HSBtoRGB' не повертає тип '{typeof(Color).FullName}'. Фактичний тип повернення: {hsbToRgbMethod.ReturnType.FullName}");
                foreach (var caseItem in testCases)
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                }
                return overallResult;
            }

            double tolerance = 1e-6;
            var hardcodedTestCases = new List<(Color rgb, HSBColor expectedHsb, float h, float s, float b, Color expectedRgb)>
            {
                // Тест для RGBtoHSB
                (Color.FromArgb(255, 255, 0), new HSBColor { H = 60, S = 1, B = 1 }, 60, 1, 1, Color.FromArgb(255, 255, 0)),
                // Тест для HSBtoRGB
                (Color.Empty, new HSBColor(), 300, 0.5f, 1f, Color.FromArgb(255, 127, 255)),
            };
            var testCasesList = testCases.ToList();

            foreach (var caseItem in testCasesList)
            {
                int index = testCasesList.IndexOf(caseItem);
                if (index >= 0 && index < hardcodedTestCases.Count)
                {
                    var (rgb, expectedHsb, h, s, b, expectedRgb) = hardcodedTestCases[index];
                    bool casePassed = true;
                    List<string> caseErrors = new List<string>();

                    // Тестування RGBtoHSB
                    try
                    {
                        object? rgbToHsbResult = rgbToHsbMethod?.Invoke(null, new object[] { rgb });
                        if (rgbToHsbResult != null && rgbToHsbResult.GetType() == hsbColorType)
                        {
                            FieldInfo? hField = hsbColorType.GetField("H");
                            FieldInfo? sField = hsbColorType.GetField("S");
                            FieldInfo? bField = hsbColorType.GetField("B");

                            if (hField?.GetValue(rgbToHsbResult) is float actualH &&
                                sField?.GetValue(rgbToHsbResult) is float actualS &&
                                bField?.GetValue(rgbToHsbResult) is float actualB)
                            {
                                bool hPassed = Math.Abs(actualH - expectedHsb.H) <= tolerance;
                                bool sPassed = Math.Abs(actualS - expectedHsb.S) <= tolerance;
                                bool bPassed = Math.Abs(actualB - expectedHsb.B) <= tolerance;

                                if (!hPassed) caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Очікуваний H {expectedHsb.H}, отримано {actualH}.");
                                if (!sPassed) caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Очікуваний S {expectedHsb.S}, отримано {actualS}.");
                                if (!bPassed) caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Очікуваний B {expectedHsb.B}, отримано {actualB}.");

                                if (!hPassed || !sPassed || !bPassed) casePassed = false;
                            }
                            else
                            {
                                caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Не вдалося отримати значення полів H, S або B з результату.");
                                casePassed = false;
                            }
                        }
                        else
                        {
                            caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Неправильний тип повернення або null.");
                            casePassed = false;
                        }
                    }
                    catch (TargetInvocationException tie)
                    {
                        caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Виняток: {tie.InnerException?.Message ?? tie.Message}");
                        casePassed = false;
                    }
                    catch (Exception ex)
                    {
                        caseErrors.Add($"Тесткейс {caseItem.Id} (RGBtoHSB): Неочікувана помилка: {ex.Message}");
                        casePassed = false;
                    }

                    // Тестування HSBtoRGB
                    try
                    {
                        object? hsbToRgbResult = hsbToRgbMethod?.Invoke(null, new object[] { h, s, b });
                        if (hsbToRgbResult != null && hsbToRgbResult is Color actualRgb)
                        {
                            bool rPassed = actualRgb.R == expectedRgb.R;
                            bool gPassed = actualRgb.G == expectedRgb.G;
                            bool bPassed = actualRgb.B == expectedRgb.B;

                            if (!rPassed) caseErrors.Add($"Тесткейс {caseItem.Id} (HSBtoRGB): Очікуваний R {expectedRgb.R}, отримано {actualRgb.R}.");
                            if (!gPassed) caseErrors.Add($"Тесткейс {caseItem.Id} (HSBtoRGB): Очікуваний G {expectedRgb.G}, отримано {actualRgb.G}.");
                            if (!bPassed) caseErrors.Add($"Тесткейс {caseItem.Id} (HSBtoRGB): Очікуваний B {expectedRgb.B}, отримано {actualRgb.B}.");

                            if (!rPassed || !gPassed || !bPassed) casePassed = false;
                        }
                        else
                        {
                            caseErrors.Add($"Тесткейс {caseItem.Id} (HSBtoRGB): Неправильний тип повернення або null.");
                            casePassed = false;
                        }
                    }
                    catch (TargetInvocationException tie)
                    {
                        caseErrors.Add($"Тесткейс {caseItem.Id} (HSBtoRGB): Виняток: {tie.InnerException?.Message ?? tie.Message}");
                        casePassed = false;
                    }
                    catch (Exception ex)
                    {
                        caseErrors.Add($"Тесткейс {caseItem.Id} (HSBtoRGB): Неочікувана помилка: {ex.Message}");
                        casePassed = false;
                    }

                    overallResult.CaseResults[caseItem.Id] = casePassed;
                    overallResult.ListErrors.AddRange(caseErrors);
                }
                else
                {
                    overallResult.CaseResults[caseItem.Id] = false;
                    overallResult.ListErrors.Add($"Тесткейс {caseItem.Id}: Відсутній відповідний жорстко закодований тест для виконання (індекс за межами діапазону).");
                }
            }

            overallResult.Success = !overallResult.CaseResults.Values.Any(r => !r) && !overallResult.ListErrors.Any();

            return overallResult;
        }
    }
}