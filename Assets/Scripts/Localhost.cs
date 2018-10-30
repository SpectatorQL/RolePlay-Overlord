using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public class Localhost : MonoBehaviour
    {
        public Camera Cam;
        public float Yaw;
        public float Pitch;
        float _sensitivity;

        float _delta;
        
        string _dataPath = "file:///Assets/";
        string _modPath;
        Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        Wall[] _walls;

        void ProcessLocalHostKeyboard(Localhost ent, PlayerInput input)
        {
            float speed = 0.1f;
            Vector3 newPos = ent.transform.position;
            if(input.MoveForward)
            {
                newPos += ent.Cam.transform.forward * speed;
            }
            if(input.MoveBackward)
            {
                newPos -= ent.Cam.transform.forward * speed;
            }
            if(input.MoveLeft)
            {
                newPos -= ent.Cam.transform.right * speed;
            }
            if(input.MoveRight)
            {
                newPos += ent.Cam.transform.right * speed;
            }
            ent.transform.position = newPos;

            if(input.Debug_SetTexture1)
            {
                ent.SetTexture("test.png");
            }
            if(input.Debug_SetTexture2)
            {
                ent.SetTexture("test2.png");
            }
        }

        void ProcessHostMouse(Localhost ent)
        {
            Vector3 rotation = new Vector3(ent.Pitch, ent.Yaw, 0.0f);
            ent.transform.eulerAngles = rotation;
        }

        string GetAssetFilePath(string file)
        {
            string result = _dataPath + _modPath + file;
            return result;
        }

        IEnumerator LoadTex(Texture2D tex, string url)
        {
            using(WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
            }
        }
        
        public void SetTexture(string texName)
        {
            int textureWidth = 1024;
            int textureHeight = 1024;
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
                _walls[i].ChangeWallTexture(tex);
            }
        }

        void Start()
        {
            Cam = GetComponent<Camera>();
            Cam.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;

            Yaw = transform.eulerAngles.y;
            Pitch = transform.eulerAngles.x;
            _sensitivity = 2.0f;

            _walls = FindObjectsOfType<Wall>();
        }

        void Update()
        {
            PlayerInput input = new PlayerInput();
            if(Input.GetKey(KeyCode.W))
            {
                input.MoveForward = true;
            }
            if(Input.GetKey(KeyCode.S))
            {
                input.MoveBackward = true;
            }
            if(Input.GetKey(KeyCode.A))
            {
                input.MoveLeft = true;
            }
            if(Input.GetKey(KeyCode.D))
            {
                input.MoveRight = true;
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                input.Debug_SetTexture1 = true;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                input.Debug_SetTexture2 = true;
            }

            ProcessLocalHostKeyboard(this, input);

            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");
            Yaw += inputX * _sensitivity;
            Pitch -= inputY * _sensitivity;

            ProcessHostMouse(this);

            _delta += (Time.deltaTime - _delta) * 0.1f;
        }
        
        void OnGUI()
        {
            float fps = 1.0f / _delta;
            
            Rect fpsRect = new Rect(Screen.width / 2, 20.0f, 250.0f, 30.0f);
            GUI.Label(fpsRect, fps.ToString("00.00"));
        }
    }
}