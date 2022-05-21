using static SDL2.SDL;
using static SDL2.SDL_ttf;

using static SDL2.SDL.SDL_WindowFlags;
using static SDL2.SDL.SDL_RendererFlags;
using static SDL2.SDL.SDL_EventType;
using static SDL2.SDL.SDL_Keycode;

namespace Takahashi.NET
{
    internal static class Program
    {
        public static IntPtr Window;
        public static IntPtr Renderer;

        private static List<Slide> _slides;
        private static Slide _currentSlide;

        private static void Main(string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]) || args.Length >= 3)
            {
                Console.WriteLine("usage: takahashi <file> [--windowed]");
                return;
            }

            if (args.Length == 2 && args[1] != "--windowed")
            {
                Console.WriteLine("usage: takahashi <file> [--windowed]");
                return;
            }

            if (SDL_Init(SDL_INIT_VIDEO) < 0) return;
            if (TTF_Init() < 0) return;

            if (args.Length == 2 && args[1] == "--windowed")
            {
                Window = SDL_CreateWindow("Takahashi", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
                    1280, 720, SDL_WINDOW_RESIZABLE | SDL_WINDOW_SHOWN);
            }
            else
            {
                Window = SDL_CreateWindow("Takahashi", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
                    0, 0, SDL_WINDOW_FULLSCREEN_DESKTOP);
            }

            SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "linear");
            
            Renderer = SDL_CreateRenderer(Window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);

            Util.Init();
            _slides = Util.LoadSlides(args[0]);
            _currentSlide = _slides[0];

            var quit = false;
            while (!quit)
            {
                while (SDL_PollEvent(out var e) != 0)
                {
                    if (e.type == SDL_QUIT)
                    {
                        quit = true;
                    }

                    if (e.type == SDL_KEYDOWN)
                    {
                        if (e.key.keysym.sym == SDLK_ESCAPE)
                        {
                            quit = true;
                        }

                        if (e.key.keysym.sym == SDLK_SPACE || 
                            e.key.keysym.sym == SDLK_RETURN || e.key.keysym.sym == SDLK_RIGHT)
                        {
                            _currentSlide = _slides[(_slides.IndexOf(_currentSlide) + 1) % _slides.Count];
                        }

                        if (e.key.keysym.sym == SDLK_LEFT)
                        {
                            _currentSlide = _slides[(_slides.IndexOf(_currentSlide) - 1 + _slides.Count) % _slides.Count];
                        }
                    }
                }

                SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
                SDL_RenderClear(Renderer);

                _currentSlide.Draw();
                SDL_RenderPresent(Renderer);
            }

            SDL_DestroyRenderer(Renderer);
            SDL_DestroyWindow(Window);
            TTF_Quit();
            SDL_Quit();
        }
    }
}