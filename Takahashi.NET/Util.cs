using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Takahashi.NET
{
    public static class Util
    {
        public static IntPtr Font;
        public static SDL_Color White;

        public static void Init()
        {
            Font = TTF_OpenFont("NotoSans-Regular.ttf", 40);
            White = new()
            {
                r = 255,
                g = 255,
                b = 255,
                a = 255,
            };
        }

        public static List<Slide> LoadSlides(string filename)
        {
            var slides = new List<Slide>();
            var text = File.ReadAllText(filename);

            // separate slides by double newlines to allow for multiple paragraphs in a slide
            var lines = text.Split(new[] { "\n\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (line == "!") // empty slide
                {
                    slides.Add(new Slide(""));
                    continue;
                }

                slides.Add(new Slide(line));
            }

            return slides;
        }

        public static SDL_Rect GetDisplayRect()
        {
            if (SDL_GetDisplayBounds(0, out var rect) < 0)
            {
                return new();
            }

            return rect;
        }
    }
}