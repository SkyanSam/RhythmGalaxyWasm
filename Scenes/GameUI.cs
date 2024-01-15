using RhythmGalaxy;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static RhythmGalaxy.TextBoxSystem;
using System.Numerics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class GameUI
{
    public static float energyPercent = .6f;
    public static float healthPercent = .8f;
    public static float conveyorOffset = .2f;
    public static void Draw()
    {
        Texture2D energyHealthBars = LoadTexture("Resources/Sprites/EnergyHealthBars.png");
        Texture2D conveyorTile = LoadTexture("Resources/Sprites/ConveyorTile.png");
        Font chavaFont = LoadFont("Resources/Fonts/ChavaRegular.ttf");
        DrawRectangle(0, 0, 250, 540, ColorPallete.Black2);
        DrawRectangle(710, 0, 250, 540, ColorPallete.Black2);

        DrawRectangle(13, 30, 132, 147, ColorPallete.White2);
        DrawRectangle(808, 29, 132, 112, ColorPallete.White2);

        DrawRectangle(720, 30 + (int)((1f - energyPercent) * 160f), 20, (int)(energyPercent * 160f), ColorPallete.Yellow);
        DrawRectangle(760, 30 + (int)((1f - healthPercent) * 160f), 20, (int)(healthPercent * 160f), ColorPallete.PinkRed);

        // this is for testing purposes
        if (IsKeyDown(KeyboardKey.KEY_SPACE))
            conveyorOffset += 0.05f;
        //
        
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 8; j++)
                if (j == 0 || j == 7) DrawTexturePro(energyHealthBars, new Rectangle(10 * i, j == 0 ? 0 : 20, 10, 10), new Rectangle(720 + (40 * i), j == 0 ? 30 : 170, 20, 20), Vector2.Zero, 0, Color.WHITE);
                else DrawTexturePro(energyHealthBars, new Rectangle(10 * i, 10, 10, 10), new Rectangle(720 + (40 * i), 30 + (20 * j), 20, 20), Vector2.Zero, 0, Color.WHITE);

        for (int i = 0; i < 8; i++)
            DrawTexturePro(conveyorTile, new Rectangle(0, conveyorOffset * 30, 30, 30), new Rectangle(177, 30 + (60 * i), 60, 60), Vector2.Zero, 0, Color.WHITE);

        // cannot set line spacing with raylib v 4.5, may need to make a helper function for this workaround
        DrawTextEx(chavaFont, "C\nO\nN\nV\nE\nY\nO\nR", new Vector2(156, 30), 30, 50, Color.WHITE);
        DrawTextEx(chavaFont, "E\nN\nE\nR\nG\nY", new Vector2(740, 30), 30, 50, Color.WHITE);
        DrawTextEx(chavaFont, "H\nE\nA\nL\nT\nH", new Vector2(780, 30), 30, 50, Color.WHITE);
        DrawTextBoxed(chavaFont, "SCORE\n9999", new Rectangle(819, 36, 112, 96), 20, 0, false, Color.WHITE);
        DrawTextBoxed(chavaFont, "Gone and Forgotten\nby Apechs\n\n BPM 175", new Rectangle(19, 36, 126, 140), 20, 0, true, Color.WHITE);
    }
    
}