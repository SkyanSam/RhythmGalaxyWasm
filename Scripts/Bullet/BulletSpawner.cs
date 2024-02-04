using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using RhythmGalaxy;
using Raylib_cs;
using RhythmGalaxy.ECS;

public struct BulletSpawner : Component
{
    public bool queueForPooling { get; set; }
    public float minRotation;
    public float maxRotation;
    public float offset = 0;
    public float offsetChange = 0;
    public int numberOfBullets;
    public bool isRandom;
    public bool isParent = true;
    public bool isManual = false;
    public float spawnRate;
    public float timer = 0;
    public float bulletSpeed = 5;
    public float bulletLifetime = 10;
    public float bulletRotationChange = 0;
    public bool queueSpawn;

    public bool emitParticles;
    public float particleCooldown;
    public float particleLifeTime;
    public Vector2 position;
    public string bulletTag;

    public BulletSpawner() { }
}
