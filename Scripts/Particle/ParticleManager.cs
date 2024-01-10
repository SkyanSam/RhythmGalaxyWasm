using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;
using RhythmGalaxy;

namespace RhythmGalaxy
{
    public static class ParticleManager
    {
        public static List<Particle> particles = new List<Particle>();
        public static void Update()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                // UPDATE
                var item = particles[i];
                item.position += item.velocity * Globals.timeDelta;
                item.timer -= Globals.timeDelta;


                particles[i] = item;

                Color color = Globals.LerpColor(item.startColor, item.endColor, -(item.timer / item.lifeTime) + 1);
               
                // DRAW
                Raylib.DrawRectangle((int)item.position.X - item.radius, (int)item.position.Y - item.radius, item.radius, item.radius, color);

                if (item.timer < 0)
                {
                    particles.RemoveAt(i);
                    i--;
                    // NEED TO CHECK FOR INDEX RANGE EXCEPTIONS.
                }
            }
        }
        public static int AddParticle(Particle p)
        {
            int index = particles.Count;
            particles.Add(p);
            return index;
        }
    }
}
