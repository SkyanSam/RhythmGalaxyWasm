using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RhythmGalaxy
{
    public class StartupScene : Scene
    {
        Texture2D skyansam;
        Texture2D raylib;
        bool toggleCredits;
        bool toggleInstructions;
        Font font;
        public void Start()
        {
            skyansam = LoadTexture("Resources/skyansam_logo.png");
            raylib = LoadTexture("Resources/raylib_logo.png");
            font = LoadFont("Resources/Fonts/ChavaRegular.ttf");
        }
        public void Update()
        {
            Raylib.ClearBackground(Color.BLACK);
            var time = (float)GetTime();
            if (0 <= time && time <= 5)
            {
                float a = 1f - MathF.Abs((time - 2.5f) / 2.5f);
                a = a > 1f ? 1f : a < 0f ? 0f : a;
                Console.WriteLine($"a {a}");
                DrawTexturePro(skyansam, new Rectangle(0, 0, skyansam.width, skyansam.height), new Rectangle(0, 0, 960, 540), Vector2.Zero, 0, new Color(255, 255, 255, (int)(a * 255)));
            }
            else if (5 <= time && time <= 10)
            {
                time -= 5f;
                float a = 1f - MathF.Abs((time - 2.5f) / 2.5f);
                a = a > 1f? 1f: a < 0f? 0f: a;
                Console.WriteLine($"a {a}");
    
                DrawTextureEx(raylib, new Vector2(960 / 2, 540 / 2) - new Vector2(raylib.width, raylib.height), 0, 2f, new Color(255, 255, 255, (int)(a * 255)));
            }
            else if (10 < time)
            {
                if (!toggleCredits && !toggleInstructions)
                {
                    if (IsKeyPressed(KeyboardKey.KEY_E)) toggleCredits = !toggleCredits;
                    if (IsKeyPressed(KeyboardKey.KEY_F)) toggleInstructions = !toggleInstructions;
                    if (IsKeyPressed(KeyboardKey.KEY_SPACE)) Application.SwitchScene(nameof(LogExcerpt1));
                    DrawTextEx(font, "Rhythm Galaxy \n \n Press Space to Start \n \n Press E for Credits \n\n Press F for Instructions", new Vector2(50, 50), 40, 1f, Color.WHITE);
                }
                else if (toggleInstructions)
                {
                    DrawTextEx(font, "Move Player with WASD \n LSHIFT to enter defense mode. \n Defense mode slows the player down \n and increases energy. \n The more energy you have \n the higher your defense is. \n Press F to go back.", new Vector2(50, 50), 40, 1f, Color.WHITE);
                    if (IsKeyPressed(KeyboardKey.KEY_F)) toggleInstructions = !toggleInstructions;
                }
                else if (toggleCredits)
                {
                    DrawTextEx(font, "SkyanSam - Programming, Art\n \n Apechs - Music \n \n SandriDraws - Background Art \n \n Press E to go Back", new Vector2(50, 50), 40, 1f, Color.WHITE);
                    if (IsKeyPressed(KeyboardKey.KEY_E)) toggleCredits = !toggleCredits;
                }
                
            }
        }
        public void Stop() { 
        
        }  
    }
}
