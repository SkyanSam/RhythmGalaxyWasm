using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;
using Raylib_cs;
using RhythmGalaxy.ECS;
using RhythmGalaxy.Scenes;
using System.Collections.Generic;

namespace RhythmGalaxy
{
    public partial class Application
    {
        public static RenderTexture2D renderTarget;
        static bool isPendingSceneSwitch = false;
        static string pendingSceneSwitch = "";
        static SpriteSystem spriteSystem;
        static BulletManager bulletManager;
        static HitboxSystem hitboxSystem;
        public static void Main()
        {
#if !UseWebGL
            try {
#endif
                Start();

#if !UseWebGL
                while (!Raylib.WindowShouldClose())
                {
                    Update();
                }
                Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
                Console.WriteLine("\n --- Stack Trace --- \n");
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.ReadLine();
                Stop();

            }
#endif
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

            
            
            //Globals.tweenHandler = [];
            
            

            
           


        }

        [JSExport]
        public static void Update()
        {
            // Scenes
            if (Globals.currentScene == "") 
            {
            
                Globals.scenes.Add(nameof(SampleScene), new SampleScene());
                Globals.scenes.Add(nameof(SampleTextScene), new SampleTextScene());
                Globals.scenes.Add(nameof(GameOverScene), new GameOverScene());
                Globals.scenes.Add(nameof(LogExcerpt1), new LogExcerpt1());
                Globals.scenes.Add(nameof(LogExcerpt2), new LogExcerpt2());
                Globals.scenes.Add(nameof(LogExcerpt3), new LogExcerpt3());
                Globals.scenes.Add(nameof(BossScene), new BossScene());
                Globals.scenes.Add(nameof(StartupScene), new StartupScene());

                Globals.currentScene = nameof(SampleScene); // use nameof
                Globals.scenes[Globals.currentScene].Start();
            }
            // End Scenes

            //tween.Update(1000 / 60);
            //Globals.tweenHandler.Update(1000 / 60);
            Raylib.BeginDrawing();
            Raylib.BeginTextureMode(renderTarget);

            Raylib.ClearBackground(Color.WHITE);

            Globals.scenes[Globals.currentScene].Update();

            

            //Globals.scenes[Globals.currentScene].UpdateUI();
            Raylib.EndTextureMode();
            Raylib.DrawTextureRec(renderTarget.texture, new Rectangle(0, 0, renderTarget.texture.width, -renderTarget.texture.height), new Vector2(0, 0), Color.WHITE);
            Raylib.EndDrawing();

            
            if (isPendingSceneSwitch)
            {
                Globals.scenes[Globals.currentScene].Stop();
                Globals.currentScene = pendingSceneSwitch;
                Database.Reset();
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
