using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor
{
    public static class DirectorySelectHelper
    {
        private static List<string> _allDirectories = new List<string>();
        private static List<string> _directoriesCache = new List<string>();


        public static void RefreshAllDirectories()
        {
            _allDirectories = new List<string>();
            _allDirectories.Add("");
            _allDirectories.Add("Assets");
            _allDirectories.AddRange(Directory.GetDirectories("Assets", "*", SearchOption.AllDirectories));

            for (var i = 0; i < _allDirectories.Count; i++)
            {
                _allDirectories[i] = _allDirectories[i].Replace(@"\", "/");
            }
        }
        

        public static List<string> GetAllDirectories()
        {
            if (_allDirectories.Count > 0)
            {
                return _allDirectories;
            }
            
            _allDirectories = new List<string>();
            _allDirectories.Add("");
            _allDirectories.Add("Assets");
            _allDirectories.AddRange(Directory.GetDirectories("Assets", "*", SearchOption.AllDirectories));

            for (var i = 0; i < _allDirectories.Count; i++)
            {
                _allDirectories[i] = _allDirectories[i].Replace(@"\", "/");
            }

            return _allDirectories;
        }

        public static string ConvertIntToDir(int value)
        {
            return GetAllDirectories()[value];
        }

        public static int ConvertStringToIndex(string value)
        {
            return GetAllDirectories().IndexOf(value);
        }


        public static List<string> GetDirectoriesFromBase(bool shouldUpdate = false)
        {
            if (!shouldUpdate)
            {
                if (_directoriesCache.Count > 0)
                    return _directoriesCache;
            }
            
            _directoriesCache = new List<string>();
            _directoriesCache.Add("");
            _directoriesCache.Add(AudioManagerEditorUtil.Settings.baseAudioScanPath);
            _directoriesCache.AddRange(Directory.GetDirectories(AudioManagerEditorUtil.Settings.baseAudioScanPath, "*", SearchOption.AllDirectories));

            for (var i = 0; i < _directoriesCache.Count; i++)
            {
                _directoriesCache[i] = _directoriesCache[i].Replace(@"\", "/");
            }
            
            return _directoriesCache;
        }
        
        public static string ConvertIntToDir(int value, List<string> options)
        {
            return options[value];
        }

        public static int ConvertStringToIndex(string value, List<string> options)
        {
            return options.IndexOf(value);
        }
    }
}