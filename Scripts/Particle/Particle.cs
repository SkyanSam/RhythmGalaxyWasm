using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Raylib_cs;
namespace RhythmGalaxy
{
    public struct Particle
    {
        public int radius;
        public Vector2 position;
        public Vector2 velocity;
        public float timer;
        public float lifeTime;
        public Color startColor;
        public Color endColor;
    }
}
