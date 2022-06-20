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


        public static void DrawAllCurves()
        {
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
            

            for (var i = 0; i < transitions.Count; i++)
            {
                DrawTransition(transitions[i]);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }


        private static void DrawTransition(CustomTransition data)
        {
            var curveName = data.id;

            EditorGUILayout.BeginHorizontal();

            
            data.id = EditorGUILayout.TextField(
                new GUIContent("Transition Id:",
                    "The name to refer to this transition as, it cannot match another transition name."), data.id);

            // Avoids dup names...
            if (DoesTransitionNameExist(data.id, true, out var total))
                data.id += $"({total - 1})";


            data.curve =
                EditorGUILayout.CurveField(new GUIContent("Transition Curve:", "The curve to use in the transition"),
                    data.curve);
            
            if (GUILayout.Button("+", GUILayout.Width(20f)))
            {
                //data.Clips.Add(library.AllClipNames[0]);
                hasMadeChanges = true;
            }
            
            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                //library.RemoveGroup(data);
                //EnumHandler.RefreshGroups();
                hasMadeChanges = true;
                return;
            }

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
    }
}