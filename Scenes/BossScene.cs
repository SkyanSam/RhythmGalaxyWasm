using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using RhythmGalaxy.ECS;
using static Raylib_cs.Raylib;
namespace RhythmGalaxy
{
    class BossScene : Scene
    {
        static SpriteSystem spriteSystem;
        static HitboxSystem hitboxSystem;
        static BulletManager bulletManager;
        static Player player;
        static Entity boss;
        static Texture2D galaxi;
        static int bossIndex;
        public void Start()
        {
            Database.Reset();
            galaxi = Raylib.LoadTexture("Resources/Sprites/galaxi.png");
            // ECS
            spriteSystem = new SpriteSystem();
            spriteSystem.Initialize();
            hitboxSystem = new HitboxSystem();
            hitboxSystem.Initialize();
            bulletManager = new BulletManager();
            bulletManager.Initialize();
            //

            SongManager.Instance = new SongManager();
            SongManager.Instance.Start("Dust_to_dust.mp3", "GoneAndForgotten.mid", 167 * 2);
            GameUI.songName = "Dust To Dust";
            GameUI.artistName = "Apechs.mp3";
            GameUI.Init();

            player = new Player();
            player.Start();

            /*SongManager.Instance.signals.Add((int step) =>
            {
                
            });*/
            // constructing le boss
            Entity entity = new Entity();
            entity.componentRefs = new Dictionary<Type, int>();
            entity.componentRefs[typeof(TransformComponent)] = Database.AddComponent(new TransformComponent()
            {
                yScale = 1f,
                xScale = 1f,
                zRotation = 0f,
                xPosition = 360,
                yPosition = 70
            });
            entity.componentRefs[typeof(SpriteComponent)] = Database.AddComponent(new SpriteComponent()
            {
                texture2D = LoadTexture("Resources/Sprites/boss.png"),
                source = new Rectangle(0,0,20*6,20*5),
                useSource = true,
                scaleX = 2,
                scaleY = 2
            });
            entity.componentRefs[typeof(HitboxComponent)] = Database.AddComponent(new HitboxComponent() { 
                offsetX = -40*3, offsetY = (int)(-40*2.5f), hp = 80, colliderType = HitboxComponent.ColliderType.RectCollider,
                rectColliderHeight = 40 * 5, rectColliderWidth = 40 * 6, signalTag = "Boss"
            });
            entity.componentRefs[typeof(EnemyHitbox)] = Database.AddComponent(new EnemyHitbox());
            entity.componentRefs[typeof(BulletSpawner)] = Database.AddComponent(new BulletSpawner()
            {
                    minRotation = 0,
                    maxRotation = 360,
                    numberOfBullets = 22,
                    bulletLifetime = 1f,
                    bulletSpeed = 300,
                    isManual = true,
                    bulletTag = "Enemy",
                    bulletRotationChange = 30,
            });
            boss = entity;
            bossIndex = Database.AddEntity(entity);

            // end construction of le boss >:3
        }
        public void Update()
        {
            ClearBackground(Color.BLACK);
            DrawRectangle(250, 0, 460, 540, new Color(34, 20, 31, 255));
            DrawTexturePro(galaxi, new Rectangle(0, 0, galaxi.width, galaxi.height), new Rectangle(250, 0, 460, 540), Vector2.Zero, 0, Color.WHITE);
            SongManager.Instance.Update();

            var bossTransform = boss.GetComponent<TransformComponent>();
            //float moveTime = 9f;
            //float t = (float)GetTime() % moveTime;
            //if (t >= moveTime / 2f) t -= moveTime;
            //t /= moveTime / 2f;
            //t = MathF.Pow(t, 2);
            bossTransform.xPosition = 480;
            bossTransform.yPosition = 200 + (50 * MathF.Sin((float)GetTime() * (MathF.PI / 5f)));
            boss.SetComponent(bossTransform);
            Database.entities[bossIndex] = boss;

            var bossSpawner = boss.GetComponent<BulletSpawner>();
            bossSpawner.minRotation = (float)GetTime() * 30f;
            bossSpawner.maxRotation = ((float)GetTime() * 30f) + 360f;
            boss.SetComponent(bossSpawner);

            // ECS
            spriteSystem.UpdateAll();
            bulletManager.UpdateAll();
            hitboxSystem.UpdateAll();
            //

            player.Update();
            GameUI.Draw();
        }
        public static void HurtBoss(int hp)
        {
            if (hp == 0)
            {
                // YOU WIN THE BOSS YIPPEEE
                Application.SwitchScene(nameof(LogExcerpt3));
            }
        }
        public static void SongManagerStep(int step)
        {
            if (SongManager.IsStartNoteAtStep(step, Melanchall.DryWetMidi.MusicTheory.NoteName.C))
            {
                BulletSpawner bs = boss.GetComponent<BulletSpawner>();
                bs.bulletLifetime = 5;
                //bs.queueSpawn = true;
                BulletManager.SpawnBullets(bs);
                //boss.SetComponent(bs);
            }
        }
        public void Stop()
        {

        }
    }
}
