using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;
using Raylib_cs;
using RhythmGalaxy.ECS;
using RhythmGalaxy.Scenes;

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
                Console.WriteLine("\n --- Stack Trace --- \n");
                Console.WriteLine(e.StackTrace);
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

            

            //Globals.tweenHandler = [];
            // Scenes
            Globals.scenes.Add(nameof(SampleScene), new SampleScene());
            Globals.scenes.Add(nameof(SampleTextScene), new SampleTextScene());
            Globals.scenes.Add(nameof(GameOverScene), new GameOverScene());
            Globals.scenes.Add(nameof(LogExcerpt1), new LogExcerpt1());
            Globals.scenes.Add(nameof(LogExcerpt2), new LogExcerpt2());
            Globals.scenes.Add(nameof(LogExcerpt3), new LogExcerpt3());
            Globals.scenes.Add(nameof(BossScene), new BossScene());
            Globals.scenes.Add(nameof(StartupScene), new StartupScene());

            Globals.currentScene = nameof(StartupScene); // use nameof
            Globals.scenes[Globals.currentScene].Start();
            

            
            //tween = ((SampleScene)Globals.scenes[Globals.currentScene]).Tween(x => x.hp).To(1.0).In(1.0).Repeat(4).Ease(Easing.CubicEaseInOut);


        }
        
        [JSExport]
        public static void Update()
        {
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
