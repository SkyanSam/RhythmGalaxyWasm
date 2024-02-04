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
                new Type[3] { typeof(PlayerHitbox), typeof(HitboxComponent), typeof(TransformComponent) },
                new Type[3] { typeof(EnemyHitbox), typeof(HitboxComponent), typeof(TransformComponent) },
                new Type[3] { typeof(PlayerBulletHitbox), typeof(HitboxComponent), typeof(TransformComponent) },
                new Type[3] { typeof(EnemyBulletHitbox), typeof(HitboxComponent), typeof(TransformComponent) }
            };
        }
        public override void UpdateNComponentSetsNRequests(List<Dictionary<Type, List<int>>> componentSetsList)
        {
            var playerHitboxes = GetComponents<HitboxComponent>(0, componentSetsList);
            var playerTransforms = GetComponents<TransformComponent>(0, componentSetsList);
            UpdateHitboxTransform(ref playerHitboxes, playerTransforms);

            var enemyHitboxes = GetComponents<HitboxComponent>(1, componentSetsList);
            var enemyTransforms = GetComponents<TransformComponent>(1, componentSetsList);
            UpdateHitboxTransform(ref enemyHitboxes, enemyTransforms);

            var playerBulletHitboxes = GetComponents<HitboxComponent>(2, componentSetsList);
            var playerBulletTransforms = GetComponents<TransformComponent>(2, componentSetsList);
            UpdateHitboxTransform(ref playerBulletHitboxes, playerBulletTransforms);

            var enemyBulletHitboxes = GetComponents<HitboxComponent>(3, componentSetsList);
            var enemyBulletTransforms = GetComponents<TransformComponent>(3, componentSetsList);
            UpdateHitboxTransform(ref enemyBulletHitboxes, enemyBulletTransforms);

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
                        if (pHitbox.signals != null)
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

                        if (pHitbox.signals != null)
                            foreach (var signal in pHitbox.signals)
                                signal(pHitbox.hp);
                    }
                    bHitbox.hpTimer -= Globals.timeDelta;
                    enemyBulletHitboxes[e] = bHitbox;
                }
                playerHitboxes[p] = pHitbox;
            }
            for (int e = 0; e < enemyHitboxes.Count; e += 1)
            {
                var eHitbox = enemyHitboxes[e];
                eHitbox.hpTimer -= Globals.timeDelta;
                for (int b = 0; b < playerBulletHitboxes.Count; b += 1)
                {
                    var bHitbox = playerBulletHitboxes[b];

                    if (Collide(eHitbox, bHitbox) && eHitbox.hpTimer <= 0)
                    {
                        eHitbox.hpTimer = 0.5f;
                        // Inflict damage on enemy if enemy collides with player BULLET
                        if (eHitbox.hpTimer <= 0) eHitbox.hp -= 1;
                        if (eHitbox.signals != null)
                            foreach (var signal in eHitbox.signals)
                                signal(eHitbox.hp);
                    }
                    playerBulletHitboxes[b] = bHitbox;
                }
                enemyHitboxes[e] = eHitbox;
            }
            
            SetComponents(0, componentSetsList, playerHitboxes);
            SetComponents(1, componentSetsList, enemyHitboxes);
            SetComponents(2, componentSetsList, playerBulletHitboxes);
            SetComponents(3, componentSetsList, enemyBulletHitboxes);

            // debug
            DrawHitboxes(playerHitboxes);
            DrawHitboxes(enemyHitboxes);
            DrawHitboxes(playerBulletHitboxes);
            DrawHitboxes(enemyBulletHitboxes);
            //
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
        public static void UpdateHitboxTransform(ref List<HitboxComponent> hitboxes, List<TransformComponent> transforms)
        {
            for (int i = 0; i < hitboxes.Count; i++)
            {
                var hbox = hitboxes[i];
                hbox.x = (int)transforms[i].xPosition + hitboxes[i].offsetX;
                hbox.y = (int)transforms[i].yPosition + hitboxes[i].offsetY;
                hitboxes[i] = hbox;
            }
        }
        public static void DrawHitboxes(List<HitboxComponent> hitboxes)
        {
            foreach (var component in hitboxes)
            {
                if (component.colliderType == HitboxComponent.ColliderType.RectCollider)
                    DrawRectangleLines(component.x, component.y, component.rectColliderWidth, component.rectColliderHeight, Color.GREEN);
                if (component.colliderType == HitboxComponent.ColliderType.CircleCollider)
                    DrawCircleLines(component.x, component.y, component.circleColliderRadius, Color.GREEN);
            }
        }
    }
}
