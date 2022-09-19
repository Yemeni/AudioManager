using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class DirectorySelectHelper
    {
        public static List<string> GetAllDirectories()
        {
            var list = new List<string>();
            list.Add("");
            list.Add("Assets");
            list.AddRange(Directory.GetDirectories("Assets", "*", SearchOption.AllDirectories));

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Replace(@"\", "/");
            }

            return list;
        }

        public static string ConvertIntToDir(int value)
        {
            return GetAllDirectories()[value];
        }

        public static int ConvertStringToIndex(string value)
        {
            return GetAllDirectories().IndexOf(value);
        }


        public static List<string> GetDirectoriesFromBase()
        {
            var list = new List<string>();
            list.Add("");
            list.Add(AudioManagerEditorUtil.Settings.baseAudioScanPath);
            list.AddRange(Directory.GetDirectories(AudioManagerEditorUtil.Settings.baseAudioScanPath, "*", SearchOption.AllDirectories));

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Replace(@"\", "/");
            }

            return list;
        }
        
        public static string ConvertIntToDir(int value, List<string> options)
        {
            return options[value];
        }

        public static int ConvertStringToIndex(string value, List<string> options)
        {
            return options.IndexOf(value);
        }
    }
}