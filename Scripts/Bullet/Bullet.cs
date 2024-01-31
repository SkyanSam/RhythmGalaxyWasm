using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using RhythmGalaxy;
using RhythmGalaxy.ECS;

public struct Bullet : Component
{
    public bool queueForPooling { get; set; }
    public Vector2 position;
    public Vector2 velocity;
    public float speed;
    public float rotation;
    public float timer;
    public float rotationChange;
    public string tag;
    public bool emitParticles;
    public float particleCooldown;
    public float particleTimer;
    public float particleLifeTime;
}
