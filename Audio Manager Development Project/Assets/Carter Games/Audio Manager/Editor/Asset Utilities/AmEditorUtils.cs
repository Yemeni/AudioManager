using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    public static class AmEditorUtils
    {
        internal static readonly Color Green = new Color32(72, 222, 55, 255);
        internal static readonly Color Yellow = new Color32(245, 234, 56, 255);
        internal static readonly Color Blue = new Color32(151, 196, 255, 255);
        internal static readonly Color Hidden = new Color(0, 0, 0, .3f);
        internal static readonly Color Red = new Color32(255, 150, 157, 255);
        
        private static Texture2D cachedManagerHeaderImg;
        private static Texture2D cachedManagerHeaderLogoTransparentImg;
        private static Texture2D cachedCarterGamesBannerImg;
        private static AudioManagerSettings cachedAudioSettings;
        

        public static Texture2D ManagerHeader
        {
            get
            {
                if (cachedManagerHeaderImg != null) return cachedManagerHeaderImg;
                cachedManagerHeaderImg = (Texture2D) GetFile<Texture2D>("AudioManagerEditorHeader");
                return cachedManagerHeaderImg;
            }
        }
        
        public static Texture2D CarterGamesBanner
        {
            get
            {
                if (cachedCarterGamesBannerImg != null) return cachedCarterGamesBannerImg;
                cachedCarterGamesBannerImg = (Texture2D) GetFile<Texture2D>("CarterGamesBanner");
                return cachedCarterGamesBannerImg;
            }
        }
        
        public static Texture ManagerLogoTransparent
        {
            get
            {
                if (cachedManagerHeaderLogoTransparentImg != null) return cachedManagerHeaderLogoTransparentImg;
                cachedManagerHeaderLogoTransparentImg = (Texture2D) GetFile<Texture2D>("AudioManagerLogoNoBG");
                return cachedManagerHeaderLogoTransparentImg;
            }
        }

        public static AudioManagerSettings Settings
        {
            get
            {
                if (cachedAudioSettings != null) return cachedAudioSettings;
                cachedAudioSettings = (AudioManagerSettings)GetFile<AudioManagerSettings>("t:audiomanagersettings");
                return cachedAudioSettings;
            }
            set => cachedAudioSettings = value;
        }
        
        
        public static AudioManagerSettings SettingsRuntime
        {
            get
            {
                if (cachedAudioSettings != null) return cachedAudioSettings;
                cachedAudioSettings = (AudioManagerSettings)GetFileAtRuntime<AudioManagerSettings>();
                return cachedAudioSettings;
            }
        }
        

        public static bool HasFile(string filter)
        {
            return AssetDatabase.FindAssets(filter, null).Length > 0;
        }

        public static void CreateFile<T>(string path)
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(typeof(T)), path);
            AssetDatabase.Refresh();
        }

        public static object GetFile<T>(string filter)
        {
            var _asset = AssetDatabase.FindAssets(filter, null);
            if (_asset == null || _asset.Length <= 0) return null;
            var _path = AssetDatabase.GUIDToAssetPath(_asset[0]);
            return AssetDatabase.LoadAssetAtPath(_path, typeof(T));
        }
        
        public static List<object> GetFiles<T>(string filter)
        {
            var _asset = AssetDatabase.FindAssets(filter, null);
            var loaded = new List<object>();
            
            foreach (var asset in _asset)
                loaded.Add(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof(T)));

            return loaded;
        }
        
        public static string GetPathOfFile(string className, string toMatch)
        {
            var _asset = AssetDatabase.FindAssets(className, null);
            var find = _asset.FirstOrDefault(t => AssetDatabase.GUIDToAssetPath(t).Contains(toMatch));
            return AssetDatabase.GUIDToAssetPath(find);
        }

        private static object GetFileAtRuntime<T>()
        {
            return Resources.FindObjectsOfTypeAll(typeof(T))[0];
        }

        public static AudioManagerSettings GenerateSettings(AudioManagerSettings find)
        {
            if (find != null) return find;
            
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(typeof(AudioManagerSettings)), $"Assets/Resources/Audio Manager/Audio Manager Settings.asset");
            AssetDatabase.Refresh();

            var settings = (AudioManagerSettings)GetFile<AudioManagerSettings>("t:audiomanagersettings");
            var audioPrefab = (GameObject) GetFile<GameObject>("l:CGAMSoundPrefab");
            var musicPrefab = (GameObject) GetFile<GameObject>("l:CGAMMusicPrefab");
            
            settings.InitialiseSettings(audioPrefab, musicPrefab);
            return settings;
        }
        
        public static float Width(this string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
    }
}