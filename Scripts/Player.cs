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

namespace RhythmGalaxy
{
    class Player
    {
        public enum Mode { 
            Attack,
            Defend
        }
        public Mode mode;
        int transformID;
        int spriteID;
        int playerID;
        const float minX = 250;
        const float maxX = 250 + 460 - 40;
        const float minY = 0;
        const float maxY = 540 - 40;
        public float attackSpeed = 40;
        public float defenseSpeed = 20;
        public float energyDepletionPerSec = 0.04f;
        public float energyRegainPerSec = 0.02f;
        public float hp = 0;
        public float energy = 0;
        public void Start()
        {
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
            playerID = Database.AddEntity(player);
        }
        public void Update()
        {
            mode = IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) ? Mode.Defend : Mode.Attack;
            energy += (mode == Mode.Defend ? -energyDepletionPerSec : energyRegainPerSec) * Globals.timeDelta;
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
        }
        
    }
}
