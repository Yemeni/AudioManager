using System.IO;
using System.Reflection;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class MixerClassGenerator
    {
        private static string ClipLinePrefix = "        public const string";
        private static string MixerClassPath => AudioManagerEditorUtil.GetPathOfFile("mixer", "Utility/Mixer.cs");
        
        
        public static void Generate()
        {
            using (var file = new StreamWriter(MixerClassPath))
            {
                WriteHeader(file);

                var data = (AudioMixerGroup[]) GetMixerGroups(AssetAccessor.GetAsset<AudioLibrary>()).GetValue(AssetAccessor.GetAsset<AudioLibrary>());
                
                if (data.Length > 0)
                {
                    foreach (var mixer in data)
                        WriteLine(file, mixer);
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
            file.WriteLine("    public struct Mixer");
            file.WriteLine("    {");
        }
        
        
        private static void WriteLine(TextWriter file, AudioMixerGroup data)
        {
            file.WriteLine($"{ClipLinePrefix} {StructHandler.ParseFieldName(data.name)} = \"{data.name}\";");
        }
        
        
        private static FieldInfo GetMixerGroups(AudioLibrary lib)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return lib.GetType().GetField("mixers", flags);
        }
    }
}