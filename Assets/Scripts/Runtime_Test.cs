using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    /*
        NOTE(SpectatorQL): This is all test code for now.
    */
    public class Runtime_Test : NetworkBehaviour
    {
        static string _dataPath;
        static string _modPath;
        // TODO: This is static for now. Change it once we implement
        // dynamic asset loading?
        static Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

        Wall[] _walls;
        float _delta;

        void InitializeGame()
        {
            int targetFrames = 60;
            Application.targetFrameRate = targetFrames;
            QualitySettings.vSyncCount = 0;
            
#if UNITY_EDITOR
            _dataPath = "file:///Assets/";
#else
            _dataPath = "file:///Test/";
#endif
        }

        void Awake()
        {
            InitializeGame();

            Type[] components =
            {
                typeof(Runtime_Test),
                typeof(Camera),
                typeof(MouseCamLook)
            };
            GameObject go = new GameObject("Runtime_Test", components);
            go.transform.position = new Vector3(0.0f, 7.0f, 0.0f);
        }

        void Start()
        {
            _walls = FindObjectsOfType<Wall>();
        }
        
        string GetAssetFilePath(string file)
        {
            string result = _dataPath + _modPath + file;
            return result;
        }

        // TODO: Turn this into something more reasonable.
        IEnumerator LoadTex(Texture2D tex, string url)
        {
            using(WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
            }
        }

        void Update()
        {
            if(isServer)
            {
                string keyString = Input.inputString;
                switch(keyString)
                {
                    case "1":
                    {
                        int textureWidth = 200;
                        int textureHeight = 200;
                        string texName = "test.png";
                        string texUrl = GetAssetFilePath(texName);

                        Texture2D tex;
                        if(_textureCache.ContainsKey(texUrl))
                        {
                            tex = _textureCache[texUrl];
                        }
                        else
                        {
                            tex = new Texture2D(textureWidth, textureHeight);
                            StartCoroutine(LoadTex(tex, texUrl));
                            _textureCache.Add(texUrl, tex);
                        }

                        for(int i = 0; i < _walls.Length; ++i)
                        {
                            _walls[i].Renderer.material.mainTexture = tex;
                        }

                        break;
                    }

                    case "2":
                    {
                        int textureWidth = 200;
                        int textureHeight = 200;
                        string texName = "test2.png";
                        string texUrl = GetAssetFilePath(texName);

                        Texture2D tex;
                        if(_textureCache.ContainsKey(texUrl))
                        {
                            tex = _textureCache[texUrl];
                        }
                        else
                        {
                            tex = new Texture2D(textureWidth, textureHeight);
                            StartCoroutine(LoadTex(tex, texUrl));
                            _textureCache.Add(texUrl, tex);
                        }

                        for(int i = 0; i < _walls.Length; ++i)
                        {
                            _walls[i].Renderer.material.mainTexture = tex;
                        }

                        break;
                    }
                }
            }

            _delta += (Time.deltaTime - _delta) * 0.1f;
        }

        void OnGUI()
        {
            float fps = 1.0f / _delta;

            float x = Screen.width / 2;
            float y = 20.0f;
            float width = 250.0f;
            float height = 30.0f;
            GUI.Label(new Rect(x, y, width, height), fps.ToString("00.00"));
        }
    }
}
