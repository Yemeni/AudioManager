using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    [CustomEditor(typeof(AudioManagerSettings))]
    public class AudioManagerSettingsEditor : UnityEditor.Editor
    {
        private SerializedProperty versionNumberProp;
        private SerializedProperty releaseDateProp;
        
        private SerializedProperty audioPrefabProp;
        private SerializedProperty clipAudioMixerProp;
        private SerializedProperty musicPrefabProp;
        private SerializedProperty musicAudioMixerProp;
        
        private SerializedProperty debugProp;
        private SerializedProperty additionalMixersProp;
        
        private static Color DefaultGUIColor;
        private static Color DefaultGUIBackground;


        private void OnEnable()
        {
            versionNumberProp = serializedObject.FindProperty("version");
            releaseDateProp = serializedObject.FindProperty("releaseDate");

            audioPrefabProp = serializedObject.FindProperty("audioPrefab");
            clipAudioMixerProp = serializedObject.FindProperty("clipAudioMixer");
            musicPrefabProp = serializedObject.FindProperty("musicPrefab");
            musicAudioMixerProp = serializedObject.FindProperty("musicAudioMixer");

            debugProp = serializedObject.FindProperty("showDebugMessages");
            additionalMixersProp = serializedObject.FindProperty("additionalMixerGroups");

            DefaultGUIColor = GUI.color;
            DefaultGUIBackground = GUI.backgroundColor;
        }


        public override void OnInspectorGUI()
        {
            DrawHeaderLogo();
            DrawEditButton();
            DrawMetaData();
            DrawAudioOptions();
            DrawExtraSettings();
            base.OnInspectorGUI();
        }


        private static void DrawHeaderLogo()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(AmEditorUtils.ManagerHeader, GUIStyle.none, GUILayout.MaxHeight(85)))
                GUI.FocusControl(null);
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }


        private static void DrawEditButton()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = AmEditorUtils.Yellow;
            
            if (GUILayout.Button("Edit Settings", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.25f)))
                SettingsService.OpenUserPreferences("Preferences/Carter Games/Audio Manager");
            
            GUI.backgroundColor = DefaultGUIBackground;
        }
        

        private void DrawMetaData()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();

            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Meta Data", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;

            GUI.enabled = false;
            EditorGUILayout.PropertyField(versionNumberProp);
            EditorGUILayout.PropertyField(releaseDateProp);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }


        private void DrawAudioOptions()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();

            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Audio Options", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(audioPrefabProp);
            EditorGUILayout.PropertyField(clipAudioMixerProp);
            EditorGUILayout.PropertyField(musicPrefabProp);
            EditorGUILayout.PropertyField(musicAudioMixerProp);
            GUI.enabled = true;
            
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
        
        
        private void DrawExtraSettings()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();

            GUI.color = AmEditorUtils.Red;
            EditorGUILayout.LabelField("Additional Options", EditorStyles.boldLabel);
            GUI.color = DefaultGUIColor;
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(debugProp);
            EditorGUILayout.PropertyField(additionalMixersProp);
            GUI.enabled = true;
            
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
    }
}