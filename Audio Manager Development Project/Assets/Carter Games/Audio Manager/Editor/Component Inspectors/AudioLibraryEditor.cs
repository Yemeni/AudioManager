using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    [CustomEditor(typeof(AudioLibrary))]
    public class AudioLibraryEditor : UnityEditor.Editor
    {
        private AudioLibrary library;
        private SerializedObject libraryObject;
        private SerializedProperty tabPos;
        private Vector2 scrollRect;
        
        private static Color DefaultGUIBackground;

        private void OnEnable()
        {
            library = target as AudioLibrary;
            libraryObject = new SerializedObject(library);
            tabPos = libraryObject.FindProperty("editorTabPos");
            DefaultGUIBackground = GUI.backgroundColor;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(2f);
            
            EditorGUI.BeginChangeCheck();
            tabPos.intValue = GUILayout.Toolbar(tabPos.intValue, new[] { "Library", "Groups" });

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            
            switch (tabPos.intValue)
            {
                case 0:
                    DrawLibrary();
                    break;
                case 1:
                    DrawGroups();
                    break;
            }
            
            EditorGUILayout.EndScrollView();

            base.OnInspectorGUI();
            
            if (!EditorGUI.EndChangeCheck()) return;
            libraryObject.ApplyModifiedProperties();
            libraryObject.Update();
        }


        private void DrawLibrary()
        {
            EditorGUILayout.LabelField($"Total Clips Found: {library.ClipCount}");

            EditorGUILayout.Space(2f);
            
            EditorGUILayout.BeginHorizontal();
            
            GUI.backgroundColor = AudioManagerEditorUtil.Green;
            if (GUILayout.Button("Edit Additional Options"))
            {
                EditorWindowMain.ShowWindowOnTab(0);
            }
            GUI.backgroundColor = DefaultGUIBackground;
            
            GUI.backgroundColor = AudioManagerEditorUtil.Yellow;
            if (GUILayout.Button("Perform Manual Scan"))
            {
                AudioScanner.ManualScan();
            }
            GUI.backgroundColor = DefaultGUIBackground;
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(2f);
            EditorGUILayout.LabelField("Library", EditorStyles.boldLabel);
            
            GUI.enabled = false;
            EditorGUILayout.BeginVertical();
            
            if (library.GetData == null)
                AudioScanner.ManualScan();

            for (var i = 0; i < library.GetData.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.TextField(library.GetData[i].key);
                EditorGUILayout.ObjectField(library.GetData[i].value, typeof(AudioClip), false);

                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            GUI.enabled = true;
        }
        
        
        private void DrawGroups()
        {
            EditorGUILayout.LabelField($"Total Groups: {library.Groups?.Count ?? 0}");
            
            EditorGUILayout.Space(2f);
            
            GUI.backgroundColor = AudioManagerEditorUtil.Green;
            if (GUILayout.Button("Edit Groups"))
            {
                EditorWindowMain.ShowWindowOnTab(1);
            }
            GUI.backgroundColor = DefaultGUIBackground;
            
            GUI.enabled = false;
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(2f);
            
            if (library.Groups == null)
                library.Groups = new List<GroupData>();
            
            for (var i = 0; i < library.Groups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(library.Groups[i].GroupName);
                
                EditorGUILayout.BeginVertical();

                for (var j = 0; j < library.Groups[i].Clips.Count; j++)
                {
                    EditorGUILayout.TextField(library.Groups[i].Clips[j]);
                }
                
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            GUI.enabled = true;
        }
    }
}