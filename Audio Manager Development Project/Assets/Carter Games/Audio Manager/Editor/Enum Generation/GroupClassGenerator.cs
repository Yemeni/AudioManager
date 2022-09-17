using System.Collections.Generic;
using System.IO;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class GroupClassGenerator
    {
        private static string GroupLinePrefix = "        public static readonly string[]";
        private static GroupData[] groups;
        private static List<string> groupsGenerated = new List<string>();
        
        
        private static string GroupClassPath => AudioManagerEditorUtil.GetPathOfFile("group", "Utility/Group.cs");
        
        
        private static GroupData[] GroupsInProject
        {
            get
            {
                var data = LibraryAssetEditorUtil.AudioLibraryAsset;
                groups = new GroupData[data.Groups.Count];
        
                for (var i = 0; i < groups.Length; i++)
                    groups[i] = data.Groups[i];

                return groups;
            }
        }
        
        
        public static void Generate()
        {
            using (var file = new StreamWriter(GroupClassPath))
            {
                StructHandler.WriteHeader(file, "Group");

                var groupsInProject = GroupsInProject;
                groupsGenerated.Clear();
            
                if (groupsInProject.Length > 0)
                {
                    foreach (var data in groupsInProject)
                    {
                        var parsedName = StructHandler.ParseFieldName(data.GroupName);

                        if (groupsGenerated.Count > 0)
                        {
                            for (var i = 0; i < groupsGenerated.Count; i++)
                            {
                                if (groupsGenerated[i].Equals(parsedName))
                                {
                                    if (AudioManagerEditorUtil.Settings.ShowDebugMessages)
                                        AmLog.Warning($"Couldn't add <i>\"{parsedName}\"</i> to Groups as a group of the name already exists");

                                    continue;
                                }
                                
                                StructHandler.WriteLine(file, GroupLinePrefix, data.GroupName, GetListInLine(data.Clips));
                                groupsGenerated.Add(parsedName);
                            }
                        }
                        else
                        {
                            StructHandler.WriteLine(file, GroupLinePrefix, data.GroupName, GetListInLine(data.Clips));
                            groupsGenerated.Add(parsedName);
                        }
                    }
                }
                
                StructHandler.WriteFooter(file);
            }
        }


        private static string GetListInLine(IReadOnlyList<string> toParse)
        {
            var line = "{";

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
    }
}