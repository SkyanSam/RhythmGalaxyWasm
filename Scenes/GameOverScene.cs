using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;

namespace RhythmGalaxy.Scenes
{
    public class GameOverScene : Scene
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
            TextBoxSystem.DrawTextBoxed(font, "Game Over. \n\n Press Space To Try Again", new Rectangle(250, 50, 460, 460), 40, 1f, true, Color.WHITE);
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
