using Raylib_cs;
using RhythmGalaxy;
using RhythmGalaxy.ECS;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System;
using static Raylib_cs.Raylib;
public class SampleScene : Scene 
{
    Music music;
    int transformID;
    int spriteID;
    int playerID;
    const float minX = 250;
    const float maxX = 250 + 460 - 40;
    const float minY = 0;
    const float maxY = 540 - 40;
    public void Start()
    {
        // music doesnt work in wasm
        music = LoadMusicStream("Resources/Audio/GoneAndForgotten.mp3");
        
        PlayMusicStream(music);
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
        ClearBackground(Color.BLACK);
        DrawRectangle(250, 0, 460, 540, new Color(34, 20, 31, 255));


        UpdateMusicStream(music);

        var transform = Database.entities[playerID].GetComponent<TransformComponent>();
        float inputX = (int)IsKeyDown(KeyboardKey.KEY_D) - (int)IsKeyDown(KeyboardKey.KEY_A);
        float inputY = (int)IsKeyDown(KeyboardKey.KEY_S) - (int)IsKeyDown(KeyboardKey.KEY_W);
        transform.xPosition += inputX * 4;
        transform.yPosition += inputY * 4;
        if (transform.xPosition < minX) transform.xPosition = minX;
        if (transform.yPosition < minY) transform.yPosition = minY;
        if (transform.xPosition > maxX) transform.xPosition = maxX;
        if (transform.yPosition > maxY) transform.yPosition = maxY;
        Database.entities[playerID].SetComponent(transform);
    }
    public void Stop()
    {

    }
}
