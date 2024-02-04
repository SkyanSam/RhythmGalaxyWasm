using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Raylib_cs;
using RhythmGalaxy;
using RhythmGalaxy.ECS;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RhythmGalaxy
{
    public class BulletManager : BaseSystem
    {
        public static List<Bullet> bullets = new List<Bullet>();
        public static List<int> bulletIndices = new List<int>();
        public static List<BulletSpawner> spawners = new List<BulletSpawner>();

        public static Texture2D bulletSpriteSheet;

        public static int playerBulletSpriteID;
        public static int enemyBulletSpriteID;

        public override void Initialize()
        {
            updateFormat = UpdateFormat.UpdateNRequestsNComponentSets;
            typesetList = new List<Type[]>()
            {
                new Type[] {typeof(Bullet), typeof(TransformComponent), typeof(HitboxComponent) },
                new Type[] {typeof(BulletSpawner), typeof(TransformComponent), typeof(HitboxComponent) }
            };
            bulletSpriteSheet = LoadTexture("Resources/Sprites/Bullets.png");

            enemyBulletSpriteID = Database.AddComponent(new SpriteComponent()
            {
                texture2D = bulletSpriteSheet,
                source = new Rectangle(0, 0, 6, 6),
                useSource = true,
                scaleX = 2f,
                scaleY = 2f,
            });

            playerBulletSpriteID = Database.AddComponent(new SpriteComponent()
            {
                texture2D = bulletSpriteSheet,
                source = new Rectangle(6, 0, 6, 6),
                useSource = true,
                scaleX = 2f,
                scaleY = 2f,
            });

            base.Initialize();
        }
        public override void UpdateNComponentSetsNRequests(List<Dictionary<Type, List<int>>> componentSetsList)
        {
            bullets = GetComponents<Bullet>(0, componentSetsList);
            var bulletTransforms = GetComponents<TransformComponent>(0, componentSetsList);
            var bulletHitboxes = GetComponents<HitboxComponent>(0, componentSetsList);
            bullets = GetComponents<Bullet>(0, componentSetsList);
            bulletIndices = GetComponentsIndices<Bullet>(0, componentSetsList);
            spawners = GetComponents<BulletSpawner>(1, componentSetsList);
            var spawnerTransforms = GetComponents<TransformComponent>(1, componentSetsList);

            Update();

            for (int i = 0; i < bullets.Count; i++)
            {
                var transform = bulletTransforms[i];
                var hitbox = bulletHitboxes[i];
                transform.xPosition = bullets[i].position.X;
                transform.yPosition = bullets[i].position.Y;
                hitbox.x = (int)bullets[i].position.X;
                hitbox.y = (int)bullets[i].position.Y;
                bulletTransforms[i] = transform;
                bulletHitboxes[i] = hitbox;

                //Console.WriteLine($"i{i}, x{transform.xPosition}, y{transform.yPosition}");
            }
            for (int i = 0; i < spawners.Count; i++)
            {
                var transform = spawnerTransforms[i];
                var spawner = spawners[i];
                spawner.position = new Vector2(transform.xPosition, transform.yPosition);
                spawners[i] = spawner;
            }

            SetComponents(0, componentSetsList, bullets);
            SetComponents(0, componentSetsList, bulletTransforms);
            SetComponents(0, componentSetsList, bulletHitboxes);
            SetComponents(1, componentSetsList, spawners);
        }
        void Update()
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                var spawner = spawners[i];
                UpdateSpawner(ref spawner);
                spawners[i] = spawner;
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                // UPDATE
                var item = bullets[i];
                item.velocity = new Vector2(MathF.Cos(item.rotation * MathF.PI / 180f), -MathF.Sin(item.rotation * MathF.PI / 180f)) * item.speed;
                item.position += item.velocity * Globals.timeDelta;
                item.rotation += item.rotationChange * Globals.timeDelta;
                item.timer -= Globals.timeDelta;
                bullets[i] = item;

                if (item.timer < 0)
                {
                    var e = Database.FindEntity([typeof(Bullet)], [bulletIndices[i]]);
                    Database.RemoveEntity(e);
                }

                // debug
                //Raylib.DrawCircle((int)item.position.X, (int)item.position.Y, 20f, Color.SKYBLUE);
            }
        }
        public static void AddBullet(Bullet b)
        {
            //int index = bullets.Count;
            var entity = new Entity();
            entity.componentRefs = new Dictionary<Type, int>();
            entity.componentRefs[typeof(Bullet)] = Database.AddComponent(b);
            entity.componentRefs[typeof(TransformComponent)] = Database.AddComponent(new TransformComponent()
            {
                xPosition = b.position.X,
                yPosition = b.position.Y,
                xScale = 1f,
                yScale = 1f,
                zRotation = 0f,
            });
            entity.componentRefs[typeof(SpriteComponent)] = b.tag == "Enemy" ? enemyBulletSpriteID : playerBulletSpriteID;
            entity.componentRefs[typeof(HitboxComponent)] = Database.AddComponent(new HitboxComponent()
            {
                offsetX = 0, offsetY = 0, rectColliderWidth = 12, rectColliderHeight = 12, circleColliderRadius = 6, colliderType = HitboxComponent.ColliderType.CircleCollider
            });
            // WHY IS THE COLLIDER NOT MATCHING THE SPRITE IS IT DRAWTEXTURE PRO IM GOING TO GO MAD AAAA

            if (b.tag == "Enemy") entity.componentRefs[typeof(EnemyBulletHitbox)] = Database.AddComponent(new EnemyBulletHitbox());
            else entity.componentRefs[typeof(PlayerBulletHitbox)] = Database.AddComponent(new PlayerBulletHitbox());
            Database.AddEntity(entity);
            //bullets.Add(b);
            //return index;
        }
        public static void AddBullets(Bullet[] bList)
        {
            foreach (var bullet in bList)
            {
                AddBullet(bullet);
                
            }
        }
        public static void UpdateSpawner(ref BulletSpawner spawner)
        {
            if (!spawner.isManual)
            {
                if (spawner.timer <= 0)
                {
                    SpawnBullets(spawner);
                    spawner.timer = spawner.spawnRate;
                }
            }
            else if (spawner.queueSpawn)
            {
                SpawnBullets(spawner);
                spawner.queueSpawn = false;
            }
            spawner.timer -= Globals.timeDelta;

            spawner.offset += spawner.offsetChange * Globals.timeDelta;
        }

        // Select a random rotation from min to max for each bullet
        public static float[] RandomRotations(BulletSpawner spawner)
        {
            var rotations = new float[spawner.numberOfBullets];
            for (int i = 0; i < spawner.numberOfBullets; i++)
            {
                var difference = spawner.maxRotation - spawner.minRotation;
                rotations[i] = ((float)Globals.random.NextDouble() * difference) + spawner.minRotation;
            }
            return rotations;
        }

        // This will set random rotations evenly distributed between the min and max Rotation.
        public static float[] DistributedRotations(BulletSpawner spawner)
        {
            var rotations = new float[spawner.numberOfBullets];
            if (spawner.numberOfBullets == 1)
            {
                rotations = new float[1];
                rotations[0] = spawner.minRotation + spawner.offset;
            }
            else
            {
                rotations = new float[spawner.numberOfBullets];
                for (int i = 0; i < spawner.numberOfBullets; i++)
                {
                    var fraction = (float)i / ((float)spawner.numberOfBullets - 1f);
                    var difference = spawner.maxRotation - spawner.minRotation;
                    var fractionOfDifference = fraction * difference;
                    rotations[i] = fractionOfDifference + spawner.minRotation + spawner.offset; // We add minRotation to undo Difference
                }
            }
            return rotations;
        }
        public static Bullet[] SpawnBullets(BulletSpawner spawner)
        {
            float[] rotations;
            if (spawner.isRandom) rotations = RandomRotations(spawner);
            else rotations = DistributedRotations(spawner);

            // Spawn Bullets
            Bullet[] spawnedBullets = new Bullet[spawner.numberOfBullets];
            for (int i = 0; i < spawner.numberOfBullets; i++)
            {
                spawnedBullets[i] = new Bullet();
                spawnedBullets[i].tag = "Enemy";
                spawnedBullets[i].position = spawner.position;
                spawnedBullets[i].rotation = rotations[i];
                spawnedBullets[i].tag = spawner.bulletTag;
                spawnedBullets[i].speed = spawner.bulletSpeed;
                spawnedBullets[i].timer = spawner.bulletLifetime;
                spawnedBullets[i].rotationChange = spawner.bulletRotationChange;
                spawnedBullets[i].particleCooldown = spawner.particleCooldown;
                spawnedBullets[i].particleTimer = spawner.particleCooldown;
                spawnedBullets[i].emitParticles = spawner.emitParticles;
                spawnedBullets[i].particleLifeTime = spawner.particleLifeTime;
            }
            AddBullets(spawnedBullets);
            return spawnedBullets;
        }

    }
}
