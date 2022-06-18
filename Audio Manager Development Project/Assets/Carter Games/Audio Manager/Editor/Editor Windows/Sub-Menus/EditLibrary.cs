// ----------------------------------------------------------------------------
// EditLibrary.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 10/06/2022
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class EditLibrary
    {
        private static AudioLibrary Library = (AudioLibrary)AmEditorUtils.GetFile<AudioLibrary>("t:audiolibrary");
        private static AudioManagerSettings Settings = (AudioManagerSettings)AmEditorUtils.GetFile<AudioManagerSettings>("t:audiomanagersettings");
        private static SerializedObject SettingsObject = new SerializedObject(Settings);
        private static Vector2 ScrollRect;
        private static bool HasMadeChanges;
        private static int PageNumber = 1;
        private static int TotalPages = -1;
        
        private static Color DefaultGUIBackground;
        
        
        public static void DrawLibrary()
        {
            Initialise();
            DrawHeaderHelp();
            DrawLibraryRows();
        }


        private static void Initialise()
        {
            if (TotalPages.Equals(-1))
                TotalPages = Mathf.CeilToInt(Library.ClipCount / (float) SettingsObject.FindProperty("numberPerPageInEditor").intValue);

            DefaultGUIBackground = GUI.backgroundColor;
        }
        

        private static void DrawHeaderHelp()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Edit some additional options for each clip in the library here", MessageType.Info);
            EditorGUILayout.Space();
        }


        private static void DrawLibraryRows()
        {
            var data = Library.GetData;
            
            for (var i = (PageNumber - 1) * SettingsObject.FindProperty("numberPerPageInEditor").intValue; i < (PageNumber) * SettingsObject.FindProperty("numberPerPageInEditor").intValue; i++)
            {
                EditorGUILayout.Space(2f);
                if (i > data.Length - 1)
                {
                    EditorGUILayout.Space(18f);
                    continue;
                }
                
                DrawLibraryRow(data[i], i.Equals((PageNumber - 1) * SettingsObject.FindProperty("numberPerPageInEditor").intValue));
            }

            DrawLibraryPageButtons();
        }


        private static void DrawLibraryPageButtons()
        {
            if (TotalPages <= 1) return;
            
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("<", GUILayout.Width(" < ".Width())))
            {
                if (PageNumber.Equals(1)) return;
                PageNumber--;
            }

            if (PageNumber > 1)
            {
                if (GUILayout.Button((PageNumber - 1).ToString(), GUILayout.Width((PageNumber - 1).ToString().Width() + 7.5f)))
                    PageNumber--;
            }

            GUI.backgroundColor = AmEditorUtils.Green;
            if (GUILayout.Button(PageNumber.ToString(), GUILayout.Width(PageNumber.ToString().Width() + 7.5f)))
            {
            }
            GUI.backgroundColor = DefaultGUIBackground;
            
            if (PageNumber < TotalPages)
            {
                if (GUILayout.Button((PageNumber + 1).ToString(), GUILayout.Width((PageNumber + 1).ToString().Width() + 7.5f)))
                    PageNumber++;
            }
            
            if (GUILayout.Button(">", GUILayout.Width(" > ".Width())))
            {
                if (PageNumber.Equals(TotalPages)) return;
                PageNumber++;
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        
        
        
        private static void DrawLibraryRow(AudioData data, bool first)
        {
            EditorGUILayout.BeginHorizontal();

            if (first)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Clip", EditorStyles.boldLabel, GUILayout.Width("Clip".Width()));
            }

            GUI.backgroundColor = AmEditorUtils.Red;
            EditorGUILayout.TextField(data.key);
            GUI.backgroundColor = DefaultGUIBackground;
            
            if (first)
                EditorGUILayout.EndVertical();
            
            if (first)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("", GUILayout.Width(0));
            }
            
            GUI.enabled = false;
            GUI.backgroundColor = AmEditorUtils.Red;
            EditorGUILayout.ObjectField(data.value, typeof(AudioClip), false);
            GUI.backgroundColor = DefaultGUIBackground;
            GUI.enabled = true;
            
            EditorGUI.BeginChangeCheck();
            
            if (first)
                EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            
            if (first)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Volume", EditorStyles.boldLabel, GUILayout.Width("Volume".Width()));
            }
            
            data.baseVolume = EditorGUILayout.Slider(data.baseVolume, 0, 1);
            
            if (first)
                EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            
            if (first)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Pitch", EditorStyles.boldLabel, GUILayout.Width("Pitch".Width()));
            }
            data.basePitch = EditorGUILayout.Slider(data.basePitch, -3, 3);
            if (first)
                EditorGUILayout.EndVertical();


            if (EditorGUI.EndChangeCheck())
            {
                var obj = new SerializedObject(Library);
                obj.ApplyModifiedProperties();
                obj.Update();
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}