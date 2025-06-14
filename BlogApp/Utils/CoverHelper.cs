using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using System.IO;
using System.Linq;

namespace BlogApp.Utils
{
    public class CoverHelper
    {
        public static void GenerateCover(string title, string savePath, int width = 420, int height = 560)
        {
            using var image = new Image<Rgba32>(width, height, Color.LightGray);

            // 字体
            Font font;
            try
            {
                var fontFamily = SystemFonts.Families.FirstOrDefault(ff => ff.Name.Contains("黑体") || ff.Name.Contains("SimHei") || ff.Name.Contains("Arial"));
                if (fontFamily == null)
                    fontFamily = SystemFonts.Families.First();
                font = fontFamily.CreateFont(36, FontStyle.Bold);
            }
            catch
            {
                font = SystemFonts.CreateFont("Arial", 36, FontStyle.Bold);
            }

            var richTextOptions = new RichTextOptions(font)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Origin = new PointF(width / 2f, height / 2f)
            };

            var textColor = Color.DarkSlateBlue;

            image.Mutate(ctx =>
                ctx.DrawText(richTextOptions, title, textColor)
            );

            image.Save(savePath);
        }
    }
}
