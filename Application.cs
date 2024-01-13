using System;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;
using Raylib_cs;

namespace RhythmGalaxy
{
    public partial class Application
    {
        public static RenderTexture2D renderTarget;
        static bool isPendingSceneSwitch = false;
        static string pendingSceneSwitch = "";
        static SpriteSystem spriteSystem;
        public static void Main()
        {
            try {
                Start();

#if !UseWebGL
                while (!Raylib.WindowShouldClose())
                {
                    Update();
                }
                Stop();        
#endif
            } catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
            finally
            {
#if !UseWebGL
                Console.ReadLine();
                Stop();
#endif
            }
        }

        public static void Start()
        {
            // Raylib
            const int screenWidth = 960;
            const int screenHeight = 540;
            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            Raylib.InitWindow(screenWidth, screenHeight, "Rhythm Galaxy");
            Raylib.InitAudioDevice();
            Raylib.SetTargetFPS(60);
            renderTarget = Raylib.LoadRenderTexture(960, 540);

            // ECS
            spriteSystem = new SpriteSystem();
            spriteSystem.Initialize();

            // Scenes
            Globals.scenes.Add("SampleScene", new SampleScene());
            Globals.scenes.Add("SampleTextScene", new SampleTextScene());
            Globals.currentScene = "SampleTextScene"; // use nameof
            Globals.scenes[Globals.currentScene].Start();
        }

        [JSExport]
        public static void Update()
        {
            Raylib.BeginDrawing();
            Raylib.BeginTextureMode(renderTarget);

            Raylib.ClearBackground(Color.WHITE);

            Globals.scenes[Globals.currentScene].Update();

            spriteSystem.UpdateAll();
            Raylib.EndTextureMode();
            Raylib.DrawTextureRec(renderTarget.texture, new Rectangle(0, 0, renderTarget.texture.width, -renderTarget.texture.height), new Vector2(0, 0), Color.WHITE);
            Raylib.EndDrawing();

            if (isPendingSceneSwitch)
            {
                Globals.scenes[Globals.currentScene].Stop();
                Globals.currentScene = pendingSceneSwitch;
                Globals.scenes[Globals.currentScene].Start();
                isPendingSceneSwitch = false;
                pendingSceneSwitch = "";
            }
        }

        public static void Stop()
        {
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

        public static void SwitchScene(string name)
        {
            isPendingSceneSwitch = true;
            pendingSceneSwitch = name;
        }
    }
}
