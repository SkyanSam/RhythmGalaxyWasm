using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;

namespace RhythmGalaxy
{
    class LogExcerpt1 : Scene
    {
        Font font;
        public static string lastScene = "";
        public void Start()
        {
            font = LoadFont("Resources/Fonts/ChavaRegular.ttf");
        }
        public void Update()
        {
            Raylib.ClearBackground(Color.BLACK);
            TextBoxSystem.DrawTextBoxed(font, "Log Placeholder", new Rectangle(250, 50, 460, 460), 40, 1f, true, Color.WHITE);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                Application.SwitchScene(lastScene);
            }
        }
        public void Stop()
        {

        }
    }
}
