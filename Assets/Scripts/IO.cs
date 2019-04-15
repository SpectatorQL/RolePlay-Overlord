using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using RolePlayOverlord.Utils;

namespace RolePlayOverlord
{
    // TODO: Move mod-related data definitions to a shared DLL once we have a proper writing tool.
    public enum ResourceTypeID
    {
        WallTexture,
        FloorTexture,
        CeilingTexture,
        SkyboxTexture,
        Audio,

        CharacterModel,
        FigureModel,

        Count
    }

    [Serializable]
    public class ResourceType
    {
        public int FirstResourceIndex;
        public int OnePastLastResourceIndex;

        public int Count
        {
            get { return OnePastLastResourceIndex - FirstResourceIndex; }
        }
    }

    [Serializable]
    public class Resource
    {
        public string File;
    }

    public class LocalData
    {
        public string[] Documents;
        public string[] Saves;
    }

    [Serializable]
    public class ModData
    {
        public ResourceType[] ResourceTypeEntries;
        public Resource[] Resources;
        [NonSerialized] public LocalData LocalData;
    }

    [Serializable]
    public class ModManifest
    {
        public string Name;
        public ulong RMMCode;
        public uint Version;

        public ModData Data;
    }

    public static class ModFormatInfo
    {
        // TODO: Make this a compile time constant _somehow_.
        public static readonly ulong RMMCode = RIFFCode64('r', 'm', 'm', ' ');

        public const uint VERSION = 0;

        public static ulong RIFFCode64(char a, char b, char c, char d)
        {
            return (((ulong)(a) << 0) | (ulong)(b) << 16 | (ulong)(c) << 32 | (ulong)(d) << 48);
        }
    }


    public struct ResourceData
    {
        public ResourceTypeID ResourceType;
        public int ID;
    }

    public static class IO
    {
        public static readonly string DEFAULT_ASSETS_PATH;

        public static readonly string DATA_PATH = "Mods/";
        // TODO: Clean this up after most of the game is finished.
        public static string ModPath = "Default/";

        static BinaryFormatter bFormatter = new BinaryFormatter();

        static IO()
        {
#if UNITY_EDITOR
            DEFAULT_ASSETS_PATH = "Assets/Mods/Default/";
#else
            DEFAULT_ASSETS_PATH = "Mods/Default/";
#endif
        }

        public static void LoadMod(ref ModData mod, string manifestFile)
        {
            using(var modStream = new FileStream(PATH(manifestFile), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mod = (ModData)bFormatter.Deserialize(modStream);
            }
            mod.LocalData = new LocalData();


            string docsPath = DATA_PATH + ModPath + "Documents/";
            string[] docs = Directory.GetFiles(PATH(docsPath));

#if UNITY_STANDALONE_WIN
            for(int i = 0;
                i < docs.Length;
                ++i)
            {
                docs[i] = ConvertToUnixPath(docs[i]);
            }
#endif

            mod.LocalData.Documents = docs;


            string savesPath = DATA_PATH + ModPath + "Saves/";
            string[] saves = Directory.GetFiles(PATH(savesPath));

#if UNITY_STANDALONE_WIN
            for(int i = 0;
                i < saves.Length;
                ++i)
            {
                saves[i] = ConvertToUnixPath(saves[i]);
            }
#endif

            mod.LocalData.Saves = saves;
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

        static string ConvertToUnixPath(string str)
        {
            string result;

            char[] arr = str.ToCharArray();
            for(int i = 0; i < arr.Length; ++i)
            {
                char c = arr[i];
                if(c == '\\')
                {
                    c = '/';
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
