using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class GlobalAudioManagerSettings
    {
        private static SerializedObject settings;
        private static Color DefaultGUIColor;


        [SettingsProvider]
        public static SettingsProvider AudioManagerSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/Carter Games/Audio Manager", SettingsScope.User)
            {
                guiHandler = (searchContext) =>
                {
                    DefaultGUIColor = GUI.color;
                    
                    DrawHeaderSection();
                    
                    if (settings == null)
                        settings = new SerializedObject(AmEditorUtils.Settings);

                    GUILayout.Space(7.5f);
                    DrawAssetMetaData();
                    
                    GUILayout.Space(2.5f);
                    DrawAudioPrefabOptions();
                    
                    GUILayout.Space(2.5f);
                    DrawOptionalSettings();
                    
                    
                    if (settings.targetObject != null)
                    {
                        settings.ApplyModifiedProperties();
                        settings.Update();
                    }
                    else
                    {
                        settings = new SerializedObject(AmEditorUtils.Settings);
                    }
                    
                    GUILayout.Space(2.5f);
                    DrawButtons();
                },
                
                keywords = new HashSet<string>(new[] { "Carter Games", "External Assets", "Tools", "Audio", "Audio Manager", "Sound Manager", "Music", "Sound" })
            };

            return provider;
        }


        private static void DrawHeaderSection()
        {
            var managerHeader = AmEditorUtils.ManagerHeader;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                    
            if (managerHeader)
            {
                if (GUILayout.Button(managerHeader, GUIStyle.none, GUILayout.MaxHeight(110)))
                {
                    GUI.FocusControl(null);
                }
            }
                    
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }


        private static void DrawAssetMetaData()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
                    
            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;
         
            EditorGUILayout.LabelField(new GUIContent("Version"), new GUIContent(settings.FindProperty("version").stringValue));
            EditorGUILayout.LabelField(new GUIContent("Release Date"), new GUIContent(settings.FindProperty("releaseDate").stringValue));

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        private static void DrawAudioPrefabOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;
            
            EditorGUILayout.PropertyField(settings.FindProperty("audioPrefab"));
            EditorGUILayout.PropertyField(settings.FindProperty("clipAudioMixer"));
                    
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
                    
            GUILayout.Space(2.5f);
                    
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
                    
            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Music", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;
            
            EditorGUILayout.PropertyField(settings.FindProperty("musicPrefab"));
            EditorGUILayout.PropertyField(settings.FindProperty("musicAudioMixer"));
                    
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }



        private static void DrawOptionalSettings()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Optional Settings", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;

            EditorGUILayout.PropertyField(settings.FindProperty("showDebugMessages"));
            EditorGUILayout.PropertyField(settings.FindProperty("additionalMixerGroups"));

            settings.FindProperty("showVariance").boolValue =
                EditorGUILayout.Foldout(settings.FindProperty("showVariance").boolValue, "Variance Options");

            if (settings.FindProperty("showVariance").boolValue)
            {
                #region Volume Controls

                EditorGUI.indentLevel++;
                
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Volume", EditorStyles.boldLabel, GUILayout.Width(65));
                EditorGUILayout.BeginHorizontal();

                var minVolValue = settings.FindProperty("minVolumeVariance").floatValue;
                var maxVolValue = settings.FindProperty("maxVolumeVariance").floatValue;

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("minVolumeVariance"), GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                    minVolValue = settings.FindProperty("minVolumeVariance").floatValue;

                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider(GUIContent.none, ref minVolValue, ref maxVolValue, -1f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    settings.FindProperty("minVolumeVariance").floatValue = minVolValue;
                    settings.FindProperty("maxVolumeVariance").floatValue = maxVolValue;
                    settings.ApplyModifiedProperties();
                    settings.Update();
                }

                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("maxVolumeVariance"), GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                    maxVolValue = settings.FindProperty("maxVolumeVariance").floatValue;

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();

                #endregion

                #region Pitch Controls

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Pitch", EditorStyles.boldLabel, GUILayout.Width(65));
                EditorGUILayout.BeginHorizontal();

                var minPitchValue = settings.FindProperty("minPitchVariance").floatValue;
                var maxPitchValue = settings.FindProperty("maxPitchVariance").floatValue;

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("minPitchVariance"), GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    minPitchValue = settings.FindProperty("minPitchVariance").floatValue;
                    settings.ApplyModifiedProperties();
                    settings.Update();
                }

                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider(GUIContent.none, ref minPitchValue, ref maxPitchValue, -1f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    settings.FindProperty("minPitchVariance").floatValue = minPitchValue;
                    settings.FindProperty("maxPitchVariance").floatValue = maxPitchValue;
                    settings.ApplyModifiedProperties();
                    settings.Update();
                }

                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("maxPitchVariance"), GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    maxPitchValue = settings.FindProperty("maxPitchVariance").floatValue;
                    settings.ApplyModifiedProperties();
                    settings.Update();
                }

                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                
                EditorGUI.indentLevel--;
                #endregion
            }

            settings.FindProperty("showEditorOptions").boolValue =
                EditorGUILayout.Foldout(settings.FindProperty("showEditorOptions").boolValue, "Editor Options");
            
            if (settings.FindProperty("showEditorOptions").boolValue)
            {
                #region Number Of Items Per Library Page

                EditorGUI.indentLevel++;
                
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.PropertyField(settings.FindProperty("numberPerPageInEditor"), new GUIContent("# Per Page", "The number of items to show per page in the library editor."));
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;

                #endregion
            }
            
            GUILayout.Space(2.55f);
            EditorGUILayout.EndVertical();
        }



        private static void DrawButtons()
        {
            var _rect = GUILayoutUtility.GetLastRect();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Asset Store", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://assetstore.unity.com/publishers/43356");

            if (GUILayout.Button("Documentation", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/audiomanager/docs");

            if (GUILayout.Button("Change Log", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/audiomanager/changelog");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Email", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("mailto:support@carter.games?subject=I need help with the Audio Manager");

            if (GUILayout.Button("Discord", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/discord");

            if (GUILayout.Button("Report Issues", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/report");

            EditorGUILayout.EndHorizontal();

            var carterGamesBanner = AmEditorUtils.CarterGamesBanner;

            if (carterGamesBanner != null)
            {
                GUI.contentColor = new Color(1, 1, 1, .75f);

                if (GUILayout.Button(carterGamesBanner, GUILayout.MaxHeight(40)))
                    Application.OpenURL("https://carter.games");

                GUI.contentColor = DefaultGUIColor;
            }
            else
            {
                if (GUILayout.Button("Carter Games", GUILayout.MaxHeight(40)))
                    Application.OpenURL("https://carter.games");
            }
        }
    }
}