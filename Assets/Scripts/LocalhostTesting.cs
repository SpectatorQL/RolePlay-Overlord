using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RolePlayOverlord
{
    public class LocalhostTesting : MonoBehaviour
    {
        string _localhostScene = "_Offline";
        string _mainScene = "MainScene";
        string _menuScene = "Menu";

        void Awake()
        {
            var objs = FindObjectsOfType<LocalhostTesting>();
            if(objs.Length > 1)
            {
                Destroy(objs[objs.Length - 1].gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /*
            NOTE(SpectatorQL): Since this is debug-only I don't really care about
            the perfomance hit that this might incur.
        */
        void Update()
        {
            if(SceneManager.GetActiveScene().name == _mainScene)
            {
                Destroy(gameObject);
            }
        }

        void OnGUI()
        {
            int xOffset = 200;
            int yOffset = 80;

            var rect = new Rect(Screen.width - xOffset,
                Screen.height - yOffset,
                xOffset,
                yOffset);
            if(GUI.Button(rect, "Localhost"))
            {
                if(SceneManager.GetActiveScene().name != _localhostScene)
                {
                    SceneManager.LoadSceneAsync(_localhostScene);
                }
            }

            var rect2 = rect;
            rect2.x -= xOffset;
            if(GUI.Button(rect2, "Menu"))
            {
                if(SceneManager.GetActiveScene().name != _menuScene)
                {
                    SceneManager.LoadSceneAsync(_menuScene);
                }
            }
        }
    }
}