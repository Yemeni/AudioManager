using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class LibraryAssetHandler
    {
        private static AudioLibrary AudioLibraryAsset = AssetAccessor.GetAsset<AudioLibrary>();
        private const string LibraryPath = "Assets/Resources/Audio Manager/Audio Library.asset";
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
            return AssetAccessor.GetAsset<AudioLibrary>();
        }


        public static Dictionary<string, AudioData> Dictionary()
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return (Dictionary<string, AudioData>) AudioLibraryAsset.GetType().GetField("library", flags)?.GetValue(AudioLibraryAsset);  
        }
        
        
        public static void SetDictionary(AudioData[] value)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            AudioLibraryAsset.GetType().GetField("library", flags)?.SetValue(AudioLibraryAsset, value);  
        }
        
        
        public static AudioMixerGroup[] MixerGroups()
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return (AudioMixerGroup[]) AudioLibraryAsset.GetType().GetField("mixers", flags)?.GetValue(AudioLibraryAsset);
        }
        
        
        public static void SetMixerGroups(AudioMixerGroup[] value)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            AudioLibraryAsset.GetType().GetField("mixers", flags)?.SetValue(AudioLibraryAsset, value);
        }
        
        
        public static List<CustomTransition> CustomTransitions()
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return (List<CustomTransition>) AudioLibraryAsset.GetType().GetField("customTransitions", flags)?.GetValue(AudioLibraryAsset);
        }
        
        
        public static void SetCustomTransitions(List<CustomTransition> value)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            AudioLibraryAsset.GetType().GetField("customTransitions", flags)?.SetValue(AudioLibraryAsset, value);
        }
    }
}