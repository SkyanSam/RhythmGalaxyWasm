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
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System.Xml;
public class SampleScene : Scene 
{
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
    public void Start()
    {
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

        SongManager.Instance = new SongManager();
        SongManager.Instance.Start("GoneAndForgotten.mp3", "GoneAndForgotten.mid", 167 * 2);
        GameUI.songName = "Gone and Forgotten";
        GameUI.artistName = "Apechs.mp3";
        GameUI.Init();

        player = new Player();
        player.Start();

        shipIndices[0] = Database.AddEntity(ShipCreator.CreateShip1(new Vector2(320, 130)));
        shipIndices[1] = Database.AddEntity(ShipCreator.CreateShip2(new Vector2(460, 70)));
        shipIndices[2] = Database.AddEntity(ShipCreator.CreateShip3(new Vector2(600, 130)));
        shipIndices[3] = Database.AddEntity(ShipCreator.CreateShip1(new Vector2(320, 130)));
        shipIndices[4] = Database.AddEntity(ShipCreator.CreateShip2(new Vector2(460, 70)));
        shipIndices[5] = Database.AddEntity(ShipCreator.CreateShip3(new Vector2(600, 130)));

        for (int i = 0; i < shipIndices.Length; i++)
        {
            var hb = Database.entities[shipIndices[i]].GetComponent<HitboxComponent>();
            var index = i;
            var signal = hb.signals[0];
            hb.signals[0] = (int hp) =>
            {
                //Console.WriteLine($"index {index}");
                if (hp <= 0) shipsQueued[index] = true;
                signal(hp); // little workaround?
            };
            Database.entities[shipIndices[i]].SetComponent(hb);
        }

        SongManager.Instance.signals.Add((int step) =>
        {
            Console.WriteLine($"Signal Step {step}");
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
        });
    }
    public void Update()
    {

        ClearBackground(Color.BLACK);
        DrawRectangle(250, 0, 460, 540, new Color(34, 20, 31, 255));


        SongManager.Instance.Update();

        for (int i = 0; i < shipIndices.Length; i++)
        {
            if (!shipsQueued[i])
            {
                var transform = Database.GetComponent<TransformComponent>(Database.entities[shipIndices[i]]);
                var position = CircleFormation(i * (360f / 6f));
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
        return new Vector2(480, 148) + (159 * new Vector2(MathF.Cos(rot), MathF.Sin(rot)));
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
