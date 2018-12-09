using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RolePlayOverlord.Utils;

namespace RolePlayOverlord
{
    public static class IO
    {
        public static string[] _defaultDataDirectories =
        {
            "Mods/Default/Textures/Walls/",
            "Mods/Default/Textures/Ceiling/",
            "Mods/Default/Textures/Floor/",
            "Mods/Default/Textures/Skybox/",

            "Mods/Default/Models/Classes/",
            "Mods/Default/Models/Figures/",

            "Mods/Default/Audio/",

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

        /*
            TODO: At some point this will have become obsolete, as
            we will be relying on data structures providing us with
            filenames using the files' paths in the finished game.
        */
        public static string GetAssetFilePath(string file)
        {
            string result = DEFAULT_ASSETS_PATH + file;
            return result;
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
