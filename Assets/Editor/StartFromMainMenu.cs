using UnityEditor;
using RuntimeSceneManager = UnityEngine.SceneManagement.SceneManager;
using EditorSceneManager = UnityEditor.SceneManagement.EditorSceneManager;

namespace RolePlayOverlord.Editor
{
    [InitializeOnLoad]
    public static class StartFromMainMenu
    {
        static string _menuScene = "Menu";

        static StartFromMainMenu()
        {
            EditorApplication.playModeStateChanged += StateChange;
        }

        static void StateChange(PlayModeStateChange change)
        {
            string currentSceneName = EditorSceneManager.GetActiveScene().name;
            if(EditorApplication.isPlaying)
            {
                EditorApplication.playModeStateChanged -= StateChange;

                if(currentSceneName[0] != '_')
                {
                    if(currentSceneName != _menuScene)
                    {
                        // NOTE(SpectatorQL): We're in playmode, right after having pressed Play.
                        RuntimeSceneManager.LoadSceneAsync(_menuScene);
                    }
                }
            }
        }
    }
}
