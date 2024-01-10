using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmGalaxy.ECS;

namespace RhythmGalaxy
{
    public struct TransformComponent : Component
    {
        public bool queueForPooling { get; set; }
        public float xPosition;
        public float yPosition;
        public float zRotation;
        public float xScale;
        public float yScale;
    }
}
