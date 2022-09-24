/*
 * 
 *  Audio Manager
 *							  
 *	Audio Manager File Editor
 *      The editor script for the Audio Manager Files, handles the custom inspector for any Audio Manager File.
 *
 *  Warning:
 *	    Please refrain from editing this script as it will cause issues to the assets...
 *			
 *  Written by:
 *      Jonathan Carter
 *
 *  Published By:
 *      Carter Games
 *      E: hello@carter.games
 *      W: https://www.carter.games
 *		
 *  Version: 2.5.8
 *	Last Updated: 18/06/2022 (d/m/y)							
 * 
 */

using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    [CustomEditor(typeof(AudioManagerFile))]
    public class AudioManagerFileEditor : UnityEditor.Editor
    {
        private readonly Color32 amRedCol = new Color32(255, 150, 157, 255);
        private readonly string[] TabTitles = new string[2] {"Settings", "Library"};
        
        private SerializedProperty audioPrefab;
        private SerializedProperty isPopulated;
        private SerializedProperty audioMixers;
        private SerializedProperty directories;
        private SerializedProperty library;
        private SerializedProperty tabPos;
        
        private Color defaultContentCol;
        private Color normalBackgroundCol;
        
        
        private void OnEnable()
        {
            RefreshReferences();
        }


        private void RefreshReferences()
        {
            audioPrefab = serializedObject.FindProperty("soundPrefab");
            isPopulated = serializedObject.FindProperty("isPopulated");
            audioMixers = serializedObject.FindProperty("audioMixer");
            directories = serializedObject.FindProperty("directory");
            library = serializedObject.FindProperty("library");
            tabPos = serializedObject.FindProperty("tabPos");
            defaultContentCol = GUI.contentColor;
            normalBackgroundCol = GUI.backgroundColor;
        }


        public override void OnInspectorGUI()
        {
            AudioManagerEditorUtil.Header("Audio Manager File");
            DrawScriptSection();
            RenderTabBar();
            RenderSettings(tabPos.intValue);
            RenderLibrary(tabPos.intValue);

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }


        private void RenderTabBar()
        {
            GUILayout.Space(5f);
            tabPos.intValue = GUILayout.Toolbar(tabPos.intValue, TabTitles);
            GUILayout.Space(5f);
        }

        
        private void DrawScriptSection()
        {
            GUILayout.Space(4.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(target as AudioManagerFile), typeof(AudioManagerFile), false);
            GUI.enabled = true;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
        

        private void RenderSettings(int pos)
        {
            if (!pos.Equals(0)) return;

            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("File Settings", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
            
            EditorGUILayout.PropertyField(audioPrefab, new GUIContent("Sound Prefab:"));
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(isPopulated);
            GUI.enabled = true;
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
            
            //
            //
            //
            //
            //
            
            GUILayout.Space(3.5f);
            
            //
            //
            //
            //
            //
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mixers", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();

            if (audioMixers.arraySize.Equals(0))
            {
                audioMixers.InsertArrayElementAtIndex(0);
                return;
            }
            
            for (var i = 0; i < audioMixers.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"#{(i+1).ToString()}", GUILayout.Width(TextWidth($"#{(i+1).ToString()} ")));
                EditorGUILayout.PropertyField(audioMixers.GetArrayElementAtIndex(i), GUIContent.none);

                GUI.backgroundColor = AudioManagerEditorUtil.Green;
                
                if (GUILayout.Button("+", GUILayout.Width(25f)))
                    audioMixers.InsertArrayElementAtIndex(i + 1);
                
                GUI.backgroundColor = AudioManagerEditorUtil.Red;

                if (GUILayout.Button("-", GUILayout.Width(25f)))
                    audioMixers.DeleteArrayElementAtIndex(i);

                GUI.backgroundColor = normalBackgroundCol;
                
                EditorGUILayout.EndHorizontal();
            }    
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
            
            //
            //
            //
            //
            //
            
            GUILayout.Space(3.5f);
            
            //
            //
            //
            //
            //
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            GUI.contentColor = amRedCol;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Directories", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = defaultContentCol;
            
            if (directories.arraySize.Equals(0))
            {
                if (GUILayout.Button("Add First Element"))
                {
                    directories.InsertArrayElementAtIndex(0);
                }
                
                return;
            }

            var options = DirectorySelectHelper.GetDirectoriesFromBase(false);
            
            for (var i = 0; i < directories.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"#{(i+1).ToString()}", GUILayout.Width(TextWidth($"#{(i+1).ToString()} ")));
                directories.GetArrayElementAtIndex(i).stringValue = DirectorySelectHelper.ConvertIntToDir(EditorGUILayout.Popup(DirectorySelectHelper.ConvertStringToIndex(directories.GetArrayElementAtIndex(i).stringValue, options), options.ToArray()), options);
                
                GUI.backgroundColor = AudioManagerEditorUtil.Green;
                
                if (GUILayout.Button("+", GUILayout.Width(25f)))
                    directories.InsertArrayElementAtIndex(i + 1);

                GUI.backgroundColor = AudioManagerEditorUtil.Red;
                
                if (GUILayout.Button("-", GUILayout.Width(25f)))
                    directories.DeleteArrayElementAtIndex(i);

                GUI.backgroundColor = normalBackgroundCol;
                
                EditorGUILayout.EndHorizontal();
            }
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        private void RenderLibrary(int pos)
        {
            if (!pos.Equals(1)) return;
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Library"))
            {
                library.ClearArray();
            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
            
            //
            //
            //
            //
            //
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio Library", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Shows all the clips that are present in the library of this file.", MessageType.None);

            GUILayout.Space(5f);
            
            if (library.arraySize.Equals(0)) return;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Clip Name", EditorStyles.boldLabel, GUILayout.Width(Screen.width / 2.25f));
            EditorGUILayout.LabelField("Audio Clip File", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            GUI.enabled = false;
            
            for (var i = 0; i < library.arraySize; i++)
            {
                var _fileName = library.GetArrayElementAtIndex(i).FindPropertyRelative("key");
                var _fileClip = library.GetArrayElementAtIndex(i).FindPropertyRelative("value");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_fileName, GUIContent.none, GUILayout.Width(Screen.width / 2.25f));
                
                EditorGUILayout.PropertyField(_fileClip, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            
            GUI.enabled = true;
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        private static float TextWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
    }
}