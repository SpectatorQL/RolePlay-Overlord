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
        static void CreateDefaultDataDirectory()
        {
            foreach(var d in _defaultDataDirectories)
            {
                string path = PATH(d);
                Directory.CreateDirectory(path);
                UnityEngine.Debug.Assert(Directory.Exists(path));
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
            int onePastLastSlash = pathToBuiltProject.OnePastLastSlash();
            string buildDir = pathToBuiltProject.Remove(onePastLastSlash, pathToBuiltProject.Length - onePastLastSlash);
            
            CreateDefaultDataDirectory();

            var defaultMod = Mod.Create();
            defaultMod.Name = "Default";

            for(int i = 0;
                i < _defaultDataDirectories.Length;
                ++i)
            {
                string dir = "Assets/" + _defaultDataDirectories[i];
                string[] files = Directory.GetFiles(PATH(dir))
                    .Where(file =>
                    {
                        return EXTENSION(file) != ".meta";
                    })
                    .ToArray();

                defaultMod.Resources[i] = new string[files.Length];

                for(int j = 0;
                    j < files.Length;
                    ++j)
                {
                    string assetFile = Path.GetFileName(files[j]);
                    string srcFile = dir + assetFile;
                    string destFile = buildDir + _defaultDataDirectories[i] + assetFile;

                    File.Copy(PATH(srcFile), PATH(destFile), true);
                    UnityEngine.Debug.Assert(File.Exists(PATH(destFile)));

                     defaultMod.Resources[i][j] = _defaultDataDirectories[i] + assetFile;
                }
            }

            string defaultModManifest = "build/Mods/Default/Default.rmm";
            using(var fs = new FileStream(PATH(defaultModManifest), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, defaultMod);
            }
        }
    }
}
