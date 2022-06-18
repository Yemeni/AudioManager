using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


namespace CarterGames.Assets.AudioManager.Editor
{
    public static class EnumHandler
    {
        private static string ClipLinePrefix = "        public const string";
        private static string GroupLinePrefix = "        public static readonly string[]";
        private static GroupData[] groups;
        private static List<string> groupsGenerated = new List<string>();


        [MenuItem("Tools/Audio Manager | CG/Perform Manual Refresh")]
        public static void ManualScan()
        {
            RefreshGroups();
        }
        

        private static string ClipClassPath => AmEditorUtils.GetPathOfFile("clip", "Utility/Clip.cs");
        private static string GroupClassPath => AmEditorUtils.GetPathOfFile("group", "Utility/Group.cs");
        private static string MixerClassPath => AmEditorUtils.GetPathOfFile("mixer", "Utility/Mixer.cs");


        private static GroupData[] GroupsInProject
        {
            get
            {
                var data = (AudioLibrary) AmEditorUtils.GetFile<AudioLibrary>("t:audiolibrary");
                groups = new GroupData[data.Groups.Count];
        
                for (var i = 0; i < groups.Length; i++)
                    groups[i] = data.Groups[i];

                return groups;
            }
        }


        public static void RefreshClips()
        {
            WriteClipClass();
            AssetDatabase.Refresh();
        }


        public static void RefreshGroups()
        {
            WriteGroupClass();
            AssetDatabase.Refresh();
        }
        
        
        public static void RefreshMixers()
        {
            WriteMixerClass();
            AssetDatabase.Refresh();
        }
        

        private static void WriteClipClass()
        {
            using (var file = new StreamWriter(ClipClassPath))
            {
                WriteCipClassHeader(file);

                if (AudioScanner.Library.GetData.Length > 0)
                {
                    foreach (var data in AudioScanner.Library.GetData)
                        WriteClipLine(file, data);
                }

                file.WriteLine("    }");
                file.WriteLine("}");
                file.Close();
            }
        }

        
        private static void WriteGroupClass()
        {
            using (var file = new StreamWriter(GroupClassPath))
            {
                WriteGroupClassHeader(file);

                var groupsInProject = GroupsInProject;
                groupsGenerated.Clear();
            
                if (groupsInProject.Length > 0)
                {
                    foreach (var data in groupsInProject)
                    {
                        var parsedName = ParseFieldName(data.GroupName);

                        if (groupsGenerated.Count > 0)
                        {
                            for (var i = 0; i < groupsGenerated.Count; i++)
                            {
                                if (groupsGenerated[i].Equals(parsedName))
                                {
                                    if (AmEditorUtils.Settings.ShowDebugMessages)
                                        AmLog.Warning($"Couldn't add <i>\"{parsedName}\"</i> to Groups as a group of the name already exists");

                                    continue;
                                }
                                
                                WriteGroupLine(file, data);
                                groupsGenerated.Add(parsedName);
                            }
                        }
                        else
                        {
                            WriteGroupLine(file, data);
                            groupsGenerated.Add(parsedName);
                        }
                    }
                }
            
                file.WriteLine("    }");
                file.WriteLine("}");
                file.Close();
            }
        }
        
        
        private static void WriteMixerClass()
        {
            using (var file = new StreamWriter(MixerClassPath))
            {
                WriteMixerClassHeader(file);

                var data = (AudioMixerGroup[]) GetMixerGroups(AudioScanner.Library).GetValue(AudioScanner.Library);
                
                if (data.Length > 0)
                {
                    foreach (var mixer in data)
                        WriteMixerLine(file, mixer);
                }

                file.WriteLine("    }");
                file.WriteLine("}");
                file.Close();
            }
        }


        private static void WriteCipClassHeader(TextWriter file)
        {
            file.WriteLine("namespace CarterGames.Assets.AudioManager");
            file.WriteLine("{");
            file.WriteLine("    public struct Clip");
            file.WriteLine("    {");
        }
        
        private static void WriteGroupClassHeader(TextWriter file)
        {
            file.WriteLine("namespace CarterGames.Assets.AudioManager");
            file.WriteLine("{");
            file.WriteLine("    public struct Group");
            file.WriteLine("    {");
        }
        
        private static void WriteMixerClassHeader(TextWriter file)
        {
            file.WriteLine("namespace CarterGames.Assets.AudioManager");
            file.WriteLine("{");
            file.WriteLine("    public struct Mixer");
            file.WriteLine("    {");
        }


        private static void WriteClipLine(TextWriter file, AudioData data)
        {
            file.WriteLine($"{ClipLinePrefix} {ParseFieldName(data.key)} = \"{data.key}\";");
        }
        
        private static void WriteGroupLine(TextWriter file, GroupData data)
        {
            file.WriteLine($"{GroupLinePrefix} {ParseFieldName(data.GroupName)} = {GetListInLine(data.Clips)};");
        }
        
        private static void WriteMixerLine(TextWriter file, AudioMixerGroup data)
        {
            file.WriteLine($"{ClipLinePrefix} {ParseFieldName(data.name)} = \"{data.name}\";");
        }

        private static string GetListInLine(List<string> toParse)
        {
            var line = string.Empty;
            line = "{";

            for (var i = 0; i < toParse.Count; i++)
            {
                if (i.Equals(0))
                    line += $"\"{toParse[i]}\"";
                else
                    line += $",\"{toParse[i]}\"";
            }

            line += "}";
            return line;
        }


        private static string ParseFieldName(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9_]", "");
        }

        private static FieldInfo GetMixerGroups(AudioLibrary lib)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return lib.GetType().GetField("mixers", flags);
        }
    }
}