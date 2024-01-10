using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;

namespace HooperSought
{
    public static class ColliderManager
    {
        public static List<Collider> colliders = new List<Collider>();

        /*public static bool Hit(this Raycast cast, Collider collider)
        {
            // step 1: get the distance between the raycast origin and the collider origin
            //step 2: multiply the raycast direction by the distance, that will be the point
            //step 3.Check if the point is inside the rect collider

            float distance = (collider.origin - cast.startPos).Length();
            if (distance <= cast.distance)
            {
                Vector2 hitPoint = cast.direction * distance;
                return collider.IsCollidingWith(hitPoint);
            } else
            {
                return false;
            }
        }*/
        public static bool IsCollidingWith(this Collider collider1, Collider collider2)
        {
            return Raylib.CheckCollisionRecs(collider1.rect, collider2.rect);
        }
        public static bool IsCollidingWith(this Collider collider1, string tag, out Collider hitCollider)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].tag == tag) 
                {
                    if (collider1.IsCollidingWith(colliders[i]))
                    {
                        hitCollider = colliders[i];
                        return true;
                    }
                }
            }
            hitCollider = new Collider();
            return false;
        }
        public static bool IsCollidingWith(this Collider collider1, string tag)
        {
            Collider outCollider;
            return collider1.IsCollidingWith(tag, out outCollider);
        }
        /*
        public static bool Hit(this Linecast cast, Collider collider, out Vector2 point)
        {
            Linecast topWall = new Linecast(collider.topleft, collider.topright);
            Linecast leftWall = new Linecast(collider.topleft, collider.bottomleft);
            Linecast bottomWall = new Linecast(collider.bottomleft, collider.bottomright);
            Linecast rightWall = new Linecast(collider.topright, collider.bottomright);

            Console.WriteLine(leftWall.length);
            Console.WriteLine(topWall.length);
            Console.WriteLine(bottomWall.length);
            Console.WriteLine(rightWall.length);

            topWall.DrawLine(Color.PURPLE);
            bottomWall.DrawLine(Color.GREEN);
            
            point = Vector2.One * 1000000f;
            Vector2 tempPoint;

            // TOP WALL
            bool hitTop = topWall.IsCollidingWith(cast, out tempPoint);
            if (hitTop)
            {
                float newLength = (cast.startPos - tempPoint).Length();
                float oldLength = (cast.startPos - point).Length();
                if (newLength < oldLength) point = tempPoint;
            }

            // RIGHT WALL
            bool hitRight = cast.IsCollidingWith(rightWall, out tempPoint);
            if (hitRight)
            {
                float newLength = (cast.startPos - tempPoint).Length();
                float oldLength = (cast.startPos - point).Length();
                if (newLength < oldLength) point = tempPoint;
            }

            // LEFT WALL
            bool hitLeft = cast.IsCollidingWith(leftWall, out tempPoint);
            if (hitLeft)
            {
                float newLength = (cast.startPos - tempPoint).Length();
                float oldLength = (cast.startPos - point).Length();
                if (newLength < oldLength) point = tempPoint;
            }

            // BOTTOM WALL
            bool hitBottom = bottomWall.IsCollidingWith(cast, out tempPoint);
            if (hitBottom)
            {
                float newLength = (cast.startPos - tempPoint).Length();
                float oldLength = (cast.startPos - point).Length();
                if (newLength < oldLength) point = tempPoint;
            }
            return hitTop || hitRight || hitLeft || hitBottom;
        }
        */
        /*
        public static bool Hit(this Linecast cast, string tag, float maxDistance)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].tag == tag)
                {
                    Vector2 point;
                    if (cast.Hit(colliders[i], out point))
                    {
                        if ((cast.startPos - point).Length() <= maxDistance)
                            return true;
                    }
                }
            }
            return false;
        }
        */
        public static bool IsCollidingWith(this Collider collider, Vector2 point)
        {
            return collider.x <= point.X && collider.x + collider.width >= point.X &&
                collider.y <= point.Y && collider.y + collider.height >= point.Y;
        }
        public static bool IsCollidingWith(this Vector2 point, Collider collider)
        {
            return collider.IsCollidingWith(point);
        }
        public static bool IsColliding(this Vector2 collider)
        {
            foreach (var item in colliders)
            {
                if (item.IsCollidingWith(collider))
                    return true;
            }
            return false;
        }
        public static void Update()
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].tag == "Wall")
                {
                    Raylib.DrawRectangle((int)colliders[i].x, (int)colliders[i].y, (int)colliders[i].width, (int)colliders[i].height, Color.GREEN);
                }
            }
        }
        
    }
}
