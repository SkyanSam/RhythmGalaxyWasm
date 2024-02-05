using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy
{
    class LogExcerpt2 : LogExcerptBase
    {
        public override void SceneStart()
        {
            textLocation = "Resources/Logs/log2.txt";
            nextScene = nameof(BossScene);
        }
    }
}
