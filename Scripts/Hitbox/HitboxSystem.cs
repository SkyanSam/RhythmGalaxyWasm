using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace RhythmGalaxy
{
    public class HitboxSystem
    {
        // Note that I may want to create a pointer for each Component instead of a Object
        public List<HitboxComponent> playerHitboxes = new List<HitboxComponent>();
        public List<HitboxComponent> playerBulletHitboxes = new List<HitboxComponent>();
        public List<HitboxComponent> enemyHitboxes = new List<HitboxComponent>();
        public List<HitboxComponent> enemyBulletHitboxes = new List<HitboxComponent>();

        public void Update()
        {
            for (int p = 0; p < playerHitboxes.Count; p++)
            {
                var pHitbox = playerHitboxes[p];
                for (int e = 0; e < enemyHitboxes.Count; e++)
                {
                    var eHitbox = enemyHitboxes[e];
                    if (Collide(pHitbox, eHitbox))
                    {
                        // Inflict damage on player if the player collides with enemy
                        pHitbox.hp = pHitbox.hp - 1;
                        foreach (var signal in pHitbox.signals)
                            signal(pHitbox.hp);
                    }
                    enemyHitboxes[e] = eHitbox;
                    /*
                    unsafe
                    {
                        var b = Globals.ConvertStruct(ref eHitbox);

                        unsafe
                        {
                            fixed (HitboxComponent* ptr = &eHitbox)
                            {

                            }
                            
                        }
                    }*/
                }
                for (int e = 0; e < enemyBulletHitboxes.Count; e++)
                {
                    var bHitbox = enemyBulletHitboxes[e];
                    if (Collide(pHitbox, bHitbox))
                    {
                        // Inflict damage on player if the player collides with enemy BULLET
                        pHitbox.hp = pHitbox.hp - 1;
                        foreach (var signal in pHitbox.signals)
                            signal(pHitbox.hp);
                    }
                    enemyHitboxes[e] = bHitbox;
                }
                playerHitboxes[p] = pHitbox;
            }
            for (int e = 0; e < enemyHitboxes.Count; e += 1)
            {
                var eHitbox = enemyHitboxes[e];
                for (int b = 0; b < playerBulletHitboxes.Count; b += 1)
                {
                    var bHitbox = playerBulletHitboxes[b];

                    if (Collide(eHitbox, bHitbox))
                    {
                        // Inflict damage on enemy if enemy collides with player BULLET
                        eHitbox.hp -= 1;
                        foreach (var signal in eHitbox.signals)
                            signal(eHitbox.hp);
                    }
                    playerBulletHitboxes[b] = bHitbox;
                }
                enemyHitboxes[e] = eHitbox;
            }
        }
        // This function should implement any collision: square & circle, circle and circle, and square and square
        bool Collide(HitboxComponent h1, HitboxComponent h2)
        {
            if (h1.colliderType == h2.colliderType)
            {
                if (h1.colliderType == HitboxComponent.ColliderType.CircleCollider)
                {
                    return CheckCollisionCircles(new Vector2(h1.x, h1.y), h1.circleColliderRadius, new Vector2(h2.x, h2.y), h2.circleColliderRadius);
                }
                else // colliderType == BoxCollider
                {
                    return CheckCollisionRecs(MakeRectangle(h1), MakeRectangle(h2));
                }
            }
            else if (h1.colliderType == HitboxComponent.ColliderType.RectCollider && h2.colliderType == HitboxComponent.ColliderType.CircleCollider)
            {
                return CheckCollisionCircleRec(new Vector2(h2.x, h2.y), h2.circleColliderRadius, MakeRectangle(h1));
            }
            else if (h1.colliderType == HitboxComponent.ColliderType.CircleCollider && h2.colliderType == HitboxComponent.ColliderType.RectCollider)
            {
                return CheckCollisionCircleRec(new Vector2(h1.x, h1.y), h1.circleColliderRadius, MakeRectangle(h2));
            }
            return false;
        }
        Rectangle MakeRectangle(HitboxComponent h)
        {
            return new Rectangle(h.x, h.y, h.rectColliderWidth, h.rectColliderHeight);
        }
    }
}
