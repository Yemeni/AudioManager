using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class EditorWindowMain : EditorWindow
    {
        private static SerializedObject settingsObject;
        private static SerializedProperty editorTabPos;
        private AudioLibrary library;


        [MenuItem("Tools/Audio Manager | CG/Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<EditorWindowMain>();
            window.titleContent = new GUIContent("Editor")
            {
                image = AudioManagerEditorUtil.ManagerLogoTransparent
            };
            
            window.Show();
        }
        
        public static void ShowWindowOnTab(int tab)
        {
            var window = GetWindow<EditorWindowMain>();
            window.titleContent = new GUIContent("Editor")
            {
                image = AudioManagerEditorUtil.ManagerLogoTransparent
            };
            
            window.Show();
            settingsObject = new SerializedObject(AudioManagerEditorUtil.Settings);
            editorTabPos = settingsObject.FindProperty("editorTabPosition");
            editorTabPos.intValue = tab;
        }


        private void OnEnable()
        {
            settingsObject = new SerializedObject(AudioManagerEditorUtil.Settings);
            editorTabPos = settingsObject.FindProperty("editorTabPosition");
        }


        private void OnDisable()
        {
            // Closing window... apply any changes made...
            StructHandler.RefreshGroups();
        }


        private void OnGUI()
        {
            if (library == null)
            {
                library = (AudioLibrary)AudioManagerEditorUtil.GetFile<AudioLibrary>("t:audiolibrary");
            }
            
            EditGroups.GetGroupsInFile();

            EditorWindow editorWindow = this;
            editorWindow.minSize = new Vector2(500f, 500f);
            editorWindow.maxSize = new Vector2(800f, 750f);
            
            DrawHeader();
            DrawTabButtons();
        }


        private static void DrawHeader()
        {
            if (!AudioManagerEditorUtil.ManagerHeader) return;
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(AudioManagerEditorUtil.ManagerHeader, GUIStyle.none, GUILayout.MaxHeight(110)))
                GUI.FocusControl(null);
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }



        private void DrawTabButtons()
        {
            EditorGUILayout.Space();

            editorTabPos.intValue = GUILayout.Toolbar(editorTabPos.intValue,
                new[] { "Edit Library", "Edit Groups", "Edit Curves" });

            switch (editorTabPos.intValue)
            {
                case 0:
                    EditLibrary.DrawLibrary();
                    break;
                case 1:
                    EditGroups.DrawAllGroups();
                    break;
                case 2:
                    EditCurves.DrawAllCurves();
                    break;
            }

            settingsObject.ApplyModifiedProperties();
            settingsObject.Update();
        }
    }
}