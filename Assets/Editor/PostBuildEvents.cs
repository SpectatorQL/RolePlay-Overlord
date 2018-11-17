using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RolePlayOverlord.Editor
{
    public class PostBuildEvents : MonoBehaviour
    {
        public static int OnePastLastSlash(string src)
        {
            int result = src.LastIndexOf('/') + 1;
            return result;
        }

        static void CreateTestDirectory(string dir)
        {
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
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
            StringBuilder buildDir = new StringBuilder();
            for(int i = 0;
                i < OnePastLastSlash(pathToBuiltProject);
                ++i)
            {
                if(pathToBuiltProject[i] == '/')
                {
                    buildDir.Append('\\');
                }
                else
                {
                    buildDir.Append(pathToBuiltProject[i]);
                }
            }

            string testDir = buildDir + "Test\\";
            CreateTestDirectory(testDir);

            DirectoryInfo projectDirInfo = new DirectoryInfo(Application.dataPath);
            // TODO(SpectatorQL): Change this to look in specific directories in the project.
            string[] testExtensions = { ".png", ".txt" };
            var testFiles = projectDirInfo.GetFiles()
                .Where(file =>
                {
                    return testExtensions.Contains(file.Extension);
                })
                .ToArray();

            for(int i = 0;
                i < testFiles.Length;
                ++i)
            {
                string assetFileName = testFiles[i].Name;
                string destFileName = testDir + assetFileName;
                File.Copy(testFiles[i].FullName, destFileName, true);
            }
        }
    }
}
