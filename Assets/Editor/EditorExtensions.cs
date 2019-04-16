using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using static RolePlayOverlord.IO;

namespace RolePlayOverlord.Editor
{
    public class EditorExtensions : MonoBehaviour
    {
        /* 
            NOTE(SpectatorQL): This guy may need to be changed frequently.
            Keep this in mind, and don't freak out if some weird errors
            appear in the console after building the player.
            IMPORTANT(SpectatorQL): This is non-portable!
        */
        [MenuItem("Overlord/Create default mod")]
        static void CreateDefaultMod()
        {
            var processInfo = new System.Diagnostics.ProcessStartInfo("copyTestResources.bat");
            var process = System.Diagnostics.Process.Start(processInfo);
            process.WaitForExit();


            string[] defaultResourceDirs =
            {
                "Textures/Walls/",
                "Textures/Floor/",
                "Textures/Ceiling/",
                "Textures/Skybox/",

                "Audio/",

                "Models/Classes/",
                "Models/Figures/",
            };
            System.Diagnostics.Debug.Assert(defaultResourceDirs.Length == (int)ResourceTypeID.Count);


            ModManifest defaultMod = new ModManifest
            {
                Name = "Default",
                RMMCode = ModFormatInfo.RMMCode,
                Version = ModFormatInfo.VERSION
            };

            ModData defaultModData = new ModData();

            int resourceTypeCount = (int)ResourceTypeID.Count;
            ResourceType[] resourceTypeEntries = new ResourceType[resourceTypeCount];
            for(int i = 0;
                i < resourceTypeCount;
                ++i)
            {
                resourceTypeEntries[i] = new ResourceType();
            }
            defaultModData.ResourceTypeEntries = resourceTypeEntries;

            string workingDirectory = PATH("build/Mods/Default/");
            int runningResourceCount = 0;
            List<Resource> resourcesToBuild = new List<Resource>();
            for(int i = 0;
                i < resourceTypeCount;
                ++i)
            {
                string resDir = PATH(workingDirectory + defaultResourceDirs[i]);
                string[] resourceFiles = Directory.GetFiles(resDir);
                Resource[] resources = new Resource[resourceFiles.Length];
                for(int j = 0;
                    j < resourceFiles.Length;
                    ++j)
                {
                    resources[j] = new Resource { File = resourceFiles[j] };
                }

                defaultModData.ResourceTypeEntries[i].FirstResourceIndex = runningResourceCount;
                for(int j = 0;
                    j < resources.Length;
                    ++j)
                {
                    resourcesToBuild.Add(resources[j]);
                    ++runningResourceCount;
                }
                defaultModData.ResourceTypeEntries[i].OnePastLastResourceIndex = runningResourceCount;
            }
            defaultModData.Resources = resourcesToBuild.ToArray();

            defaultMod.Data = defaultModData;

            string defaultModManifestPath = PATH("build/Mods/Default/Default.rmm");
            using(var fs = new FileStream(defaultModManifestPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, defaultMod);
            }
        }
    }
}
