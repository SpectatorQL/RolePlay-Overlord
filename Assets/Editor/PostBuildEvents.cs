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
            // TODO: What is a resource path?
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
                Debug.Assert(Directory.Exists(path));
            }


            ModManifest defaultMod = new ModManifest
            {
                Name = "Default",
                RMMCode = ModFormatInfo.RMMCode,
                Version = ModFormatInfo.VERSION
            };

            int resourceTypeCount = (int)ResourceTypeID.Count;
            ResourceType[] resourceTypeEntries = new ResourceType[resourceTypeCount];
            for(int i = 0;
                i < resourceTypeCount;
                ++i)
            {
                resourceTypeEntries[i] = new ResourceType();
            }
            defaultMod.Data.ResourceTypeEntries = resourceTypeEntries;

            List<ResourceType> resourceList = new List<ResourceType>();
            WriteResources(resourceList, ResourceTypeID.WallTexture, defaultDataDirs[0], buildDir);
            WriteResources(resourceList, ResourceTypeID.FloorTexture, defaultDataDirs[1], buildDir);
            WriteResources(resourceList, ResourceTypeID.CeilingTexture, defaultDataDirs[2], buildDir);
            WriteResources(resourceList, ResourceTypeID.SkyboxTexture, defaultDataDirs[3], buildDir);
            WriteResources(resourceList, ResourceTypeID.Audio, defaultDataDirs[4], buildDir);
            WriteResources(resourceList, ResourceTypeID.CharacterModel, defaultDataDirs[5], buildDir);
            WriteResources(resourceList, ResourceTypeID.FigureModel, defaultDataDirs[6], buildDir);
            defaultMod.Data.Resources = resourceList.ToArray();
            CopyResources(defaultDataDirs[7], buildDir);
            CopyResources(defaultDataDirs[8], buildDir);


            string defaultModManifest = "build/Mods/Default/Default.rmm";
            using(var fs = new FileStream(PATH(defaultModManifest), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, defaultMod);
            }
        }

        static void WriteResources(List<ResourceType> resList, ResourceTypeID resType, string resTypeDir, string buildDir)
        {
            string dir = "Assets/" + resTypeDir;
            string[] files = Directory.GetFiles(PATH(dir))
                .Where(file =>
                {
                    return EXTENSION(file) != ".meta";
                })
                .ToArray();

            ResourceType res = new ResourceType()
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
