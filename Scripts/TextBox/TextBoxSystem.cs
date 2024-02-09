﻿using System;
using System.Numerics;
using System.Text;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace RhythmGalaxy
{
    class TextBoxSystem
    {
        public static void DrawTextBoxed(
            Font font,
            string text,
            Rectangle rec,
            float fontSize,
            float spacing,
            bool wordWrap,
            Color tint
        )
        {
            DrawTextBoxedSelectable(font, text, rec, fontSize, spacing, wordWrap, tint, 0, 0, Color.WHITE, Color.WHITE);
        }

        // Draw text using font inside rectangle limits with support for text selection

        public static unsafe sbyte* GetSByte(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);


            unsafe
            {
                fixed (byte* p = bytes)
                {
                    sbyte* sp = (sbyte*)p;
                    //SP is now what you want
                    return sp;
                }
            }
        }
        public static unsafe void DrawTextBoxedSelectable(
            Font font,
            string text,
            Rectangle rec,
            float fontSize,
            float spacing,
            bool wordWrap,
            Color tint,
            int selectStart,
            int selectLength,
            Color selectTint,
            Color selectBackTint
        )
        {
            int length = text.Length;

            // Offset between lines (on line break '\n')
            float textOffsetY = 0;

            // Offset X to next character to draw
            float textOffsetX = 0.0f;

            // Character rectangle scaling factor
            float scaleFactor = fontSize / (float)font.baseSize;

            // Word/character wrapping mechanism variables
            bool shouldMeasure = wordWrap;

            // Index where to begin drawing (where a line begins)
            int startLine = -1;

            // Index where to stop drawing (where a line ends)
            int endLine = -1;

            // Holds last value of the character position
            int lastk = -1;

            sbyte* textNative = GetSByte(text);
            

                for (int i = 0, k = 0; i < length; i++, k++)
                {
                    // Get next codepoint from byte string and glyph index in font
                    int codepointByteCount = 0;
                    int codepoint = GetCodepoint(&textNative[i], &codepointByteCount);
                    int index = GetGlyphIndex(font, codepoint);

                    // NOTE: Normally we exit the decoding sequence as soon as a bad byte is found (and return 0x3f)
                    // but we need to draw all of the bad bytes using the '?' symbol moving one byte
                    if (codepoint == 0x3f)
                    {
                        codepointByteCount = 1;
                    }

                    i += (codepointByteCount - 1);

                    float glyphWidth = 0;
                    if (codepoint != '\n')
                    {
                        glyphWidth = (font.glyphs[index].advanceX == 0) ?
                            font.recs[index].width * scaleFactor :
                            font.glyphs[index].advanceX * scaleFactor;

                        if (i + 1 < length)
                        {
                            glyphWidth = glyphWidth + spacing;
                        }
                    }

                    // NOTE: When wordWrap is ON we first measure how much of the text we can draw before going outside of
                    // the rec container. We store this info in startLine and endLine, then we change states, draw the text
                    // between those two variables and change states again and again recursively until the end of the text
                    // (or until we get outside of the container). When wordWrap is OFF we don't need the measure state so
                    // we go to the drawing state immediately and begin drawing on the next line before we can get outside
                    // the container.
                    if (shouldMeasure)
                    {
                        // TODO: There are multiple types of spaces in UNICODE, maybe it's a good idea to add support for
                        // more. Ref: http://jkorpela.fi/chars/spaces.html
                        if ((codepoint == ' ') || (codepoint == '\t') || (codepoint == '\n'))
                        {
                            endLine = i;
                        }

                        if ((textOffsetX + glyphWidth) > rec.width)
                        {
                            endLine = (endLine < 1) ? i : endLine;
                            if (i == endLine)
                            {
                                endLine -= codepointByteCount;
                            }
                            if ((startLine + codepointByteCount) == endLine)
                            {
                                endLine = (i - codepointByteCount);
                            }

                            shouldMeasure = !shouldMeasure;
                        }
                        else if ((i + 1) == length)
                        {
                            endLine = i;
                            shouldMeasure = !shouldMeasure;
                        }
                        else if (codepoint == '\n')
                        {
                            shouldMeasure = !shouldMeasure;
                        }

                        if (!shouldMeasure)
                        {
                            textOffsetX = 0;
                            i = startLine;
                            glyphWidth = 0;

                            // Save character position when we switch states
                            int tmp = lastk;
                            lastk = k - 1;
                            k = tmp;
                        }
                    }
                    else
                    {
                        if (codepoint == '\n')
                        {
                            if (!wordWrap)
                            {
                                textOffsetY += (font.baseSize + font.baseSize / 2) * scaleFactor;
                                textOffsetX = 0;
                            }
                        }
                        else
                        {
                            if (!wordWrap && ((textOffsetX + glyphWidth) > rec.width))
                            {
                                textOffsetY += (font.baseSize + font.baseSize / 2) * scaleFactor;
                                textOffsetX = 0;
                            }

                            // When text overflows rectangle height limit, just stop drawing
                            if ((textOffsetY + font.baseSize * scaleFactor) > rec.height)
                            {
                                break;
                            }

                            // Draw selection background
                            bool isGlyphSelected = false;
                            if ((selectStart >= 0) && (k >= selectStart) && (k < (selectStart + selectLength)))
                            {
                                DrawRectangleRec(
                                    new Rectangle(
                                        rec.x + textOffsetX - 1,
                                        rec.y + textOffsetY,
                                        glyphWidth,
                                        (float)font.baseSize * scaleFactor
                                    ),
                                    selectBackTint
                                );
                                isGlyphSelected = true;
                            }

                            // Draw current character glyph
                            if ((codepoint != ' ') && (codepoint != '\t'))
                            {
                                DrawTextCodepoint(
                                    font,
                                    codepoint,
                                    new Vector2(rec.x + textOffsetX, rec.y + textOffsetY),
                                    fontSize,
                                    isGlyphSelected ? selectTint : tint
                                );
                            }
                        }

                        if (wordWrap && (i == endLine))
                        {
                            textOffsetY += (font.baseSize + font.baseSize / 2) * scaleFactor;
                            textOffsetX = 0;
                            startLine = endLine;
                            endLine = -1;
                            glyphWidth = 0;
                            selectStart += lastk - k;
                            k = lastk;

                            shouldMeasure = !shouldMeasure;
                        }
                    }

                    if ((textOffsetX != 0) || (codepoint != ' '))
                    {
                        // avoid leading spaces
                        textOffsetX += glyphWidth;
                    }
                }
            
        }
    }
}
