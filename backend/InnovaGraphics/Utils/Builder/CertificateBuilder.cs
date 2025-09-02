using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace InnovaGraphics.Utils.Builder
{
    public class CertificateBuilder : ICertificateBuilder, IDisposable 
    {
        private Bitmap _image;
        private Graphics _graphics;

        private readonly Font _nameFont = new Font("Arial", 42, FontStyle.Italic);
        private readonly Font _lecturerFont = new Font("Arial", 14, FontStyle.Italic);
        private readonly Font _dateFont = new Font("Arial", 14, FontStyle.Italic);

        public CertificateBuilder(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                _image = new Bitmap(imagePath);
            }
            else
            {
                _image = new Bitmap(800, 600, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(_image)) 
                {
                    g.Clear(Color.White);
                }
            }

            _graphics = Graphics.FromImage(_image);
            _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        public void AddDate(DateTime date)
        {
            float rightX = _image.Width - 310;
            float bottomY = _image.Height - 200;
            StringFormat rightFormat = new StringFormat { Alignment = StringAlignment.Far };

            _graphics.DrawString(date.ToString("dd.MM.yyyy"), _dateFont, Brushes.Black, rightX, bottomY, rightFormat);
        }

        public void AddLecturer(string name)
        {
            float leftX = 285;
            float bottomY = _image.Height - 200;
            StringFormat leftFormat = new StringFormat { Alignment = StringAlignment.Near };

            _graphics.DrawString(name, _lecturerFont, Brushes.Black, leftX, bottomY, leftFormat);
        }

        public void DrawName(string name)
        {
            float centerX = _image.Width / 2;
            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };

            SizeF nameSize = _graphics.MeasureString(name, _nameFont, _image.Width, centerFormat);
            float nameY = (_image.Height - nameSize.Height) / 2 + 30;

            _graphics.DrawString(name, _nameFont, Brushes.Black, centerX, nameY, centerFormat);
        }

        public Bitmap Build()
        {
            return _image;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _graphics?.Dispose(); 
                _image?.Dispose();   
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CertificateBuilder() 
        {
            Dispose(false);
        }
    }
}
