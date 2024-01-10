using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmGalaxy.ECS;
using Raylib_cs;
namespace RhythmGalaxy
{
    public struct SpriteComponent : Component
    {
        public bool queueForPooling { get; set; }
        public Texture2D texture2D;
        public float scaleX;
        public float scaleY;
    }
}
