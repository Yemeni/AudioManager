using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager.Editor
{
    /// <summary>
    /// Scans for audio clips when a new audio clip is added to the project...
    /// </summary>
    public class AudioScanner : AssetPostprocessor
    {
        private static bool HasNewAudioClip;
        private static AudioLibrary Library = AssetAccessor.GetAsset<AudioLibrary>();
        
        
        private static bool LibraryExists => Library != null;
        
        
        
        [MenuItem("Tools/Audio Manager | CG/Perform Manual Scan")]
        public static void ManualScan()
        {
            LibraryAssetEditorUtil.SetDictionary(GetAllClipsInProject());
            LibraryAssetEditorUtil.SetMixerGroups(GetAllMixersInProject());
            StructHandler.RefreshClips();
            StructHandler.RefreshGroups();
            StructHandler.RefreshMixers();
            EditorUtility.SetDirty(Library);
        }


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (LibraryExists)
            {
                if (importedAssets.Any(t => t.Contains(".mixer")))
                {
                    LibraryAssetEditorUtil.SetMixerGroups(GetAllMixersInProject());
                    StructHandler.RefreshMixers();
                }
                
                if (!HasNewAudioClip) return;
                LibraryAssetEditorUtil.SetDictionary(GetAllClipsInProject());
                StructHandler.RefreshClips();
                EditorUtility.SetDirty(Library);
                HasNewAudioClip = false;
            }
            else
            {
                CreateLibrary();
            }
        }


        private void OnPostprocessAudio(AudioClip a)
        {
            HasNewAudioClip = true;
        }


        private static void CreateLibrary()
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(typeof(AudioLibrary)), $"Assets/Resources/Audio Manager/Audio Library.asset");
            AssetDatabase.Refresh();
        }


        private static AudioData[] GetAllClipsInProject()
        {
            AssetDatabase.Refresh();
            var assets = AssetDatabase.FindAssets("t:AudioClip", null);

            if (assets.Length <= 0) return null;

            var clips = new AudioClip[assets.Length];

            for (var i = 0; i < assets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                clips[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            }

            var data = new AudioData[clips.Length];

            for (var i = 0; i < clips.Length; i++)
                data[i] = new AudioData(clips[i].name, clips[i]);

            return data;
        }
        
        
        private static AudioMixerGroup[] GetAllMixersInProject()
        {
            AssetDatabase.Refresh();
            var assets = AssetDatabase.FindAssets("t:AudioMixerGroup", null);

            if (assets.Length <= 0) return null;

            var mixers = new List<AudioMixerGroup>();

            for (var i = 0; i < assets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                var mixer = (AudioMixer)AssetDatabase.LoadAssetAtPath(path, typeof(AudioMixer));

                for (var j = 0; j < mixer.FindMatchingGroups(string.Empty).Length; j++)
                {
                    if (mixers.Contains(mixer.FindMatchingGroups(string.Empty)[j])) continue;
                    mixers.Add(mixer.FindMatchingGroups(string.Empty)[j]);
                }
            }

            return mixers.ToArray();
        }


        public static void RemoveNullEntriesInLibrary(string removed)
        {
            var clipRemovedName = removed.Split('/');
            var clipName = clipRemovedName[clipRemovedName.Length - 1].Split('.')[0];
            var newLib = Library.GetData.Where(t => !t.key.Equals(clipName)).ToArray();

            LibraryAssetEditorUtil.SetDictionary(newLib);
            StructHandler.RefreshClips();
        }
    }
}