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

        
        
        

        public static void DrawAllCurves()
        {
            GetEditorColours();
            
            EditorGUILayout.Space();

            var transitions = LibraryAssetHandler.CustomTransitions();

            EditorGUILayout.HelpBox(
                ".",
                MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create New Transition"))
            {
                transitions.Add(new CustomTransition());

                var newTransition = transitions[transitions.Count - 1];
                
                if (DoesTransitionNameExist(newTransition.id, false, out var total))
                    newTransition.id += $"({total - 1})";

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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Id", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Curve", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Behaviour", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("   ", GUIStyle.none, GUILayout.Width(40f));
            EditorGUILayout.EndHorizontal();

            for (var i = 0; i < transitions.Count; i++)
            {
                DrawTransition(transitions[i]);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }


        private static void DrawTransition(CustomTransition data)
        {
            EditorGUILayout.BeginHorizontal();
            
            data.id = EditorGUILayout.TextField(GUIContent.none, data.id, GUILayout.MinWidth(50f));

            // Avoids dup names...
            if (DoesTransitionNameExist(data.id, true, out var total))
                data.id += $"({total - 1})";
            
            data.curve = EditorGUILayout.CurveField(GUIContent.none, data.curve, GUILayout.MinWidth(50f));
            
            data.fadeType = (CustomFadeTypes) EditorGUILayout.EnumPopup(data.fadeType, GUILayout.MinWidth(50f));

            GUI.color = AmEditorUtils.Green;
            if (GUILayout.Button("+", GUILayout.Width(20f)))
            {
                //data.Clips.Add(library.AllClipNames[0]);
                
                hasMadeChanges = true;
            }
            GUI.color = AmEditorUtils.Red;
            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                //library.RemoveGroup(data);
                //EnumHandler.RefreshGroups();
                hasMadeChanges = true;
                return;
            }
            GUI.color = DefaultGUIColor;

            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }



        private static bool DoesTransitionNameExist(string name, bool allowClose, out int total)
        {
            var transitions = LibraryAssetHandler.CustomTransitions();
            
            total = allowClose 
                ? transitions.Where(t => !string.IsNullOrEmpty(t.id)).Count(t => t.id.Equals(name)) 
                : transitions.Where(t => !string.IsNullOrEmpty(t.id)).Count(t => t.id.Equals(name) || t.id.Contains(name));

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