using UnityEditor;
using UnityEngine;

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
 */

namespace CarterGames.Legacy.AudioManager.Editor
{
    [CustomEditor(typeof(AudioManagerFile))]
    public class AudioManagerFileEditor : UnityEditor.Editor
    {
        private readonly string[] TabTitles = new string[2] {"Settings", "Library"};
        
        private SerializedProperty baseScanDir;
        private SerializedProperty audioPrefab;
        private SerializedProperty hasDir;
        private SerializedProperty audioMixers;
        private SerializedProperty directories;
        private SerializedProperty library;
        private SerializedProperty tabPos;
        
        private AudioManagerFile thy;
        private Color defaultContentCol;
        private Color normalBackgroundCol;
        
        private bool HasErrors { get; set; }


        private void Awake()
        {
            thy = serializedObject.targetObject as AudioManagerFile;
            
            // Sets up the base scan directory stuff...
            if (AudioManager.AreClipsInBaseDirectory(thy))
                AudioManager.FirstSetup(thy);
        }

        
        private void OnEnable()
        {
            baseScanDir = serializedObject.FindProperty("baseScanDirectory");
            audioPrefab = serializedObject.FindProperty("soundPrefab");
            hasDir = serializedObject.FindProperty("hasDirectories");
            audioMixers = serializedObject.FindProperty("audioMixer");
            directories = serializedObject.FindProperty("directory");
            library = serializedObject.FindProperty("clips");
            tabPos = serializedObject.FindProperty("tabPos");

            defaultContentCol = GUI.contentColor;
            normalBackgroundCol = GUI.backgroundColor;
            
            AudioManager.ScanForFiles(serializedObject.targetObject as AudioManagerFile);
        }


        public override void OnInspectorGUI()
        {
            AudioManagerEditorHelper.Header("Audio Manager File", false, normalBackgroundCol);
            RenderTabBar();

            HasErrors = AudioManager.NoFilesInADirectoryCheck;
            
            if (HasErrors)
            {
                AudioManager.ShowNoFilesInDirectoryHelpMessage(serializedObject.targetObject as AudioManagerFile);
            }
            
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


        private void RenderSettings(int pos)
        {
            if (!pos.Equals(0)) return;
            
            GUI.contentColor = AudioManagerEditorHelper.AmRedCol;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = defaultContentCol;
            
            EditorGUILayout.HelpBox("The directory that this file scans from...", MessageType.None);
            EditorGUILayout.PropertyField(baseScanDir, new GUIContent("Base Directory:"));
            EditorGUILayout.Space(7.5f);
            
            EditorGUILayout.HelpBox("The audio prefab that is spawned in when you call for audio to be played.", MessageType.None);
            EditorGUILayout.PropertyField(audioPrefab, new GUIContent("Sound Prefab:"));
            
            GUILayout.Space(15f);
            
            GUI.contentColor = AudioManagerEditorHelper.AmRedCol;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("File Setup", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = defaultContentCol;
            
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
                
                if (GUILayout.Button("+", GUILayout.Width(25f)))
                    audioMixers.InsertArrayElementAtIndex(i + 1);

                if (GUILayout.Button("-", GUILayout.Width(25f)))
                    audioMixers.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();
            }            
            
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Directories", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();

            if (!hasDir.boolValue || directories.arraySize.Equals(0))
            {
                directories.InsertArrayElementAtIndex(0);
                hasDir.boolValue = true;
            }
            
            for (var i = 0; i < directories.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"#{(i+1).ToString()}", GUILayout.Width(TextWidth($"#{(i+1).ToString()} ")));
                EditorGUILayout.PropertyField(directories.GetArrayElementAtIndex(i), GUIContent.none);
                
                if (GUILayout.Button("+", GUILayout.Width(25f)))
                    directories.InsertArrayElementAtIndex(i + 1);

                if (GUILayout.Button("-", GUILayout.Width(25f)))
                    directories.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();
            }
        }


        private void RenderLibrary(int pos)
        {
            if (!pos.Equals(1)) return;


            GUI.contentColor = AudioManagerEditorHelper.AmRedCol;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = defaultContentCol;
            
            EditorGUILayout.BeginHorizontal();
            
            if (HasErrors)
            {
                EditorGUILayout.HelpBox("Cannot scan audio files while an error is present! Please resolve any issues before trying to scan.", MessageType.None);
            }
            else
            {
                GUI.backgroundColor = AudioManagerEditorHelper.GreenCol;

                if (GUILayout.Button("Scan For New Clips"))
                    AudioManager.ScanForFiles(serializedObject.targetObject as AudioManagerFile);

                GUI.backgroundColor = normalBackgroundCol;

                if (GUILayout.Button("ClearIndicator Library"))
                    library.ClearArray();
            }

            EditorGUILayout.EndHorizontal();

                GUI.contentColor = AudioManagerEditorHelper.AmRedCol;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Audio Library", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
                EditorGUILayout.EndHorizontal();
                GUI.contentColor = defaultContentCol;

                EditorGUILayout.HelpBox("Shows all the clips that are present in the library of this file.",
                    MessageType.None);
            

            GUILayout.Space(5f);


            
            if (library.arraySize.Equals(0)) return;
            
            GUI.contentColor = AudioManagerEditorHelper.AmRedCol;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Clip Directory", EditorStyles.boldLabel, GUILayout.Width(Screen.width / 3.25f));
            EditorGUILayout.LabelField("Clip Name", EditorStyles.boldLabel, GUILayout.Width(Screen.width / 3.25f));
            EditorGUILayout.LabelField("Audio Clip File", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = defaultContentCol;
            
            GUI.enabled = false;
            
            for (var i = 0; i < library.arraySize; i++)
            {
                var _filePath = library.GetArrayElementAtIndex(i).FindPropertyRelative("path");
                var _fileName = library.GetArrayElementAtIndex(i).FindPropertyRelative("key");
                var _fileClip = library.GetArrayElementAtIndex(i).FindPropertyRelative("value");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_filePath, GUIContent.none, GUILayout.Width(Screen.width / 3.25f));
                EditorGUILayout.PropertyField(_fileName, GUIContent.none, GUILayout.Width(Screen.width / 3.25f));
                EditorGUILayout.PropertyField(_fileClip, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            
            GUI.enabled = true;
        }
        
        
        private static float TextWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
    }
}