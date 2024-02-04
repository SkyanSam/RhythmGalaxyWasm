using RhythmGalaxy.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace RhythmGalaxy.Scripts
{
    class ShipCreator
    {
        public static Entity CreateShip1(Vector2 position)
        {
            Entity ship = new Entity();
            ship.componentRefs = new Dictionary<Type, int>();
            ship.componentRefs[typeof(SpriteComponent)] = Database.AddComponent(new SpriteComponent()
            {
                texture2D = LoadTexture("Resources/Sprites/Ships.png"),
                scaleX = 2,
                scaleY = 2,
                useSource = true,
                source = new Rectangle(0, 0, 20, 20),
            });
            ship.componentRefs[typeof(TransformComponent)] = Database.AddComponent(new TransformComponent()
            {
                xPosition = position.X,
                yPosition = position.Y,
                xScale = 1,
                yScale = 1,
                zRotation = 180,
            });
            ship.componentRefs[typeof(HitboxComponent)] = Database.AddComponent(new HitboxComponent()
            {
                offsetX = -20,
                offsetY = -20,
                rectColliderHeight = 40,
                rectColliderWidth = 40,
                hp = 10,
                signals = new List<HitboxComponent.Signal>() { (hp) => HurtEnemy(hp, ship) }
            });
            ship.componentRefs[typeof(EnemyHitbox)] = Database.AddComponent(new EnemyHitbox());
            ship.componentRefs[typeof(BulletSpawner)] = Database.AddComponent(new BulletSpawner()
            {
                minRotation = -90,
                maxRotation = -90,
                numberOfBullets = 1,
                bulletLifetime = 10,
                bulletSpeed = 300,
                isManual = true,
                bulletTag = "Enemy"
            });
            return ship;
        }
        public static Entity CreateShip2(Vector2 position)
        {
            Console.WriteLine("!!! creating ship 2 !!!");
            Entity ship = new Entity();
            ship.componentRefs = new Dictionary<Type, int>();
            ship.componentRefs[typeof(SpriteComponent)] = Database.AddComponent(new SpriteComponent()
            {
                texture2D = LoadTexture("Resources/Sprites/Ships.png"),
                scaleX = 2,
                scaleY = 2,
                useSource = true,
                source = new Rectangle(20, 0, 20, 20),
            });
            ship.componentRefs[typeof(TransformComponent)] = Database.AddComponent(new TransformComponent()
            {
                xPosition = position.X,
                yPosition = position.Y,
                xScale = 1,
                yScale = 1,
                zRotation = 180,
            });
            ship.componentRefs[typeof(HitboxComponent)] = Database.AddComponent(new HitboxComponent()
            {
                offsetX = -20,
                offsetY = -20,
                rectColliderHeight = 40,
                rectColliderWidth = 40,
                hp = 10,
                signals = new List<HitboxComponent.Signal>() { (hp) => HurtEnemy(hp, ship) }
            });

            ship.componentRefs[typeof(EnemyHitbox)] = Database.AddComponent(new EnemyHitbox());
            ship.componentRefs[typeof(BulletSpawner)] = Database.AddComponent(new BulletSpawner()
            {
                minRotation = -120,
                maxRotation = -60,
                numberOfBullets = 2,
                bulletLifetime = 10,
                bulletSpeed = 300,
                bulletTag = "Enemy",
                isManual = true,


                // debug
                //spawnRate = 0.5f,
                //isManual = false
            });
            return ship;
        }
        public static Entity CreateShip3(Vector2 position)
        {
            Entity ship = new Entity();
            ship.componentRefs = new Dictionary<Type, int>();
            ship.componentRefs[typeof(SpriteComponent)] = Database.AddComponent(new SpriteComponent()
            {
                texture2D = LoadTexture("Resources/Sprites/Ships.png"),
                scaleX = 2,
                scaleY = 2,
                useSource = true,
                source = new Rectangle(40, 0, 20, 20),
            });
            ship.componentRefs[typeof(TransformComponent)] = Database.AddComponent(new TransformComponent()
            {
                xPosition = position.X,
                yPosition = position.Y,
                xScale = 1,
                yScale = 1,
                zRotation = 180,
            });
            ship.componentRefs[typeof(HitboxComponent)] = Database.AddComponent(new HitboxComponent()
            {
                offsetX = -20,
                offsetY = -20,
                rectColliderHeight = 40,
                rectColliderWidth = 40,
                hp = 10,
                signals = new List<HitboxComponent.Signal>() { (hp) => HurtEnemy(hp, ship) }
            });

            ship.componentRefs[typeof(EnemyHitbox)] = Database.AddComponent(new EnemyHitbox());
            ship.componentRefs[typeof(BulletSpawner)] = Database.AddComponent(new BulletSpawner()
            {
                minRotation = -150,
                maxRotation = -30,
                numberOfBullets = 3,
                bulletLifetime = 10,
                bulletSpeed = 300,
                isManual = true,
                bulletTag = "Enemy"
            });
            return ship;
        }
        public static void HurtEnemy(int hp, Entity ship)
        {
            var index = Database.FindEntity(ship.componentRefs.Keys.ToArray(), ship.componentRefs.Values.ToArray());
            if (hp <= 0) Database.RemoveEntity(index);
            Console.WriteLine($"Removing ship at index {index}");
        }
    }
}
