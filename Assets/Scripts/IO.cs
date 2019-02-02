using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using RolePlayOverlord.Utils;

namespace RolePlayOverlord
{
    public enum ResourceType
    {
        WallTexture,
        FloorTexture,
        CeilingTexture,
        SkyboxTexture,
        Audio,

        CharacterModel,
        FigureModel,
    }

    [Serializable]
    public class Resource
    {
        public ResourceType ID;
        public string[] Data;
    }

    [Serializable]
    public class Mod
    {
        public string Name;

        public string[] WallTextures;
        public string[] FloorTextures;
        public string[] CeilingTextures;
        public string[] SkyboxTextures;
        public string[] Audio;

        public string[] CharacterModels;
        public string[] FigureModels;

        public Resource[] Resources;

        [NonSerialized] public LocalData LocalData; 

        public Resource GetResource(ResourceType type)
        {
            Resource res = Resources.Single(r => (r.ID == type));
            return res;
        }

        public string[][] GetSceneResources()
        {
            string[][] sceneRes =
            {
                WallTextures,
                FloorTextures,
                CeilingTextures,
                SkyboxTextures,
                Audio
            };

            return sceneRes;
        }
    }

    public class LocalData
    {
        public string[] Documents;
        public string[] Saves;
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

        public static void LoadCurrentMod(ref Mod mod, string manifestFile)
        {
            using(var modStream = new FileStream(PATH(manifestFile), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mod = (Mod)bFormatter.Deserialize(modStream);
            }
        }

        public static void LoadLocalData(ref LocalData localData)
        {
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
            localData.Documents = docs;


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
            localData.Saves = saves;
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
