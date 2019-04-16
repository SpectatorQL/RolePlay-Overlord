﻿using System;
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
        public static string WorkingDirectory;
        static BinaryFormatter bFormatter = new BinaryFormatter();

        public static void LoadModData(ref ModData mod, string manifestFileName)
        {
            using(var modStream = new FileStream(PATH(manifestFileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ModManifest modManifest = (ModManifest)bFormatter.Deserialize(modStream);
                if(modManifest.RMMCode == ModFormatInfo.RMMCode)
                {
                    if(modManifest.Version == ModFormatInfo.VERSION)
                    {
                        mod = modManifest.Data;
                        mod.LocalData = new LocalData();

                        string docsPath = WorkingDirectory + "Documents/";
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


                        string savesPath = WorkingDirectory + "Saves/";
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
                    else
                    {
                        Debug.LogError("Error: Unsupported manifest file version!");
                    }
                }
                else
                {
                    Debug.LogError("Error: The file is not a RoleplayOverlod manifest file!");
                }
            }
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

        public static string ConvertToUnixPath(string str)
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
