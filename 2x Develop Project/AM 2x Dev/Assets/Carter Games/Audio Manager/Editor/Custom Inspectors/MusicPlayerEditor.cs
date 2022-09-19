/*
 * 
 *  Audio Manager
 *							  
 *	Music Player Editor
 *      Handles the custom inspector for the music player script....
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
 *  Version: 2.5.8
 *	Last Updated: 18/06/2022 (d/m/y)							
 * 
 */

using UnityEngine;
using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    [CustomEditor(typeof(MusicPlayer)), CanEditMultipleObjects]
    public class MusicPlayerEditor : UnityEditor.Editor
    {
        private readonly Color32 greenCol = new Color32(41, 176, 97, 255);
        private readonly Color32 redCol = new Color32(190, 42, 42, 255);
        private readonly Color32 amRedCol = new Color32(255, 150, 157, 255);
        private MusicPlayer player;

        private Color normalBgCol;
        
        private SerializedProperty musicTrack;
        private SerializedProperty mixer;
        private SerializedProperty timeToStartFrom;
        private SerializedProperty shouldLoop;
        private SerializedProperty playOnAwake;
        private SerializedProperty timeToLoopAt;
        private SerializedProperty showSource;
        private SerializedProperty volume;
        private SerializedProperty pitch;
        private SerializedProperty musicIntroTransition;
        private SerializedProperty transitionLength;
        
        
        /// <summary>
        /// Assigns the script and does any setup needed.
        /// </summary>
        private void OnEnable()
        {
            player = (MusicPlayer)target;
            
            musicTrack = serializedObject.FindProperty("musicTrack");
            mixer = serializedObject.FindProperty("mixer");
            timeToStartFrom = serializedObject.FindProperty("timeToStartFrom");
            shouldLoop = serializedObject.FindProperty("shouldLoop");
            timeToLoopAt = serializedObject.FindProperty("timeToLoopAt");
            showSource = serializedObject.FindProperty("showSource");
            volume = serializedObject.FindProperty("volume");
            pitch = serializedObject.FindProperty("pitch");
            playOnAwake = serializedObject.FindProperty("playOnAwake");
            musicIntroTransition = serializedObject.FindProperty("introTransition");
            transitionLength = serializedObject.FindProperty("transitionLength");

            normalBgCol = GUI.backgroundColor;
        }
        
        
        /// <summary>
        /// Overrides the default inspector of the Music Player Script with this custom one.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!player)
                player = (MusicPlayer)target;

            AudioManagerEditorUtil.Header("Music Player");
            AudioSourceSetup();

            
            DrawScriptSection();
            
            GUILayout.Space(2.5f);
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            EditorGUILayout.LabelField("Track Info", EditorStyles.boldLabel, GUILayout.MaxWidth(92f));

            GUILayout.Space(2.5f);
            
            EditorGUILayout.PropertyField(musicTrack, new GUIContent("Track To Play:"));
            EditorGUILayout.PropertyField(mixer, new GUIContent("Music Audio Mixer"));

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
            
            //
            //
            //
            //
            //
            
            GUILayout.Space(2.5f);
            
            //
            //
            //
            //
            //
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            EditorGUILayout.LabelField("First Play Setup", EditorStyles.boldLabel, GUILayout.MaxWidth(125f));

            GUILayout.Space(2.5f);
            
            EditorGUILayout.PropertyField(playOnAwake, new GUIContent("Play On Awake:"));
            EditorGUILayout.PropertyField(musicIntroTransition, new GUIContent("Intro Transition:"));
            EditorGUILayout.PropertyField(transitionLength, new GUIContent("Transition Length:"));
            volume.floatValue = EditorGUILayout.Slider( new GUIContent("Track Volume:"), volume.floatValue, 0f, 1f);
            pitch.floatValue = EditorGUILayout.Slider( new GUIContent("Track Pitch:"), pitch.floatValue, 0f, 1f);
            EditorGUILayout.PropertyField(shouldLoop, new GUIContent("Should Loop Track:"));
            EditorGUILayout.PropertyField(timeToStartFrom, new GUIContent("Start Track At:"));
            EditorGUILayout.PropertyField(timeToLoopAt, new GUIContent("Loop Track At:"));

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
            
            //
            //
            //
            //
            //
            
            GUILayout.Space(2.5f);
            
            //
            //
            //
            //
            //
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (!showSource.boolValue)
            {
                GUI.backgroundColor = AudioManagerEditorUtil.Green;
                if (GUILayout.Button("Show Audio Sources", GUILayout.Width(140f)))
                {
                    player.GetComponents<AudioSource>()[0].hideFlags = HideFlags.None;
                    player.GetComponents<AudioSource>()[1].hideFlags = HideFlags.None;
                    showSource.boolValue = true;
                }
                GUI.backgroundColor = normalBgCol;
            }
            else
            {
                GUI.backgroundColor = AudioManagerEditorUtil.Red;
                if (GUILayout.Button("Hide Audio Sources", GUILayout.Width(140f)))
                {
                    player.GetComponents<AudioSource>()[0].hideFlags = HideFlags.HideInInspector;
                    player.GetComponents<AudioSource>()[1].hideFlags = HideFlags.HideInInspector;
                    showSource.boolValue = false;
                }
                GUI.backgroundColor = normalBgCol;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }

        
        private void DrawScriptSection()
        {
            GUILayout.Space(4.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(target as MusicPlayer), typeof(MusicPlayer), false);
            GUI.enabled = true;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }


        private void AudioSourceSetup()
        {
            // Adds an Audio Source to the gameObject this script is on if its not already there
            // * Hide flags hides it from the inspector so you don't notice it there *
            if (player.gameObject.GetComponent<AudioSource>()) return;
            
            player.gameObject.AddComponent<AudioSource>();
            player.gameObject.AddComponent<AudioSource>();
            player.gameObject.GetComponents<AudioSource>()[0].hideFlags = HideFlags.HideInInspector;
            player.gameObject.GetComponents<AudioSource>()[1].hideFlags = HideFlags.HideInInspector;
            player.gameObject.GetComponents<AudioSource>()[1].playOnAwake = false;
        }
    }
}