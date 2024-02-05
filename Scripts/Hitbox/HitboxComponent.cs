using System.Collections.Generic;
using System.Collections;
using RhythmGalaxy.ECS;
namespace RhythmGalaxy
{
    public struct PlayerHitbox : Component { public bool queueForPooling { get; set; } }
    public struct PlayerBulletHitbox : Component { public bool queueForPooling { get; set; } }
    public struct EnemyBulletHitbox : Component { public bool queueForPooling { get; set; } }
    public struct EnemyHitbox : Component { public bool queueForPooling { get; set; } }
    public struct HitboxComponent : Component
    {
        public bool queueForPooling { get; set; }
        public int x;
        public int y;
        public int offsetX;
        public int offsetY;
        public int hp = -1;
        public int circleColliderRadius = 0;
        public int rectColliderWidth = 0;
        public int rectColliderHeight = 0;
        public string signalTag;
        public enum ColliderType
        {
            RectCollider,
            CircleCollider
        }
        public ColliderType colliderType;
        public float hpTimer;
        public HitboxComponent(int x, int y, int radius, int hp)
        {
            this.x = x;
            this.y = y;
            this.hp = hp;
            circleColliderRadius = radius;
            colliderType = ColliderType.CircleCollider;
            //signals = new List<IntPtr>();
        }
        public HitboxComponent(int x, int y, int width, int height, int hp)
        {
            this.x = x;
            this.y = y;
            this.hp = hp;
            rectColliderWidth = width;
            rectColliderHeight = height;
            colliderType = ColliderType.RectCollider;
        }
        public HitboxComponent(ColliderType type, int x = 0, int y = 0, int radius = 1, int width = 1, int height = 1, int hp = -1)
        {
            this.x = x;
            this.y = y;
            this.hp = hp;
            rectColliderWidth = width;
            rectColliderHeight = height;
            colliderType = type;
        }
    }
}
