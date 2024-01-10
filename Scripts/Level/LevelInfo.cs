/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raylib_cs;
using static Raylib_cs.Raylib;
using CSScripting;
using CSScriptLib;
using System.IO;

namespace RhythmGalaxy
{
    class LevelInfo
    {
        public string LevelTitle;
        public string LevelAuthor;
        public string LevelDescription;
        public string MusicTitle;
        public string MusicAuthor;
        public string ThumbnailFilePath;
        public string ScriptFilePath;
        public string MusicFilePath;
        public LevelInfo(string jsonFilePath)
        {
            dynamic jObject = new JObject(File.ReadAllText(jsonFilePath));
            LevelTitle = jObject.levelTitle;
            LevelAuthor = jObject.levelAuthor;
            LevelDescription = jObject.levelDescription;
            MusicTitle = jObject.musicTitle;
            MusicAuthor = jObject.musicAuthor;
            ThumbnailFilePath = jObject.thumbnailFilePath;
            ScriptFilePath = jObject.scriptFilePath;
            MusicFilePath = jObject.musicFilePath;
        }
        public Music LoadMusic()
        {
            return LoadMusicStream(Globals.levelsPath + $"/{LevelTitle}_{LevelAuthor}/{MusicFilePath}");
        }
        public Texture2D LoadThumbnail()
        {
            return LoadTexture(Globals.levelsPath + $"/{LevelTitle}_{LevelAuthor}/{ThumbnailFilePath}");
        }
        public object LoadScript()
        {
            return CSScript.Evaluator.LoadFile(Globals.levelsPath + $"/{LevelTitle}_{LevelAuthor}/{ScriptFilePath}");
        }
    }
}*/
