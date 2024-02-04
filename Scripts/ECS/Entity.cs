using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy.ECS
{
    public struct Entity : Pooling.Poolable
    {
        //public Vector3 transform;
        public bool queueForPooling { get; set; }
        public Dictionary<Type, int> componentRefs;
        public Entity() { }
    }
}
