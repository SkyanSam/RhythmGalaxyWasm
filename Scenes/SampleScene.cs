using Raylib_cs;
using RhythmGalaxy;
using RhythmGalaxy.ECS;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System;
using static Raylib_cs.Raylib;
//using TweenSharp;
//using TweenSharp.Factory;
//using TweenSharp.Animation;
using System.Linq.Expressions;
using RhythmGalaxy.Scripts;
using System.Xml;
public class SampleScene : Scene 
{
    public static SampleScene Instance;
    public Player player;
    const float minX = 250;
    const float maxX = 250 + 460 - 40;
    const float minY = 0;
    const float maxY = 540 - 40;

    public int[] shipIndices = new int[6];
    public bool[] shipsQueued = new bool[6];

    static SpriteSystem spriteSystem;
    static HitboxSystem hitboxSystem;
    static BulletManager bulletManager;

    static Texture2D galaxi;
    public void Start()
    {
        Instance = this;
        shipIndices = new int[6];
        shipsQueued = new bool[6];
        // ECS
        spriteSystem = new SpriteSystem();
        spriteSystem.Initialize();
        hitboxSystem = new HitboxSystem();
        hitboxSystem.Initialize();
        bulletManager = new BulletManager();
        bulletManager.Initialize();
        //
        galaxi = LoadTexture("Resources/Sprites/galaxi.png");

        SongManager.Instance = new SongManager();
        SongManager.Instance.Start("GoneAndForgotten.mp3", "GoneAndForgotten.mid", 167 * 2);
        GameUI.songName = "Gone and Forgotten";
        GameUI.artistName = "Apechs.mp3";
        GameUI.Init();

        player = new Player();
        player.Start();

        shipIndices[0] = Database.AddEntity(ShipCreator.CreateShip1(new Vector2(320, 130), "Ship0"));
        shipIndices[1] = Database.AddEntity(ShipCreator.CreateShip2(new Vector2(460, 70), "Ship1"));
        shipIndices[2] = Database.AddEntity(ShipCreator.CreateShip3(new Vector2(600, 130), "Ship2"));
        shipIndices[3] = Database.AddEntity(ShipCreator.CreateShip1(new Vector2(320, 130), "Ship3"));
        shipIndices[4] = Database.AddEntity(ShipCreator.CreateShip2(new Vector2(460, 70), "Ship4"));
        shipIndices[5] = Database.AddEntity(ShipCreator.CreateShip3(new Vector2(600, 130), "Ship5"));

        
    }
    public void SongManagerStep(int step)
    {
        if (step % 4 == 0)
        {
            var spawner = Database.entities[shipIndices[1]].GetComponent<BulletSpawner>();
            spawner.queueSpawn = true;
            Database.entities[shipIndices[1]].SetComponent(spawner);

            spawner = Database.entities[shipIndices[4]].GetComponent<BulletSpawner>();
            spawner.queueSpawn = true;
            Database.entities[shipIndices[4]].SetComponent(spawner);
        }
        if (step % 4 == 2)
        {
            var spawner = Database.entities[shipIndices[2]].GetComponent<BulletSpawner>();
            spawner.queueSpawn = true;
            Database.entities[shipIndices[2]].SetComponent(spawner);

            spawner = Database.entities[shipIndices[5]].GetComponent<BulletSpawner>();
            spawner.queueSpawn = true;
            Database.entities[shipIndices[5]].SetComponent(spawner);
        }
        if (step % 8 == 0)
        {
            var spawner = Database.entities[shipIndices[0]].GetComponent<BulletSpawner>();
            spawner.queueSpawn = true;
            Database.entities[shipIndices[0]].SetComponent(spawner);

            spawner = Database.entities[shipIndices[3]].GetComponent<BulletSpawner>();
            spawner.queueSpawn = true;
            Database.entities[shipIndices[3]].SetComponent(spawner);
        }
    }
    public void Update()
    {

        ClearBackground(Color.BLACK);
        DrawRectangle(250, 0, 460, 540, new Color(34, 20, 31, 255));

        DrawTexturePro(galaxi, new Rectangle(0, 0, galaxi.width, galaxi.height), new Rectangle(250, 0, 460, 540), Vector2.Zero, 0, Color.WHITE);


        SongManager.Instance.Update();

        var t = 0f;
        var time = (float)(GetTime() % 40);
        //Console.WriteLine($"sample scene time {time}");
        if (0 <= time && time <= 2.5f) t = time / 2.5f;
        else if (2.5f <= time && time <= 20) t = 1;
        else if (20 <= time && time <= 22.5f) t = 1f - ((time - 20f) / 2.5f);
        else t = 0;

        for (int i = 0; i < shipIndices.Length; i++)
        {
            if (!shipsQueued[i])
            {
                
                var transform = Database.GetComponent<TransformComponent>(Database.entities[shipIndices[i]]);
                var circPosition = CircleFormation(i * (360f / 6f));
                var rectPosition = RectFormation(i == 0 ? new Vector2(-1, -1) : i == 1 ? new Vector2(0, -1) : i == 2 ? new Vector2(1, -1) : i == 5 ? new Vector2(-1, 1) : i == 4 ? new Vector2(0, 1) : i == 3 ? new Vector2(1, 1) : Vector2.Zero);
                var position = Vector2.Lerp(circPosition, rectPosition, t > 1f? 1f : t < 0f? 0: t);
                //if (i == 0) Console.WriteLine($"pos {position.X} {position.Y}");
                transform.xPosition = position.X;
                transform.yPosition = position.Y;
                Database.SetComponent(Database.entities[shipIndices[i]], transform);
            }
        }
        

        // ECS
        spriteSystem.UpdateAll();
        bulletManager.UpdateAll();
        hitboxSystem.UpdateAll();
        //

        player.Update();
        GameUI.Draw();

        if (DoShipsExist())
        {
            Application.SwitchScene(nameof(LogExcerpt2));
        }
    }
    public Vector2 CircleFormation(float rotation)
    {
        float rot = rotation + (float)(GetTime() * 90f);
        rot *= (MathF.PI / 180);
        return new Vector2(480, 148) + (146 * new Vector2(MathF.Cos(rot), MathF.Sin(rot)));
    }
    public Vector2 RectFormation(Vector2 xy)
    {
        float x = xy.X;
        float y = xy.Y;
        
        float lineSec = 2f;
        float time = (float)GetTime() % (lineSec * 4);

        Vector2 topLeft = new Vector2(350, 70);
        Vector2 topRight = new Vector2(350 + 250, 70);
        Vector2 bottomRight = new Vector2(350 + 250, 70 + 150);
        Vector2 bottomLeft = new Vector2(350, 70 + 150);
        Vector2 a = Vector2.Zero, b = Vector2.Zero;
        float t = 0;
        if (0 <= time && time <= lineSec)
        {
            a = topLeft; b = topRight;
            t = time / lineSec;
        }
        else if (lineSec <= time && time <= lineSec * 2)
        {
            a = topRight; b = bottomRight;
            t = (time - lineSec) / lineSec;
        }
        else if (lineSec * 2 <= time && time <= lineSec * 3)
        {
            a = bottomRight; b = bottomLeft;
            t = (time - lineSec - lineSec) / lineSec;
        }
        else if (lineSec * 3 <= time && time <= lineSec * 4)
        {
            a = bottomLeft; b = topLeft;
            t = (time - lineSec - lineSec - lineSec) / lineSec;
        }
        return Vector2.Lerp(a, b, t > 1f? 1: t < 0f? 0: t) + new Vector2(x * 50, y * 25);
    }
    public static void HurtEnemy(int hp, string tag)
    {
        Entity entity = new Entity();
        int index = 0;
        if (tag == "Ship0") index = 0;
        if (tag == "Ship1") index = 1;
        if (tag == "Ship2") index = 2;
        if (tag == "Ship3") index = 3;
        if (tag == "Ship4") index = 4;
        if (tag == "Ship5") index = 5;
        entity = Database.entities[Instance.shipIndices[index]];
        
        if (hp <= 0)
        {
            SampleScene.Instance.shipsQueued[index] = true;
        }
        ShipCreator.HurtEnemy(hp, entity);
    }
    public void UpdateUI()
    {
        
    }
    public void Stop()
    {

    }
    bool DoShipsExist()
    {
        for (int i = 0; i < shipsQueued.Length; i++)
            if (!shipsQueued[i])
                return false;
        return true;
    }
}
