using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;

namespace RhythmGalaxy
{
    class LogExcerpt1 : LogExcerptBase
    {
        public override void SceneStart()
        {
            textLocation = "Resources/Logs/log1.txt";
            nextScene = nameof(SampleScene);
        }
    }
}
