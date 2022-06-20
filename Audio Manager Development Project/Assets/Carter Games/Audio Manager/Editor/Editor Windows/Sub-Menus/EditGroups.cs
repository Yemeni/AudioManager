// ----------------------------------------------------------------------------
// EditGroups.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 10/06/2022
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class EditGroups
    {
        private static readonly AudioLibrary library = AssetAccessor.GetAsset<AudioLibrary>();
        private static Vector2 scrollRect;
        private static bool hasMadeChanges;
        

        public static void DrawAllGroups()
        {
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "Edit or create new groups of clips here, you can change these at any time. When pressing \"Apply Changes\" or closing the window, the groups will update.",
                MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create New Group"))
            {
                library.Groups?.Add(new GroupData("New Clip Group"));

                if (library.Groups != null && DoesGroupNameExist(library.Groups[library.Groups.Count - 1].GroupName,
                        false, out var total))
                    library.Groups[library.Groups.Count - 1].GroupName += $"({total - 1})";

                hasMadeChanges = true;
            }

            if (hasMadeChanges)
            {
                if (GUILayout.Button("Apply Changes"))
                {
                    EnumHandler.RefreshGroups();
                    hasMadeChanges = false;
                }
            }

            EditorGUILayout.EndHorizontal();

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            EditorGUILayout.BeginVertical("box");
            

            for (var i = 0; i < library.Groups.Count; i++)
            {
                DrawGroup(library.Groups[i], i);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }


        private static void DrawGroup(GroupData data, int index)
        {
            var groupName = $"{data.GroupName} ({data.Clips.Count} Clips)";

            EditorGUILayout.BeginHorizontal();
            data.EditorDropDownState = EditorGUILayout.Foldout(data.EditorDropDownState, groupName);
            
            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                library.RemoveGroup(data);
                EnumHandler.RefreshGroups();
                hasMadeChanges = true;
                return;
            }

            EditorGUILayout.EndHorizontal();

            
            if (data.EditorDropDownState)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical("box");

                data.GroupName = EditorGUILayout.TextField(
                    new GUIContent("Group Name:",
                        "The name to refer to this group as, it cannot match another group name."), data.GroupName);

                // Avoids dup names...
                if (DoesGroupNameExist(data.GroupName, true, out var total))
                    data.GroupName += $"({total - 1})";

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Clips:");
                
                for (var i = 0; i < data.Clips.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    data.Clips[i] = GetClipFromIndex(EditorGUILayout.Popup(GetIndexOfClip(data.Clips[i]), library.AllClipNames));
                    
                    if (GUILayout.Button("-", GUILayout.Width(20f)))
                    {
                        data.Clips.RemoveAt(i);
                        hasMadeChanges = true;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add New Clip"))
                {
                    data.Clips.Add(library.AllClipNames[0]);
                    hasMadeChanges = true;
                }

                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
        }


        private static int GetIndexOfClip(string clipName)
        {
            return library.GetData.ToList().FindIndex(t => t.key.Equals(clipName));
        }
        
        
        private static string GetClipFromIndex(int index)
        {
            return library.GetData[index].key;
        }


        private static bool DoesGroupNameExist(string name, bool allowClose, out int total)
        {
            total = allowClose 
                ? library.Groups.Count(t => t.GroupName.Equals(name)) 
                : library.Groups.Count(t => t.GroupName.Equals(name) || t.GroupName.Contains(name));

            return total > 1;
        }


        public static void GetGroupsInFile()
        {
            var fields = typeof(Group).GetFields(BindingFlags.Static | BindingFlags.Public);
            var groupNames = new List<string>();
            var groupValues = new List<string[]>();

            for (var i = 0; i < fields.Length; i++)
            {
                groupNames.Add(fields[i].Name);
                groupValues.Add(fields[i].GetValue(fields[i]) as string[]);
            }
            
            if (library.Groups.Count > 0)
            {
                for (var i = 0; i < library.Groups.Count; i++)
                {
                    for (var j = 0; j < groupNames.Count; j++)
                    {
                        if (library.Groups.Any(t => t.GroupName.Equals(groupNames[j]))) continue;
                        library.Groups.Add(new GroupData(groupNames[j], groupValues[j]));
                    }
                }
            }
            else
            {
                for (var i = 0; i < groupNames.Count; i++)
                {
                    library.Groups.Add(new GroupData(groupNames[i], groupValues[i]));
                }
            }
        }
    }
}