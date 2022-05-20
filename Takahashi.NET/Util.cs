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
            Font = TTF_OpenFont("NotoSans-Regular.ttf", 24);
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
            var lines = text.Split(new[] { "\r\n\r\n" }, StringSplitOptions.None);
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

        public static uint GetScreenWidth()
        {
            SDL_GetWindowSize(Program.Window, out var w, out _);
    
            return (uint)w;
        }
    }
}