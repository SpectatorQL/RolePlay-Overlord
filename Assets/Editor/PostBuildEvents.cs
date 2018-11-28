using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using static RolePlayOverlord.IO;

namespace RolePlayOverlord.Editor
{
    public class PostBuildEvents : MonoBehaviour
    {
        static void CreateDefaultDataDirectory(string dir)
        {
            Directory.CreateDirectory(dir);

            string[] dataDirs =
            {
                "Textures/Walls/",
                "Textures/Ceiling/",
                "Textures/Floor/",
                "Textures/Skybox/",

                "Models/Classes/",
                "Models/Figures/",

                "Audio/",

                "Documents/",

                "Saves/",
            };

            foreach(var d in dataDirs)
            {
                string path = PATH(dir + d);
                Directory.CreateDirectory(path);
                Debug.Assert(Directory.Exists(path));
            }
        }

        /* 
            NOTE(SpectatorQL): This guy may need to be changed frequently.
            Keep this in mind, and don't freak out if some weird errors
            appear in the console after building the player.
            IMPORTANT(SpectatorQL): This is non-portable!
        */
        [PostProcessBuild(0)]
        static void BuildTestAssets(BuildTarget target, string pathToBuiltProject)
        {
            // NOTE(SpectatorQL): Removes the built executable from the path.
            int onePastLastSlash = OnePastLastSlash(pathToBuiltProject);
            string buildDir = pathToBuiltProject.Remove(onePastLastSlash, pathToBuiltProject.Length - onePastLastSlash);

            string defaultDataDir = "Mods/Default/";
            string defaultDataDirPath = PATH(buildDir + defaultDataDir);
            CreateDefaultDataDirectory(defaultDataDirPath);

            string defaultAssetsPath = PATH(Application.dataPath + "/" + defaultDataDir);
            DirectoryInfo projectDirInfo = new DirectoryInfo(defaultAssetsPath);

            string[] defaultAssetExtensions = { ".png", ".ogg", ".txt" };
            var defaultFiles = projectDirInfo.GetFiles()
                .Where(file =>
                {
                    return defaultAssetExtensions.Contains(file.Extension);
                })
                .ToArray();

            for(int i = 0;
                i < defaultFiles.Length;
                ++i)
            {
                string assetFileName = defaultFiles[i].Name;
                string destFileName = defaultDataDirPath + assetFileName;
                File.Copy(defaultFiles[i].FullName, destFileName, true);
                Debug.Assert(File.Exists(destFileName));
            }
        }
    }
}
