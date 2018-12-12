﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RolePlayOverlord.Utils;

namespace RolePlayOverlord
{
    [Serializable]
    public class Mod
    {
        // Scene resources
        [NonSerialized] public const int WTEX = 0;
        [NonSerialized] public const int FTEX = 1;
        [NonSerialized] public const int CTEX = 2;
        [NonSerialized] public const int STEX = 3;
        [NonSerialized] public const int AUDIO = 4;

        // Session resources
        [NonSerialized] public const int CLASSMOD = 5;
        [NonSerialized] public const int FIGUREMOD = 6;

        // Local resources
        [NonSerialized] public const int TEXT = 7;
        [NonSerialized] public const int SAVE = 8;

        [NonSerialized] const int RANK = 9;

        public string Name;
        public string[][] Resources;

        public static Mod Create()
        {
            var mod = new Mod()
            {
                Resources = new string[RANK][]
            };
            return mod;
        }

        private Mod() { }
    }

    public static class IO
    {
        public static string[] _defaultDataDirectories =
        {
            "Mods/Default/Textures/Walls/",
            "Mods/Default/Textures/Floor/",
            "Mods/Default/Textures/Ceiling/",
            "Mods/Default/Textures/Skybox/",

            "Mods/Default/Audio/",

            "Mods/Default/Models/Classes/",
            "Mods/Default/Models/Figures/",

            "Mods/Default/Documents/",

            "Mods/Default/Saves/",
        };

        public static readonly string DEFAULT_ASSETS_PATH;

        public static readonly string DATA_PATH;
        // TODO: Clean this up after most of the game is finished.
        public static string ModPath = "Default/";

        static IO()
        {
#if UNITY_EDITOR
            DEFAULT_ASSETS_PATH = "Assets/Mods/Default/";
            DATA_PATH = "Assets/";
#else
            DEFAULT_ASSETS_PATH = "Mods/Default/";
            DATA_PATH = "Mods/";
#endif
        }

        public static string LoadDocument(string path)
        {
            string result = "";

            using(var stream = new FileStream(PATH(path), FileMode.Open, FileAccess.Read, FileShare.None))
            using(var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static void SaveDocument(string path, string data)
        {
            using(var stream = new FileStream(PATH(path), FileMode.Truncate, FileAccess.Write, FileShare.Read))
            using(var writer = new StreamWriter(stream))
            {
                writer.Write(data);
            }
        }

        static string ConvertToWin32Path(string str)
        {
            string result;

            char[] arr = str.ToCharArray();
            for(int i = 0; i < arr.Length; ++i)
            {
                char c = arr[i];
                if(c == '/')
                {
                    c = '\\';
                    arr[i] = c;
                }
            }
            result = new string(arr);

            return result;
        }

        public static string PATH(string str)
        {
            string result;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            result = ConvertToWin32Path(str);
#else
            result = str;
#endif

            return result;
        }

        public static string FILENAME(string path)
        {
            string result;

            int onePastLastSlash = path.OnePastLastSlash();
            int dot = path.LastIndexOf('.');
            int len = dot - onePastLastSlash;

            result = path.Substring(onePastLastSlash, len);
            UnityEngine.Debug.Assert(!result.Contains("."));

            return result;
        }

        public static string EXTENSION(string file)
        {
            string result;

            int dot = file.LastIndexOf(".");
            result = file.Substring(dot);

            return result;
        }
    }
}
