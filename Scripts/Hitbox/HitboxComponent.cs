using System.Collections.Generic;
using System.Collections;
namespace RhythmGalaxy
{
    public struct HitboxComponent
    {
        public int x;
        public int y;
        public int hp = -1;
        public int circleColliderRadius = 0;
        public int rectColliderWidth = 0;
        public int rectColliderHeight = 0;
        public enum ColliderType
        {
            RectCollider,
            CircleCollider
        }
        public ColliderType colliderType;
        public delegate void signal(int hp);
        public List<signal> signals = new List<signal>();
        public HitboxComponent(int x, int y, int radius, int hp)
        {
            this.x = x;
            this.y = y;
            this.hp = hp;
            circleColliderRadius = radius;
            colliderType = ColliderType.CircleCollider;
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
