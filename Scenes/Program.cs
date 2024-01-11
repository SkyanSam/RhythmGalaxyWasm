using Raylib_cs;
using CSScriptLib;
using System;
/*using RhythmGalaxy;
using RhythmGalaxy.ECS;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json.Linq;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// RAYLIB
const int screenWidth = 1920;
const int screenHeight = 1080;
Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
Raylib.InitWindow(screenWidth, screenHeight, "Rhythm Galaxy");
Raylib.InitAudioDevice();
//if (!Raylib.IsWindowFullscreen()) Raylib.ToggleFullscreen();
Raylib.SetTargetFPS(60);
RenderTexture2D renderTarget = Raylib.LoadRenderTexture(screenWidth, screenHeight);

// ECS
SpriteSystem spriteSystem = new SpriteSystem();
spriteSystem.Initialize();

Entity entity = new Entity();
Entity entity2 = new Entity();
Entity entity3 = new Entity();


entity.componentRefs = new Dictionary<Type, int>();
entity2.componentRefs = new Dictionary<Type, int>();
entity3.componentRefs = new Dictionary<Type, int>();
var tIndex = Database.AddComponent(new TransformComponent()
{
    xScale = 40,
    yScale = 40,
    zRotation = 0,
    xPosition = 100,
    yPosition = 100
});
var tIndex2 = Database.AddComponent(new TransformComponent()
{
    xScale = 40,
    yScale = 40,
    zRotation = 0,
    xPosition = 200,
    yPosition = 500
});
var tIndex3 = Database.AddComponent(new TransformComponent()
{
    xScale = 40,
    yScale = 40,
    zRotation = 0,
    xPosition = 900,
    yPosition = 10
});

var sIndex = Database.AddComponent(new SpriteComponent()
{
    texture2D = Raylib.LoadTexture("Resources/greenflag.png"),
    scaleX = 1,
    scaleY = 1
});

entity.componentRefs[typeof(TransformComponent)] = tIndex;
entity.componentRefs[typeof(SpriteComponent)] = sIndex;
entity2.componentRefs[typeof(TransformComponent)] = tIndex2;
entity2.componentRefs[typeof(SpriteComponent)] = sIndex;
entity3.componentRefs[typeof(TransformComponent)] = tIndex3;
entity3.componentRefs[typeof(SpriteComponent)] = sIndex;
Database.AddEntity(entity);
Database.AddEntity(entity2);
Database.AddEntity(entity3);

Font font;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.BeginTextureMode(renderTarget);
    spriteSystem.UpdateAll();
    Raylib.EndTextureMode();
    Raylib.DrawTextureRec(renderTarget.texture, new Rectangle(0, 0, renderTarget.texture.width, -renderTarget.texture.height), new Vector2(0, 0), Color.WHITE);
    Raylib.EndDrawing();
    var tc = ((TransformComponent)Database.components[typeof(TransformComponent)][tIndex]);
    tc.xPosition += 1;
    Database.components[typeof(TransformComponent)][tIndex] = tc;
}
Raylib.CloseAudioDevice();
Raylib.CloseWindow();
*/


