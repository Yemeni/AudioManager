/*
 * 
 *  Audio Manager
 *							  
 *	Audio Manager Editor Helper
 *      A helper class to make it easier to make global editor changes for the Audio Manager asset.
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

using UnityEditor;
using UnityEngine;

namespace CarterGames.Legacy.AudioManager.Editor
{
    public static class AudioManagerEditorHelper
    {
        private static readonly string AssetVersionNumber = "3.0.0";
        
        public static readonly Color32 AmRedCol = new Color32(255, 150, 157, 255);
        public static readonly Color32 GreenCol = new Color32(41, 176, 97, 255);
        public static readonly Color32 RedCol = new Color32(190, 42, 42, 255);


        public static void Header(string scriptName, bool showButtons, Color normalBackgroundColour)
        {
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Shows either the Carter Games Logo or an alternative for if the icon is deleted/not included when you import the package
            // Note: if you are using an older version of the asset, the directory/name of the logo may not match this and therefore will display the text title only
            if (Resources.Load<Texture2D>("LogoAM"))
            {
                if (GUILayout.Button(Resources.Load<Texture2D>("LogoAM"), GUIStyle.none, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);

            // Label that shows the name of the script / tool & the Version number for user reference sake.
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(scriptName, EditorStyles.boldLabel, GUILayout.Width(TextWidth($"{scriptName}   ")));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField($"Version: {AssetVersionNumber}", GUILayout.Width(TextWidth($"Version {AssetVersionNumber}  ")));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);

            // Links to the docs and discord server for the user to access quickly if needed.
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Docs", GUILayout.Width(45f)))
                {
                    Application.OpenURL("https://carter.games/audiomanager");
                }

                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("Discord", GUILayout.Width(65f)))
                {
                    Application.OpenURL("https://carter.games/discord");
                }

                GUI.backgroundColor = RedCol;
                if (GUILayout.Button("Report Issue", GUILayout.Width(100f)))
                {
                    Application.OpenURL("https://carter.games/report");
                }

                GUI.backgroundColor = normalBackgroundColour;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            
            GUILayout.Space(10f);
        }
        
        
        public static float TextWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
    }
}