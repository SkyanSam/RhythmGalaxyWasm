using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using RhythmGalaxy;
using Raylib_cs;

public class BulletSpawner 
{
    public Dictionary<System.Type, int> refs;
    public float minRotation;
    public float maxRotation;
    public float offset = 0;
    public float offsetChange = 0;
    public int numberOfBullets;
    public bool isRandom;
    public bool isParent = true;
    public bool isManual = false;
    public float spawnRate;
    float timer = 0;
    public float bulletSpeed = 5;
    public float bulletLifetime = 10;
    public float bulletRotationChange = 0;

    public bool emitParticles;
    public float particleCooldown;
    public float particleLifeTime;
    public Vector2 position;

    float[] rotations;

    public Sound shootSound;

    ParticleEmitter deathEmitter;
    public bool usingDeathEmitter { get; private set; } = false;

    public BulletSpawner deepCopy
    {
        get
        {
            BulletSpawner copy = new BulletSpawner();
            copy.minRotation = minRotation;
            copy.maxRotation = maxRotation;
            copy.numberOfBullets = numberOfBullets;
            copy.isRandom = isRandom;
            copy.isParent = isParent;
            copy.isManual = isManual;
            copy.spawnRate = spawnRate;
            copy.bulletSpeed = bulletSpeed;
            copy.bulletLifetime = bulletLifetime;
            copy.bulletRotationChange = bulletRotationChange;

            copy.emitParticles = emitParticles;
            copy.particleCooldown = particleCooldown;
            copy.particleLifeTime = particleLifeTime;
            copy.position = position;

            return copy;
        }
    }
    // Update is called once per frame
    public void Update()
    {
        if (!isManual)
        {
            // DRAW
            Raylib.DrawRectangle((int)position.X - 4, (int)position.Y - 4, 8, 8, Color.RED);
            //
            if (timer <= 0)
            {
                SpawnBullets();
                timer = spawnRate;
                Raylib.PlaySound(shootSound);
            }
        }
        timer -= Globals.timeDelta;

        offset += offsetChange * Globals.timeDelta;

        if (usingDeathEmitter)
            deathEmitter.Update();
    }

    // Select a random rotation from min to max for each bullet
    public float[] RandomRotations()
    {
        rotations = new float[numberOfBullets];
        for (int i = 0; i < numberOfBullets; i++)
        {
            var difference = maxRotation - minRotation;
            rotations[i] = ((float)Globals.random.NextDouble() * difference) + minRotation;
        }
        return rotations;
    }
    
    // This will set random rotations evenly distributed between the min and max Rotation.
    public float[] DistributedRotations()
    {
        if (numberOfBullets == 1)
        {
            rotations = new float[1];
            rotations[0] = minRotation + offset;
        }
        else
        {
            rotations = new float[numberOfBullets];
            for (int i = 0; i < numberOfBullets; i++)
            {
                var fraction = (float)i / ((float)numberOfBullets - 1f);
                var difference = maxRotation - minRotation;
                var fractionOfDifference = fraction * difference;
                rotations[i] = fractionOfDifference + minRotation + offset; // We add minRotation to undo Difference
            }
        }
        return rotations;
    }
    public Bullet[] SpawnBullets()
    {
        if (isRandom) RandomRotations();
        else DistributedRotations();
        
        // Spawn Bullets
        Bullet[] spawnedBullets = new Bullet[numberOfBullets];
        for (int i = 0; i < numberOfBullets; i++)
        {
            spawnedBullets[i] = new Bullet();
            spawnedBullets[i].tag = "Enemy";
            spawnedBullets[i].position = position;
            spawnedBullets[i].rotation = rotations[i];
            spawnedBullets[i].speed = bulletSpeed;
            spawnedBullets[i].timer = bulletLifetime;
            spawnedBullets[i].rotationChange = bulletRotationChange;
            spawnedBullets[i].particleCooldown = particleCooldown;
            spawnedBullets[i].particleTimer = particleCooldown;
            spawnedBullets[i].emitParticles = emitParticles;
            spawnedBullets[i].particleLifeTime = particleLifeTime;
        }
        BulletManager.AddBullets(spawnedBullets);
        return spawnedBullets;
    }
}
