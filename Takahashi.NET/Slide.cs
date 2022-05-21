using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Takahashi.NET
{
    public class Slide
    {
        public string Text;

        private IntPtr _surface;
        private IntPtr _textTexture;
        private SDL_Rect _rect = new();
        private SDL_Rect _rect2 = new();

        public Slide(string text)
        {
            Text = text;

            if (Text == "") return;

            _surface = TTF_RenderText_Blended_Wrapped(Util.Font, Text, Util.White, (uint)Util.GetDisplayRect().w - 20);
            _textTexture = SDL_CreateTextureFromSurface(Program.Renderer, _surface);

            SDL_QueryTexture(_textTexture, out _, out _, out _, out _rect2.h);

            var lines = Text.Split('\n');
            var biggestWidth = 0;
            foreach (var line in lines)
            {
                var surface = TTF_RenderText_Blended_Wrapped(Util.Font, line, Util.White, (uint)Util.GetDisplayRect().w - 20);
                var textTexture = SDL_CreateTextureFromSurface(Program.Renderer, surface);
                SDL_QueryTexture(textTexture, out _, out _, out var width, out _);
                SDL_FreeSurface(surface);
                SDL_DestroyTexture(textTexture);

                if (width > biggestWidth)
                {
                    biggestWidth = width;
                }
            }

            _rect2.w = biggestWidth;

            // center the text
            _rect.x = Util.GetDisplayRect().w / 2 - _rect2.w / 2;
            _rect.y = Util.GetDisplayRect().h / 2 - _rect2.h / 2;
            _rect.w = _rect2.w;
            _rect.h = _rect2.h;
        }

        public void Draw()
        {
            if (Text == "") return;

            var lines = Text.Split('\n');
            var biggestWidth = 0;
            foreach (var line in lines)
            {
                var surface = TTF_RenderText_Blended_Wrapped(Util.Font, line, Util.White, (uint)Util.GetDisplayRect().w - 20);
                var textTexture = SDL_CreateTextureFromSurface(Program.Renderer, surface);
                SDL_QueryTexture(textTexture, out _, out _, out var width, out _);
                SDL_FreeSurface(surface);
                SDL_DestroyTexture(textTexture);

                if (width > biggestWidth)
                {
                    biggestWidth = width;
                }
            }

            _rect2.w = biggestWidth;

            // center the text
            _rect.x = Util.GetDisplayRect().w / 2 - _rect2.w / 2;
            _rect.y = Util.GetDisplayRect().h / 2 - _rect2.h / 2;
            _rect.w = _rect2.w;
            _rect.h = _rect2.h;

            SDL_RenderCopy(Program.Renderer, _textTexture, ref _rect2, ref _rect);
        }
    }
}