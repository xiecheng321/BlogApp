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
        public static void GenerateCover(string title, string subtitle, string savePath, int width = 420, int height = 560)
        {
            var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/SourceHanSansCN-Regular.otf");
            var collection = new FontCollection();
            var fontFamily = collection.Add(fontPath);

            using var image = new Image<Rgba32>(width, height);

            // 美化背景：渐变 + 光影
            image.Mutate(ctx => ctx.Fill(new LinearGradientBrush(
                new PointF(0, 0), new PointF(width, height),
                GradientRepetitionMode.None,
                new ColorStop(0f, Color.WhiteSmoke),
                new ColorStop(1f, Color.LightGray))
            ));

            // 字体设置
            int titleFontSize = title.Length > 15 ? 40 : 52;
            var titleFont = fontFamily.CreateFont(titleFontSize, FontStyle.Bold);
            var subtitleFont = fontFamily.CreateFont(20, FontStyle.Italic);

            var titleOptions = new RichTextOptions(titleFont)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Origin = new PointF(width / 2f, height * 0.3f)
            };

            var subtitleOptions = new RichTextOptions(subtitleFont)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Origin = new PointF(width / 2f, height * 0.85f)
            };

            // 渲染文字
            image.Mutate(ctx =>
            {
                ctx.DrawText(titleOptions, title, Color.MediumSlateBlue);
                if (!string.IsNullOrWhiteSpace(subtitle))
                {
                    ctx.DrawText(subtitleOptions, subtitle, Color.DimGray);
                }
            });

            image.Save(savePath);
        }


    }
}
