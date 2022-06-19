using System.Text.RegularExpressions;
using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class EnumHandler
    {
        public static void RefreshClips()
        {
            ClipClassGenerator.Generate();
            AssetDatabase.Refresh();
        }


        public static void RefreshGroups()
        {
            GroupClassGenerator.Generate();
            AssetDatabase.Refresh();
        }
        
        
        public static void RefreshMixers()
        {
            MixerClassGenerator.Generate();
            AssetDatabase.Refresh();
        }
        
        
        public static string ParseFieldName(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9_]", "");
        }
    }
}