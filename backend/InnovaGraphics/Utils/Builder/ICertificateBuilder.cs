using System.Drawing;

namespace InnovaGraphics.Utils.Builder
{
    public interface ICertificateBuilder
    {
        void DrawName(string name);
        void AddDate(DateTime date);
        void AddLecturer(string name);
        Bitmap Build();
    }
}
