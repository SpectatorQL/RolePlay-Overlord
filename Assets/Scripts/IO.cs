using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord
{
    public static class IO
    {
        public static readonly string DATA_PATH;
        public static string ModPath;

        static Dictionary<string, string> _modDict;

        static IO()
        {
#if UNITY_EDITOR
            DATA_PATH = "Assets/";
#else
            DATA_PATH = "Mods/";
#endif
        }

        public static int OnePastLastSlash(string src)
        {
            int result = src.LastIndexOf('/') + 1;
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
                }
            }
            result = arr.ToString();

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

            int onePastLastSlash = OnePastLastSlash(path);
            int dot = path.LastIndexOf('.');
            int len = dot - onePastLastSlash;

            result = path.Substring(onePastLastSlash, len);
            Debug.Assert(!result.Contains("."));

            return result;
        }
    }
}
