using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using RhythmGalaxy.ECS;

namespace RhythmGalaxy
{
    public class HitboxSystem : BaseSystem
    {
        public override void Initialize()
        {
            updateFormat = UpdateFormat.UpdateNRequestsNComponentSets;
            typesetList = new List<Type[]> () {
                new Type[2] { typeof(PlayerHitbox), typeof(HitboxComponent) },
                new Type[2] { typeof(EnemyHitbox), typeof(HitboxComponent) },
                new Type[2] { typeof(PlayerBulletHitbox), typeof(HitboxComponent) },
                new Type[2] { typeof(EnemyBulletHitbox), typeof(HitboxComponent) }
            };
        }
        public override void UpdateNComponentSetsNRequests(List<Dictionary<Type, List<int>>> componentSetsList)
        {
            var playerHitboxes = GetHitboxes(0, componentSetsList);
            var enemyHitboxes = GetHitboxes(1, componentSetsList);
            var playerBulletHitboxes = GetHitboxes(2, componentSetsList);
            var enemyBulletHitboxes = GetHitboxes(3, componentSetsList);

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
        public List<HitboxComponent> GetHitboxes(int i, List<Dictionary<Type, List<int>>> componentSetsList)
        {
            List<int> hitboxesIndices = componentSetsList[i][typeof(HitboxComponent)];
            List<HitboxComponent> hitboxes = new List<HitboxComponent>();
            foreach (var j in hitboxesIndices)
                hitboxes.Add((HitboxComponent)Database.components[typeof(HitboxComponent)][j]);
            return hitboxes;
        }
        public void SetHitboxes(int i, List<Dictionary<Type, List<int>>> componentSetsList, List<HitboxComponent> hitboxes)
        {
            List<int> hitboxesIndices = componentSetsList[i][typeof(HitboxComponent)];
            for (int j = 0; j < hitboxesIndices.Count; j++)
            {
                Database.components[typeof(HitboxComponent)][hitboxesIndices[j]] = hitboxes[j];
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
