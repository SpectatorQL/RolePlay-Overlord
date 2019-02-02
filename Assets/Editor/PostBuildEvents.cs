using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using RolePlayOverlord.Utils;
using static RolePlayOverlord.IO;

namespace RolePlayOverlord.Editor
{
    public class PostBuildEvents : MonoBehaviour
    {
        /* 
            NOTE(SpectatorQL): This guy may need to be changed frequently.
            Keep this in mind, and don't freak out if some weird errors
            appear in the console after building the player.
            IMPORTANT(SpectatorQL): This is non-portable!
        */
        [PostProcessBuild(0)]
        static void BuildTestAssets(BuildTarget target, string pathToBuiltProject)
        {
            // TODO: Move to the Resource class. Every Resource should know its base directory.
            string[] defaultDataDirs =
            {
                "Mods/Default/Textures/Walls/",
                "Mods/Default/Textures/Floor/",
                "Mods/Default/Textures/Ceiling/",
                "Mods/Default/Textures/Skybox/",

                "Mods/Default/Audio/",

                "Mods/Default/Models/Classes/",
                "Mods/Default/Models/Figures/",

                "Mods/Default/Documents/",

                "Mods/Default/Saves/",
            };

            // NOTE(SpectatorQL): Removes the built executable from the path.
            int onePastLastSlash = pathToBuiltProject.OnePastLastSlash();
            string buildDir = pathToBuiltProject.Remove(onePastLastSlash, pathToBuiltProject.Length - onePastLastSlash);
            string modsDir = buildDir + "Mods/";

            if(Directory.Exists(PATH(modsDir)))
            {
                Directory.Delete(PATH(modsDir), true);
            }
            foreach(var d in defaultDataDirs)
            {
                string path = PATH(buildDir + d);
                Directory.CreateDirectory(path);
                UnityEngine.Debug.Assert(Directory.Exists(path));
            }


            var defaultMod = new Mod();
            defaultMod.Name = "Default";

#if true
            List<Resource> resourceList = new List<Resource>();
            WriteResources(ref resourceList, ResourceType.WallTexture, defaultDataDirs[0], buildDir);
            WriteResources(ref resourceList, ResourceType.FloorTexture, defaultDataDirs[1], buildDir);
            WriteResources(ref resourceList, ResourceType.CeilingTexture, defaultDataDirs[2], buildDir);
            WriteResources(ref resourceList, ResourceType.SkyboxTexture, defaultDataDirs[3], buildDir);
            WriteResources(ref resourceList, ResourceType.Audio, defaultDataDirs[4], buildDir);
            WriteResources(ref resourceList, ResourceType.CharacterModel, defaultDataDirs[5], buildDir);
            WriteResources(ref resourceList, ResourceType.FigureModel, defaultDataDirs[6], buildDir);
            defaultMod.Resources = resourceList.ToArray();
#else
            WriteResources(ref defaultMod.WallTextures, defaultDataDirs[0], buildDir);
            WriteResources(ref defaultMod.FloorTextures, defaultDataDirs[1], buildDir);
            WriteResources(ref defaultMod.CeilingTextures, defaultDataDirs[2], buildDir);
            WriteResources(ref defaultMod.SkyboxTextures, defaultDataDirs[3], buildDir);
            WriteResources(ref defaultMod.Audio, defaultDataDirs[4], buildDir);
            WriteResources(ref defaultMod.CharacterModels, defaultDataDirs[5], buildDir);
            WriteResources(ref defaultMod.FigureModels, defaultDataDirs[6], buildDir);
#endif
            CopyResources(defaultDataDirs[7], buildDir);
            CopyResources(defaultDataDirs[8], buildDir);

            string defaultModManifest = "build/Mods/Default/Default.rmm";
            using(var fs = new FileStream(PATH(defaultModManifest), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, defaultMod);
            }
        }

        static void WriteResources(ref List<Resource> resList, ResourceType resType, string resTypeDir, string buildDir)
        {
            string dir = "Assets/" + resTypeDir;
            string[] files = Directory.GetFiles(PATH(dir))
                .Where(file =>
                {
                    return EXTENSION(file) != ".meta";
                })
                .ToArray();

            Resource res = new Resource()
            {
                ID = resType,
                Data = new string[files.Length]
            };

            for(int i = 0;
                i < files.Length;
                ++i)
            {
                string assetFile = Path.GetFileName(files[i]);
                string srcFile = dir + assetFile;
                string destFile = buildDir + resTypeDir + assetFile;

                File.Copy(PATH(srcFile), PATH(destFile), true);
                UnityEngine.Debug.Assert(File.Exists(PATH(destFile)));

                res.Data[i] = resTypeDir + assetFile;
            }

            resList.Add(res);
        }

        static void WriteResources(ref string[] arr, string resTypeDir, string buildDir)
        {
            string dir = "Assets/" + resTypeDir;
            string[] files = Directory.GetFiles(PATH(dir))
                .Where(file =>
                {
                    return EXTENSION(file) != ".meta";
                })
                .ToArray();

            arr = new string[files.Length];

            for(int i = 0;
                i < files.Length;
                ++i)
            {
                string assetFile = Path.GetFileName(files[i]);
                string srcFile = dir + assetFile;
                string destFile = buildDir + resTypeDir + assetFile;

                File.Copy(PATH(srcFile), PATH(destFile), true);
                UnityEngine.Debug.Assert(File.Exists(PATH(destFile)));

                arr[i] = resTypeDir + assetFile;
            }
        }

        static void CopyResources(string resTypeDir, string buildDir)
        {
            string dir = "Assets/" + resTypeDir;
            string[] files = Directory.GetFiles(PATH(dir))
                .Where(file =>
                {
                    return EXTENSION(file) != ".meta";
                })
                .ToArray();

            for(int i = 0;
                i < files.Length;
                ++i)
            {
                string assetFile = Path.GetFileName(files[i]);
                string srcFile = dir + assetFile;
                string destFile = buildDir + resTypeDir + assetFile;

                File.Copy(PATH(srcFile), PATH(destFile), true);
                UnityEngine.Debug.Assert(File.Exists(PATH(destFile)));
            }
        }
    }
}
