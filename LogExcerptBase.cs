using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using System.IO;
using static Raylib_cs.Raylib;
namespace RhythmGalaxy
{
    class LogExcerptBase : Scene
    {
        Font font;
        public string nextScene;
        public string textLocation;
        public string[] text;
        public string outputText = "";
        int textIndex = 0;
        int charIndex = 0;
        float textCooldown = .01f;
        float textTimer = 0;
        bool fastMode = false;
        public virtual void SceneStart()
        {

        }
        public void Start()
        {
            SceneStart();
            font = LoadFont("Resources/Fonts/ChavaRegular.ttf");
            Console.WriteLine($"finding at dir {Directory.GetCurrentDirectory()}/{textLocation}");
            text = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/{textLocation}");
            textIndex = 0;
            charIndex = 0;
        }
        public void Update()
        {
            textTimer -= Globals.timeDelta;

            ClearBackground(Color.BLACK);
            TextBoxSystem.DrawTextBoxed(font, outputText, new Rectangle(180, 70, 600, 400), 30, 1f, true, Color.WHITE);

            if ((textTimer <= 0 || fastMode) && charIndex < text[textIndex].Length - 1)
            {
                if (fastMode)
                {
                    while (charIndex < text[textIndex].Length - 1)
                    {
                        outputText += text[textIndex][charIndex];
                        charIndex++;
                    }
                    fastMode = false;
                }
                else
                {
                    outputText += text[textIndex][charIndex];
                    charIndex++;
                    if(IsKeyPressed(KeyboardKey.KEY_SPACE) )
                    {
                        fastMode = true;
                    }
                }
                if (charIndex >= text[textIndex].Length - 1)
                {
                    fastMode = false;
                    outputText += "\n\nPRESS SPACE TO CONTINUE";
                }
                textTimer = textCooldown;
            }
            if (charIndex >= text[textIndex].Length - 1 && IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                outputText = "";
                textIndex++;
                charIndex = 0;
            }
            
            
            if (textIndex >= text.Length - 1 && charIndex >= text[text.Length - 1].Length - 1 && IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                Application.SwitchScene(nextScene);
            }
        }
        public void Stop()
        {

        }
    }
}
