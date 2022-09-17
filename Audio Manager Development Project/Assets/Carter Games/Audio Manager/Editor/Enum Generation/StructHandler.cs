using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    /// <summary>
    /// Handles the creation/editing of the struct classes use as helper classes for the asset...
    /// </summary>
    public static class StructHandler
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
        
        
        public static void WriteHeader(TextWriter file, string structName)
        {
            file.WriteLine("namespace CarterGames.Assets.AudioManager");
            file.WriteLine("{");
            file.WriteLine($"    public struct {structName}");
            file.WriteLine("    {");
        }
        
        
        public static void WriteFooter(TextWriter file)
        {
            file.WriteLine("    }");
            file.WriteLine("}");
            file.Close();
        }
        
        
        public static void WriteLine(TextWriter file, string prefix, string fieldName, string data)
        {
            file.WriteLine($"{prefix} {ParseFieldName(fieldName)} = {data};");
        }
        
        
        public static string ParseFieldName(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9_]", "");
        }
    }
}