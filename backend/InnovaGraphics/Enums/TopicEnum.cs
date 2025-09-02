using System.ComponentModel;

namespace InnovaGraphics.Enums
{
    public enum ThemeEnum
    {
        [Description("Побудова двовимірних зображень")]
        Graphics2D, 
            
        [Description("Програмування кривої Безьє")]
         BezierCurves, 

        [Description("Побудова фрактальних зображень")]
        FractalGraphics, 

        [Description("Колірні моделі та алгоритми їхнього перетворення")]
        ColorModelsAndTransformations,

        [Description("Програмування рухомих зображень")]
        AnimationProgramming 
    }
}

