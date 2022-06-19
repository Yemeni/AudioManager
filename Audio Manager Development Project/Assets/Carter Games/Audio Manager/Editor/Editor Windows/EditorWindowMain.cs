using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class EditorWindowMain : EditorWindow
    {
        private static SerializedProperty editorTabPos;
        private AudioLibrary library;


        [MenuItem("Tools/Audio Manager | CG/Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<EditorWindowMain>();
            window.titleContent = new GUIContent("Editor")
            {
                image = AmEditorUtils.ManagerLogoTransparent
            };
            
            window.Show();
        }
        
        public static void ShowWindowOnTab(int tab)
        {
            var window = GetWindow<EditorWindowMain>();
            window.titleContent = new GUIContent("Editor")
            {
                image = AmEditorUtils.ManagerLogoTransparent
            };
            
            window.Show();
            editorTabPos = new SerializedObject(AmEditorUtils.Settings).FindProperty("editorTabPosition");
            editorTabPos.intValue = tab;
        }


        private void OnEnable()
        {
            editorTabPos = new SerializedObject(AmEditorUtils.Settings).FindProperty("editorTabPosition");
        }


        private void OnDisable()
        {
            // Closing window... apply any changes made...
            EnumHandler.RefreshGroups();
        }


        private void OnGUI()
        {
            if (library == null)
            {
                library = (AudioLibrary)AmEditorUtils.GetFile<AudioLibrary>("t:audiolibrary");
            }
            
            EditGroups.GetGroupsInFile();

            DrawHeader();
            DrawTabButtons();
        }


        private static void DrawHeader()
        {
            if (!AmEditorUtils.ManagerHeader) return;
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(AmEditorUtils.ManagerHeader, GUIStyle.none, GUILayout.MaxHeight(110)))
                GUI.FocusControl(null);
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }


        
        private void DrawTabButtons()
        {
            EditorGUILayout.Space();
            
            editorTabPos.intValue = GUILayout.Toolbar(editorTabPos.intValue, new[] { "Edit Library", "Edit Groups", "Edit Curves" });

            switch (editorTabPos.intValue)
            {
                case 0:
                    EditLibrary.DrawLibrary();
                    break;
                case 1:
                    EditGroups.DrawAllGroups();
                    break;
                case 2:
                    break;
            }
        }
    }
}