/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace RhythmGalaxy
{
    class LevelManager
    {
        public static LevelInfo levelInfo;
        public static dynamic script;
        public static Music music;
        public static void Start()
        {
            music = levelInfo.LoadMusic();
            script = levelInfo.LoadScript();

            PlayMusicStream(music);
            script.Start();
        }
        public static void Update()
        {
            script.Update();
        }
        public static double GetTime()
        {
            return GetMusicTimePlayed(music);
        }
    }
}*/
