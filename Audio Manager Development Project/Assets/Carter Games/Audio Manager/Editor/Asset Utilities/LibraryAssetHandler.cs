using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class LibraryAssetHandler
    {
        private static AudioLibrary AudioLibraryAsset;
        public const string LibraryPath = "Assets/Resources/Audio Manager/Audio Library.asset";
        private const string LibraryFilter = "t:AudioLibrary";


        public static AudioLibrary CreateLibraryAsset()
        {
            var instance = ScriptableObject.CreateInstance(typeof(AudioLibrary));
            AssetDatabase.CreateAsset(instance, LibraryPath);
            AssetDatabase.Refresh();
            AudioLibraryAsset = instance as AudioLibrary;
            return AudioLibraryAsset;
        }


        public static AudioLibrary GetAsset()
        {
            if (AudioLibraryAsset != null) return AudioLibraryAsset;
            
            if (Application.isPlaying)
            {
                AudioLibraryAsset = Resources.Load(LibraryPath) as AudioLibrary;
                return AudioLibraryAsset;
            }
            else
            {
                var asset = AssetDatabase.FindAssets(LibraryFilter, null);

                if (asset.Length <= 0) return CreateLibraryAsset();
                    
                var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                var loadedAsset = (AudioLibrary)AssetDatabase.LoadAssetAtPath(path, typeof(AudioLibrary));

                AudioLibraryAsset = loadedAsset;
                return AudioLibraryAsset;
            }
        }
        
        
        public static Dictionary<string, AudioData> Dictionary(AudioLibrary lib)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return (Dictionary<string, AudioData>) lib.GetType().GetField("library", flags)?.GetValue(lib);  
        }
        
        
        public static void SetDictionary(AudioData[] value)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            AudioLibraryAsset.GetType().GetField("library", flags)?.SetValue(AudioLibraryAsset, value);  
        }
        
        
        public static AudioMixerGroup[] MixerGroups(AudioLibrary lib)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return (AudioMixerGroup[]) lib.GetType().GetField("mixers", flags)?.GetValue(AudioLibraryAsset);
        }
        
        
        public static void SetMixerGroups(AudioMixerGroup[] value)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            AudioLibraryAsset.GetType().GetField("mixers", flags)?.SetValue(AudioLibraryAsset, value);
        }
    }
}