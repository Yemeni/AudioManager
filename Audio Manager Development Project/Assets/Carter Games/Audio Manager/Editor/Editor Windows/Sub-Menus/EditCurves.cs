using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class EditCurves
    {
        private static readonly AudioLibrary library = AssetAccessor.GetAsset<AudioLibrary>();
        private static Vector2 scrollRect;
        private static bool hasMadeChanges;
        
        private static Color DefaultGUIColor;
        private static Color DefaultGUIBackground;

        private static SerializedObject obj;
        private static SerializedProperty transitions;


        public static void DrawAllCurves()
        {
            GetEditorColours();
            
            EditorGUILayout.Space();

            if (obj == null)
            {
                obj = new SerializedObject(library);
            }

            transitions = obj.FindProperty("customTransitions");

            EditorGUILayout.HelpBox(
                ".",
                MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create New Transition"))
            {
                var newIndex = 0;
                
                if (transitions.arraySize > 0)
                {
                    newIndex = transitions.arraySize - 1;
                }
                
                transitions.InsertArrayElementAtIndex(newIndex);

                var newTransition = transitions.GetArrayElementAtIndex(newIndex);
                newTransition.FindPropertyRelative("uniqueId").intValue = newIndex;
                
                if (DoesTransitionNameExist(newTransition.FindPropertyRelative("id").stringValue, newTransition.FindPropertyRelative("uniqueId").intValue, false, out var total))
                    newTransition.FindPropertyRelative("id").stringValue += $"({total - 1})";

                hasMadeChanges = true;
            }

            if (hasMadeChanges)
            {
                if (GUILayout.Button("Apply Changes"))
                {
                    StructHandler.RefreshGroups();
                    hasMadeChanges = false;
                }
            }
            
            
            EditorGUILayout.EndHorizontal();

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Id", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Curve", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Behaviour", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("   ", GUIStyle.none, GUILayout.Width(40f));
            EditorGUILayout.EndHorizontal();

            for (var i = 0; i < transitions.arraySize; i++)
            {
                DrawTransition(transitions.GetArrayElementAtIndex(i), i);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }


        private static void DrawTransition(SerializedProperty data, int index)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PropertyField(data.FindPropertyRelative("id"), GUIContent.none, GUILayout.MinWidth(50f));

            // Avoids dup names...
            if (DoesTransitionNameExist(data.FindPropertyRelative("id").stringValue, data.FindPropertyRelative("uniqueId").intValue, true, out var total))
                data.FindPropertyRelative("id").stringValue += $"({total - 1})";
            
            EditorGUILayout.PropertyField(data.FindPropertyRelative("curve"), GUIContent.none, GUILayout.MinWidth(50f));
            
            EditorGUILayout.PropertyField(data.FindPropertyRelative("fadeType"), GUIContent.none, GUILayout.MinWidth(50f));

            if (EditorGUI.EndChangeCheck())
            {
                obj.ApplyModifiedProperties();
                obj.Update();
            }

            GUI.color = AudioManagerEditorUtil.Green;
            if (GUILayout.Button("+", GUILayout.Width(20f)))
            {
                transitions.InsertArrayElementAtIndex(index);
                transitions.GetArrayElementAtIndex(index).FindPropertyRelative("uniqueId").intValue = transitions.arraySize - 1;
            }
            GUI.color = AudioManagerEditorUtil.Red;
            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                transitions.DeleteArrayElementAtIndex(index);
            }
            GUI.color = DefaultGUIColor;

            EditorGUILayout.EndHorizontal();
        }



        private static bool DoesTransitionNameExist(string name, int uniqueId, bool allowClose, out int total)
        {
            var allTransitions = LibraryAssetEditorUtil.CustomTransitions();
            
            total = allowClose 
                ? allTransitions.Where(t => !string.IsNullOrEmpty(t.id)).Count(t => t.id.Equals(name)) 
                : allTransitions.Where(t => !string.IsNullOrEmpty(t.id)).Count(t => t.id.Equals(name) || t.id.Contains(name));

            if (total > 1)
            {
                var indexedObj = allTransitions.FirstOrDefault(t => t.id.Equals(name));
                if (indexedObj.uniqueId.Equals(uniqueId))
                {
                    total = 0;
                    return total > 1;
                }
            }

            return total > 1;
        }



        private static void GetEditorColours()
        {
            if (DefaultGUIColor != GUI.color)
                DefaultGUIColor = GUI.color;
            
            if (DefaultGUIBackground != GUI.backgroundColor)
                DefaultGUIBackground = GUI.backgroundColor;
        }
    }
}