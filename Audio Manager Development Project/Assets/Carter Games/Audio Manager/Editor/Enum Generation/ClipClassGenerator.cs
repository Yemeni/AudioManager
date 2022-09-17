using System.IO;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class ClipClassGenerator
    {
        private static string ClipLinePrefix = "        public const string";
        private static string ClipClassPath => AudioManagerEditorUtil.GetPathOfFile("clip", "Utility/Clip.cs");
        
        
        public static void Generate()
        {
            using (var file = new StreamWriter(ClipClassPath))
            {
                WriteHeader(file);

                if (AssetAccessor.GetAsset<AudioLibrary>().GetData.Length > 0)
                {
                    foreach (var data in AssetAccessor.GetAsset<AudioLibrary>().GetData)
                        WriteLine(file, data);
                }

                file.WriteLine("    }");
                file.WriteLine("}");
                file.Close();
            }
        }
        
        
        private static void WriteHeader(TextWriter file)
        {
            file.WriteLine("namespace CarterGames.Assets.AudioManager");
            file.WriteLine("{");
            file.WriteLine("    public struct Clip");
            file.WriteLine("    {");
        }
        
        
        private static void WriteLine(TextWriter file, AudioData data)
        {
            file.WriteLine($"{ClipLinePrefix} {StructHandler.ParseFieldName(data.key)} = \"{data.key}\";");
        }
    }
}