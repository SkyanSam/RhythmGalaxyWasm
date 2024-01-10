using System;
using System.Collections.Generic;
using System.Text;
using HooperSought;
using Raylib_cs;
using System.Numerics;
using RhythmGalaxy;
public class ParticleEmitter
{
    public Vector2 position;
    public float particleSpeed;
    public float particleLifeTime;
    public float particleSpawnRate;
    public float particleRadius;
    public Color startColor;
    public Color endColor;
    float timer;
    public void Update()
    {
        if (timer <= 0) {
            var particle = new Particle();
            particle.startColor = startColor;
            particle.endColor = endColor;
            particle.lifeTime = particleLifeTime;
            particle.timer = particleLifeTime;
            particle.radius = (int)MathF.Round(particleRadius);
            particle.position = position;
            particle.velocity = new Vector2(((float)Globals.random.NextDouble() * 2f) - 1f, ((float)Globals.random.NextDouble() * 2f) - 1f) * particleSpeed;
            ParticleManager.AddParticle(particle);
            timer = particleSpawnRate;
        }
        timer -= Globals.timeDelta;
    }
}

