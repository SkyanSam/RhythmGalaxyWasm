using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Raylib_cs;
using RhythmGalaxy;

namespace RhythmGalaxy
{
    public static class BulletManager
    {
        public static List<Bullet> bullets = new List<Bullet>();
        public static List<BulletSpawner> spawners = new List<BulletSpawner>();
        public static int bulletRadius = 2;
        public static Color bulletColor = Color.RED;
        public static bool areSpawnersDead { get; private set; }
        static bool IsDead(BulletSpawner spawner)
        {
            return spawner.isManual && !spawner.usingDeathEmitter;
        }
        public static void Update()
        {
            bool temp_areSpawnersDead = true;
            for (int i = 0; i < spawners.Count; i++)
            {
                spawners[i].Update();
                if (!IsDead(spawners[i])) temp_areSpawnersDead = false;
            }
            areSpawnersDead = temp_areSpawnersDead;

            for (int i = 0; i < bullets.Count; i++)
            {
                // UPDATE
                var item = bullets[i];
                item.velocity = new Vector2(MathF.Cos(item.rotation * MathF.PI / 180f), -MathF.Sin(item.rotation * MathF.PI / 180f)) * item.speed;
                item.position += item.velocity * Globals.timeDelta;
                item.rotation += item.rotationChange * Globals.timeDelta;

                Collider collider = new Collider();
                collider.x = item.position.X;
                collider.y = item.position.Y;
                collider.height = bulletRadius * 2;
                collider.width = bulletRadius * 2;

                Color bulletColor = item.tag == "Player" ? Color.BLUE : Color.RED;

                // PARTICLES
                if (item.emitParticles && item.particleTimer <= 0)
                {
                    Particle particle = new Particle();
                    particle.position = item.position;
                    particle.velocity = Vector2.Zero;
                    particle.lifeTime = item.particleLifeTime;
                    particle.timer = particle.lifeTime;
                    particle.radius = bulletRadius / 2;
                    particle.startColor = bulletColor;
                    particle.endColor = Color.WHITE;
                    item.particleTimer = item.particleCooldown;
                    ParticleManager.AddParticle(particle);
                }

                item.timer -= Globals.timeDelta;
                item.particleTimer -= Globals.timeDelta;

                bullets[i] = item;

                // DRAW
                
                Raylib.DrawRectangle((int)item.position.X - bulletRadius, (int)item.position.Y - bulletRadius, bulletRadius * 2, bulletRadius * 2, bulletColor);

                if (item.timer < 0 /*|| collider.IsCollidingWith("Wall")*/)
                {
                    bullets.RemoveAt(i);
                    i--;
                    // NEED TO CHECK FOR INDEX RANGE EXCEPTIONS.
                }
            }
        }
        public static int AddBullet(Bullet b)
        {
            int index = bullets.Count;
            bullets.Add(b);
            return index;
        }
        public static void AddBullets(Bullet[] bList)
        {
            //typeof(Bullet[]);
            foreach (var bullet in bList)
                bullets.Add(bullet);
               
        }

    }
}
