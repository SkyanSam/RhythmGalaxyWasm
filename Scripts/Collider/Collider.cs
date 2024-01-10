using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

public struct Collider
{
    public float x;
    public float y;
    public float width;
    public float height;
    public string tag;
    public Rectangle rect
    {
        get
        {
            return new Rectangle(x, y, width, height);
        }
    }
    public Vector2 origin
    {
        get {
            return new Vector2(x + (width / 2), y + (height / 2));
        }
    }
    public Vector2 topleft
    {
        get
        {
            return new Vector2(x, y);
        }
    }
    public Vector2 topright
    {
        get
        {
            return new Vector2(x + width, y);
        }
    }
    public Vector2 bottomright
    {
        get
        {
            return new Vector2(x + width, y + height);
        }
    }
    public Vector2 bottomleft
    {
        get
        {
            return new Vector2(x, y + height);
        }
    }
    public Collider(Vector2 origin, float radius, string _tag)
    {
        x = origin.X - radius;
        y = origin.Y - radius;
        width = radius * 2;
        height = radius * 2;
        tag = _tag;
    }
    public Collider(float _x, float _y, float _width, float _height, string _tag)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        tag = _tag;
    }
    public Collider(Vector2 _pos, float _width, float _height, string _tag)
    {
        x = _pos.X;
        y = _pos.Y;
        width = _width;
        height = _height;
        tag = _tag;
    }
}
public struct Raycast
{
    public Vector2 startPos;
    public Vector2 direction;
    public float distance;
    public float range;
}
public struct Linecast
{
    public Vector2 startPos;
    public Vector2 endPos;
    public float range;
    public Linecast(Vector2 start, Vector2 end)
    {
        startPos = start;
        endPos = end;
        range = 1;
    }
    public float length
    {
        get
        {
            return (startPos - endPos).Length();
        }
    }
}
public struct Point
{
    public Vector2 position;
    public float range;
}
