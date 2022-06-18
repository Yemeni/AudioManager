using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class AudioScanner : AssetPostprocessor
    {
        private static bool _hasNewAudioClip;
        private static AudioLibrary _library;
        
        
        public static AudioLibrary Library
        {
            get
            {
                if (_library != null)
                    return _library;

                var asset = AssetDatabase.FindAssets("t:AudioLibrary", null);

                if (asset.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                    var loadedAsset = (AudioLibrary)AssetDatabase.LoadAssetAtPath(path, typeof(AudioLibrary));

                    _library = loadedAsset;
                    return _library;
                }

                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(typeof(AudioLibrary)), $"Assets/Resources/Audio Manager/Audio Library.asset");
                AssetDatabase.Refresh();

                asset = AssetDatabase.FindAssets("t:AudioLibrary", null);

                if (asset.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                    var loadedAsset = (AudioLibrary)AssetDatabase.LoadAssetAtPath(path, typeof(AudioLibrary));

                    _library = loadedAsset;
                    return _library;
                }

                Debug.LogError("Audio Manager | CG: No Audio Library Found In Project!");
                return null;
            }
        }
        
        private static bool LibraryExists => Library != null;
        
        
        
        [MenuItem("Tools/Audio Manager | CG/Perform Manual Scan")]
        public static void ManualScan()
        {
            GetLibraryDictionary(Library).SetValue(Library, GetAllClipsInProject());
            GetMixerGroups(Library).SetValue(Library, GetAllMixersInProject());
            EnumHandler.RefreshClips();
            EnumHandler.RefreshGroups();
            EnumHandler.RefreshMixers();
            EditorUtility.SetDirty(Library);
        }


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (LibraryExists)
            {
                if (importedAssets.Any(t => t.Contains(".mixer")))
                {
                    GetMixerGroups(Library).SetValue(Library, GetAllMixersInProject());
                    EnumHandler.RefreshMixers();
                }
                
                if (!_hasNewAudioClip) return;
                GetLibraryDictionary(Library).SetValue(Library, GetAllClipsInProject());
                EnumHandler.RefreshClips();
                EditorUtility.SetDirty(Library);
                _hasNewAudioClip = false;
            }
            else
            {
                CreateLibrary();
            }
        }


        private void OnPostprocessAudio(AudioClip a)
        {
            _hasNewAudioClip = true;
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
            
            GetLibraryDictionary(Library).SetValue(Library, newLib);
            EnumHandler.RefreshClips();
        }


        private static FieldInfo GetLibraryDictionary(AudioLibrary lib)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return lib.GetType().GetField("library", flags);
        }
        
        
        private static FieldInfo GetMixerGroups(AudioLibrary lib)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return lib.GetType().GetField("mixers", flags);
        }
    }
}