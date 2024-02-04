using RhythmGalaxy;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static RhythmGalaxy.TextBoxSystem;
using System.Numerics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.Contracts;
using Melanchall.DryWetMidi.MusicTheory;
using System.Diagnostics;
using System;
using Microsoft.CodeAnalysis.Scripting.Hosting;

public static class GameUI
{
    public static float energyPercent = .6f;
    public static float healthPercent = .8f;
    public static float conveyorOffset = .2f;
    public static int score;
    public static string songName;
    public static string artistName;
    public static int BPM;
    public enum ConveyorMode { Hold, Tap }
    public static ConveyorMode conveyorMode = ConveyorMode.Tap;

    public static Texture2D energyHealthBars, conveyorTile, conveyorHit, conveyorHoldEnd, conveyorHoldMiddle, conveyorHoldStart, conveyorSlowDown, conveyorSpeedUp;
    public static Font chavaFont;
    public static void Init()
    {
        energyHealthBars = LoadTexture("Resources/Sprites/EnergyHealthBars.png");
        conveyorTile = LoadTexture("Resources/Sprites/ConveyorTile.png");
        conveyorHit = LoadTexture("Resources/Sprites/ConveyorHit.png");
        conveyorHoldEnd = LoadTexture("Resources/Sprites/ConveyorHoldEnd.png");
        conveyorHoldMiddle = LoadTexture("Resources/Sprites/ConveyorHoldMiddle.png");
        conveyorHoldStart = LoadTexture("Resources/Sprites/ConveyorHoldStart.png");
        conveyorSlowDown = LoadTexture("Resources/Sprites/ConveyorSlowDown.png");
        conveyorSpeedUp = LoadTexture("Resources/Sprites/ConveyorSpeedUp.png");
        chavaFont = LoadFont("Resources/Fonts/ChavaRegular.ttf");
    }
    public static void Draw()
    {
        // Draw UI Background
        DrawRectangle(0, 0, 250, 540, ColorPallete.Black2);
        DrawRectangle(710, 0, 250, 540, ColorPallete.Black2);

        // Draw Text Box Background
        DrawRectangle(13, 30, 132, 147, ColorPallete.White2);
        DrawRectangle(808, 29, 132, 112, ColorPallete.White2);

        // Draw Energy & Health Bar Filling
        DrawRectangle(720, 30 + (int)((1f - energyPercent) * 160f), 20, (int)(energyPercent * 160f), ColorPallete.Yellow);
        DrawRectangle(760, 30 + (int)((1f - healthPercent) * 160f), 20, (int)(healthPercent * 160f), ColorPallete.PinkRed);

        
        // Draw Energy & Health Bar Deco
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 8; j++)
                if (j == 0 || j == 7) DrawTexturePro(energyHealthBars, new Rectangle(10 * i, j == 0 ? 0 : 20, 10, 10), new Rectangle(720 + (40 * i), j == 0 ? 30 : 170, 20, 20), Vector2.Zero, 0, Color.WHITE);
                else DrawTexturePro(energyHealthBars, new Rectangle(10 * i, 10, 10, 10), new Rectangle(720 + (40 * i), 30 + (20 * j), 20, 20), Vector2.Zero, 0, Color.WHITE);

        // Draw Conveyor Belt
        conveyorOffset = 1f - SongManager.GetCurrentStepRemainder();
        for (int i = 0; i < 8; i++)
            DrawTexturePro(conveyorTile, new Rectangle(0, conveyorOffset * 30, 30, 30), new Rectangle(177, 30 + (60 * i), 60, 60), Vector2.Zero, 0, Color.WHITE);

        for (int i = 0; i < 8; i++)
        {
            var source = new Rectangle(0, 0, 30, 30);
            var dest = new Rectangle(177, 30 + (60 * 7) - (60 * i) - (conveyorOffset * 60), 60, 60);
            if (SongManager.IsMidNoteAtStep(SongManager.GetCurrentStep() + i, NoteName.E))
            {
                
                DrawTexturePro(conveyorSlowDown, source, dest, Vector2.Zero, 0, Color.WHITE);
            }
            else if (SongManager.IsMidNoteAtStep(SongManager.GetCurrentStep() + i, NoteName.F))
            {
                
                DrawTexturePro(conveyorSpeedUp, source, dest, Vector2.Zero, 0, Color.WHITE);
            }
            else if (conveyorMode == ConveyorMode.Tap && SongManager.IsStartNoteAtStep(SongManager.GetCurrentStep() + i, NoteName.C))
            {
                
                DrawTexturePro(conveyorHit, source, dest, Vector2.Zero, 0, Color.WHITE);
            }
            else if (conveyorMode == ConveyorMode.Hold)
            {
                if (SongManager.IsEndNoteAtStep(SongManager.GetCurrentStep() + i, NoteName.D))
                {
                    
                    DrawTexturePro(conveyorHoldEnd, source, dest, Vector2.Zero, 0, Color.WHITE);
                }
                else if (SongManager.IsStartNoteAtStep(SongManager.GetCurrentStep() + i, NoteName.D))
                {
                    
                    DrawTexturePro(conveyorHoldStart, source, dest, Vector2.Zero, 0, Color.WHITE);
                }
                else if (SongManager.IsMidNoteAtStep(SongManager.GetCurrentStep() + i, NoteName.D))
                {
                    
                    DrawTexturePro(conveyorHoldMiddle, source, dest, Vector2.Zero, 0, Color.WHITE);
                }
            }
        }
        // cannot set line spacing with raylib v 4.5, may need to make a helper function for this workaround
        DrawTextEx(chavaFont, "C\nO\nN\nV\nE\nY\nO\nR", new Vector2(156, 30), 30, 50, Color.WHITE);
        DrawTextEx(chavaFont, "E\nN\nE\nR\nG\nY", new Vector2(740, 30), 30, 50, Color.WHITE);
        DrawTextEx(chavaFont, "H\nE\nA\nL\nT\nH", new Vector2(780, 30), 30, 50, Color.WHITE);
        DrawTextBoxed(chavaFont, $"SCORE\n{score}", new Rectangle(819, 36, 112, 96), 20, 0, false, Color.WHITE);
        DrawTextBoxed(chavaFont, $"{songName}\nby {artistName}\n\n BPM {BPM}", new Rectangle(19, 36, 126, 140), 20, 0, true, Color.WHITE);
    }
    
}