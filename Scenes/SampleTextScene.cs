using Raylib_cs;
using RhythmGalaxy;
using RhythmGalaxy.ECS;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System;
using static Raylib_cs.Raylib;
public class SampleTextScene : Scene 
{
    Font font;
    public void Start()
    {
        font = LoadFont("Resources/Fonts/ChavaRegular.ttf");
    }
    public void Update()
    {
        ClearBackground(Color.BLACK);
        DrawRectangle(250, 0, 460, 540, new Color(34, 20, 31, 255));
        TextBoxSystem.DrawTextBoxed(font, "This is a test of grave error", new Rectangle(250, 0, 460, 540), 40, 0, false, Color.WHITE);
    }
    public void Stop()
    {

    }
}