using System;
using UnityEditor;
using RuntimeSceneManager = UnityEngine.SceneManagement.SceneManager;
using EditorSceneManager = UnityEditor.SceneManagement.EditorSceneManager;

namespace RolePlayOverlord.Editor
{
    [InitializeOnLoad]
    public static class StartFromMainMenu
    {
        static StartFromMainMenu()
        {
            EditorApplication.playModeStateChanged += StateChange;
        }

        static void StateChange(PlayModeStateChange change)
        {
            var currentScene = EditorSceneManager.GetActiveScene();
            if(EditorApplication.isPlaying)
            {
                EditorApplication.playModeStateChanged -= StateChange;

                if(currentScene.name[0] != '_')
                {
                    if(!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        //We're in playmode, just about to change playmode to Editor
                        EditorSceneManager.LoadSceneAsync(1);
                    }
                    else
                    {
                        //We're in playmode, right after having pressed Play
                        RuntimeSceneManager.LoadSceneAsync(0);
                    }
                }
            }
        }
    }
}
