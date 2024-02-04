using MonoGame.Extended.Tweening;
using Raylib_cs;
using static Raylib_cs.Raylib;
using RhythmGalaxy.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Melanchall.DryWetMidi.MusicTheory;
using RhythmGalaxy.Scenes;

namespace RhythmGalaxy
{
    public class Player
    {
        public enum BulletMode { 
            OneBullet,
            ThreeBullet,
            Minigun,
            Laser
        }
        BulletMode bulletMode = BulletMode.OneBullet;
        public enum Mode { 
            Attack,
            Defend
        }
        public Mode mode = Mode.Attack;
        int transformID;
        int spriteID;
        int playerID;
        const float minX = 250;
        const float maxX = 250 + 460 - 40;
        const float minY = 0;
        const float maxY = 540 - 40;
        public float attackSpeed = 200;
        public float defenseSpeed = 100;
        public float energyDepletionPerSec = 1f / 15f;
        public float energyRegainPerSec = 1f / 12f;
        public float hp = 1f;
        public const float hpCooldown = 0.5f;
        float hpTimer = 0;
        public float energy = 1f;
        public const float minigunCooldown = 0.1f;
        public float minigunTimer = 0;
        Random random;
        public void Start()
        {
            hp = 1f;
            energy = 1f;
            random = new Random();
            transformID = Database.AddComponent(new TransformComponent()
            {
                xPosition = minX,
                yPosition = maxY,
                xScale = 1,
                yScale = 1,
            });
            spriteID = Database.AddComponent(new SpriteComponent()
            {
                texture2D = LoadTexture("Resources/Sprites/PlayerShip.png"),
                scaleX = 2,
                scaleY = 2,
            });
            Entity player = new Entity();
            player.queueForPooling = false;
            player.componentRefs = new Dictionary<Type, int>();
            player.componentRefs[typeof(TransformComponent)] = transformID;
            player.componentRefs[typeof(SpriteComponent)] = spriteID;
            player.componentRefs[typeof(PlayerHitbox)] = Database.AddComponent(new PlayerHitbox());
            player.componentRefs[typeof(HitboxComponent)] = Database.AddComponent(new HitboxComponent()
            {
                colliderType = HitboxComponent.ColliderType.CircleCollider,
                circleColliderRadius = 13,
                signals = new List<HitboxComponent.Signal>() { (int hp) => {
                    if (hpTimer <= 0)
                    {
                        if (energy > 0.5f) this.hp -= 0.1f;
                        else this.hp -= 0.2f;
                        hpTimer = hpCooldown;
                        if (this.hp <= 0)
                        {
                            QueueGameOver();
                        }
                    }
                }},
            });
            playerID = Database.AddEntity(player);

            SongManager.Instance.signals.Add((int step) =>
            {
                if (GameUI.conveyorMode == GameUI.ConveyorMode.Tap)
                {
                    if (SongManager.IsStartNoteAtStep(step, NoteName.C))
                    {
                        if (bulletMode == BulletMode.OneBullet)
                        {
                            var transform = player.GetComponent<TransformComponent>();
                            BulletManager.AddBullet(new Bullet()
                            {
                                tag = "Player",
                                position = new Vector2(transform.xPosition, transform.yPosition),
                                rotation = 90,
                                speed = 727 / 2,
                                timer = 20,
                            });
                        }
                        else if (bulletMode == BulletMode.ThreeBullet)
                        {
                            var transform = player.GetComponent<TransformComponent>();

                            for (int i = 0; i < 3; i++)
                            {
                                BulletManager.AddBullet(new Bullet()
                                {
                                    tag = "Player",
                                    position = new Vector2(transform.xPosition, transform.yPosition),
                                    rotation = (30 * i) + 60,
                                    speed = 727,
                                    timer = 20,
                                });
                            }
                        }
                    }
                }
                else if (GameUI.conveyorMode == GameUI.ConveyorMode.Hold) {
                    if (SongManager.IsMidNoteAtStep(step, NoteName.D) || SongManager.IsStartNoteAtStep(step, NoteName.D) || SongManager.IsEndNoteAtStep(step, NoteName.D))
                    {
                        if (bulletMode == BulletMode.Minigun && minigunTimer <= 0)
                        {
                            var transform = player.GetComponent<TransformComponent>();
                            BulletManager.AddBullet(new Bullet()
                            {
                                tag = "Player",
                                position = new Vector2(transform.xPosition + (float)(random.NextDouble() * 4f), transform.yPosition + (float)(random.NextDouble() * 4f)),
                                rotation = 90,
                                speed = 727,
                                timer = 20,
                            });
                            minigunTimer = minigunCooldown;
                        }
                        else if (bulletMode == BulletMode.Laser)
                        {
                            // might want to just draw laser here, and update hitboxes..
                        }
                    }
                }
            });
        }
        public void Update()
        {
            mode = IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) ? Mode.Defend : Mode.Attack;

            // this line isnt working correctly
            energy += (mode == Mode.Defend ? energyDepletionPerSec : -energyRegainPerSec) * Globals.timeDelta;
            var transform = Database.entities[playerID].GetComponent<TransformComponent>();
            float inputX = (int)IsKeyDown(KeyboardKey.KEY_D) - (int)IsKeyDown(KeyboardKey.KEY_A);
            float inputY = (int)IsKeyDown(KeyboardKey.KEY_S) - (int)IsKeyDown(KeyboardKey.KEY_W);
            Vector2 input = new Vector2(inputX, inputY);
            input.Normalize();
            transform.xPosition += input.X * (mode == Mode.Attack? attackSpeed : defenseSpeed) * Globals.timeDelta;
            transform.yPosition += input.Y * (mode == Mode.Attack? attackSpeed : defenseSpeed) * Globals.timeDelta;
            if (transform.xPosition < minX) transform.xPosition = minX;
            if (transform.yPosition < minY) transform.yPosition = minY;
            if (transform.xPosition > maxX) transform.xPosition = maxX;
            if (transform.yPosition > maxY) transform.yPosition = maxY;
            Database.entities[playerID].SetComponent(transform);

            GameUI.healthPercent = hp;
            GameUI.energyPercent = energy;

            hpTimer -= Globals.timeDelta;
            minigunTimer -= Globals.timeDelta;
        }
        public void QueueGameOver()
        {
            Console.WriteLine("Game Over!");
            GameOverScene.lastScene = Globals.currentScene;
            Application.SwitchScene("GameOverScene");
        }
        
    }
}
