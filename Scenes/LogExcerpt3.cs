using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace RhythmGalaxy
{
    class LogExcerpt3 : LogExcerptBase
    {
        public override void SceneStart()
        {
            textLocation = "Resources/Logs/log3.txt";
            nextScene = nameof(StartupScene);
        }
    }
}
