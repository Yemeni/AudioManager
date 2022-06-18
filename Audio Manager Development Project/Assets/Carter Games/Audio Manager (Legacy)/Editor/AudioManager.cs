/*
 * 
 *  Audio Manager
 *							  
 *	Audio Manager
 *      The editor script that handles the scanning & audio management....
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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Legacy.AudioManager.Editor
{
    public static class AudioManager
    {
        private static readonly string DefaultBaseAudioScanPath = "/audio";
        private static string BaseAudioFolderLocation = DefaultBaseAudioScanPath;
        private static readonly string NoClipsInDirMessage = "No clips found in these directories, please check you have all directories spelt correctly or they have audio files in them:\n";
        private static string[,] audioStrings;
        private static List<AudioClip> audioList;
        private static List<string> allFiles;
        private static List<string> allFilePaths;
        private static List<string> noClipDirs;


        /// <summary>
        /// Runs the Init setup for the manager if needed....
        /// </summary>
        public static void FirstSetup(AudioManagerFile file)
        {
            if ((string.IsNullOrEmpty(file.baseScanDirectory)))
                BaseAudioFolderLocation = DefaultBaseAudioScanPath;
            else
                BaseAudioFolderLocation = file.baseScanDirectory;
        }



        public static void ScanForFiles(AudioManagerFile file)
        {
            FirstSetup(file);
            
            // If the file count if the same, so nothing....
            if (file.clips == null && file.clips.Count <= 0) return;
            if (file.clips.Count.Equals(CheckAmount(file))) return;
            
            file.clips.Clear();
            
            AddAudioClips(file);
            AddStrings();

            for (var i = 0; i < audioList.Count; i++)
            {
                file.clips.Add(new AudioLibrary(audioStrings[i,1], audioStrings[i,0], audioList[i]));
            }
        }
        
        
        /// <summary>
        /// Checks to see how many files are found from the scan so it can be displayed.
        /// </summary>
        /// <returns>Int | The amount of clips that have been found.</returns>
        public static int CheckAmount(AudioManagerFile file)
        {
            int _amount = default;
            allFiles = new List<string>();
            allFilePaths = new List<string>();
            noClipDirs = new List<string>();

            if (file.directory == null) return 0;
            if (file.directory.Count > 0)
            {
                for (int i = 0; i < file.directory.Count; i++)
                {
                    if (Directory.Exists(Application.dataPath + BaseAudioFolderLocation + "/" + file.directory[i]))
                    {
                        // 2.3.1 - adds a range so it adds each directory to the asset 1 by 1
                        allFiles.AddRange(new List<string>(Directory.GetFiles(Application.dataPath + BaseAudioFolderLocation + "/" + file.directory[i])));
                    }
                    else
                    {
                        // !Warning Message - shown in the console should there not be a directory named what the user entered
                        noClipDirs.Add(file.directory[i].ToLowerInvariant());
                    }
                }
            }

            // Checks to see if there is anything in the path, if its empty it will not run the rest of the code and instead put a message in the console
            if (allFiles.Count <= 0) return _amount;

            foreach (var _thingy in allFiles)
            {
                var _path = "Assets" + _thingy.Replace(Application.dataPath, "").Replace('\\', '/');

                if (AssetDatabase.LoadAssetAtPath(_path, typeof(AudioClip)))
                {
                    ++_amount;
                    allFilePaths.Add(_path);
                }
            }

            return _amount;
        }

        
        /// <summary>
        /// Adds all strings for the found clips to the AMF.
        /// </summary>
        private static void AddStrings()
        {
            audioStrings = new string[audioList.Count, 2];
            
            for (var i = 0; i < audioList.Count; i++)
            {
                if (audioList[i] == null)
                    audioList.Remove(audioList[i]);
            }

            var _ignored = 0;
            
            for (var i = 0; i < allFilePaths.Count; i++)
            {
                allFilePaths[i] = allFilePaths[i]
                    .Replace("Assets/", "")
                    .Replace(" (UnityEngine.AudioClip)", "")
                    .Substring(0, (allFilePaths[i].Length - (audioList[i].name.Length + 4)));
            }

            for (var i = 0; i < audioList.Count; i++)
            {
                var _t = audioList[i];
                if (_t.ToString().Contains("(UnityEngine.AudioClip)"))
                {
                    audioStrings[i, 0] = _t.ToString().Replace(" (UnityEngine.AudioClip)", "");
                    audioStrings[i, 1] = allFilePaths[i];
                }
                else
                    _ignored++;
            }

            if (_ignored > 0)
            {
                // This message should never show up, but its here just in-case
                Debug.LogAssertion("* Audio Manager *: " + _ignored + " entries ignored, this is due to the files either been in sub directories or other files that are not Unity AudioClips.");
            }
        }

        
        /// <summary>
        /// Adds all the AudioClips to the AMF.
        /// </summary>
        private static void AddAudioClips(AudioManagerFile file)
        {
            // Makes a new list the size of the amount of objects in the path
            // In V:2.3+ - Updated to allow for custom folders the hold audio clips (so users can organise their clips and still use the manager, default will just get all sounds in "/audio")
            var _allFiles = new List<string>();

            audioList = new List<AudioClip>();
            
            
            for (int i = 0; i < file.directory.Count; i++)
            {
                if (file.directory[i].Equals(""))
                    // 2.4.1 - fixed an issue where the directory order would break the asset finding files.
                    _allFiles.AddRange(new List<string>(Directory.GetFiles(Application.dataPath + BaseAudioFolderLocation)));
                else if (Directory.Exists(Application.dataPath + BaseAudioFolderLocation + "/" + file.directory[i]))
                    // 2.3.1 - adds a range so it adds each directory to the asset 1 by 1.
                    _allFiles.AddRange(new List<string>(Directory.GetFiles(Application.dataPath + BaseAudioFolderLocation + "/" + file.directory[i])));
                else
                {
                    // !Warning Message - shown in the console should there not be a directory named what the user entered
                    Debug.LogWarning("(*Audio Manager*): Path does not exist! please make sure you spelt your path correctly: " + Application.dataPath + BaseAudioFolderLocation + "/" + file.directory[i]);
                }
            }


            // Checks to see if there is anything in the path, if its empty it will not run the rest of the code and instead put a message in the console
            if (_allFiles.Any())
            {
                AudioClip _source;

                foreach (var _thingy in _allFiles)
                {
                    var _path = "Assets" + _thingy.Replace(Application.dataPath, "").Replace('\\', '/');

                    if (!AssetDatabase.LoadAssetAtPath(_path, typeof(AudioClip)))
                    {
                        continue;
                    }
                    
                    _source = (AudioClip)AssetDatabase.LoadAssetAtPath(_path, typeof(AudioClip));
                    audioList.Add(_source);
                }
            }
            else
            {
                // !Warning Message - shown in the console should there be no audio in the directory been scanned
                Debug.LogWarning("(*Audio Manager*): Please ensure there are Audio files in the directory: " +
                                 Application.dataPath + BaseAudioFolderLocation);
            }
        }



        public static bool AreClipsInBaseDirectory(AudioManagerFile file)
        {
            return CheckAmount(file).Equals(0) && noClipDirs.Count <= 0;
        }
        
        
        /// <summary>
        /// Checks to see if there are no directories...
        /// </summary>
        /// <returns>Bool | Whether or not there is a directory in the list.</returns>
        public static bool AreAllDirectoryStringsBlank(AudioManagerFile file)
        {
            var _check = 0;

            if (file.directory == null || file.directory.Count <= 1) return false;
            
            for (var i = 0; i < file.directory.Count; i++)
            {
                if (file.directory[i].Equals(""))
                    ++_check;
            }

            return _check.Equals(file.directory.Count);
        }

        
        /// <summary>
        /// Checks to see if there are directories with the same name (avoid scanning when there is).
        /// </summary>
        /// <returns>Bool | Whether or not there is a duplicate directory in the list.</returns>
        public static bool AreDupDirectories(AudioManagerFile file)
        {
            var _check = 0;

            if (file.directory == null) return false;
            if (file.directory.Count > 0)
            {
                if (file.directory.Count < 2) return false;

                for (var i = 0; i < file.directory.Count; i++)
                {
                    for (var j = 0; j < file.directory.Count; j++)
                    {
                        // avoids checking the same position... as that would be true....
                        if (i.Equals(j)) continue;

                        string dir1, dir2;

                        dir1 = file.directory[i].ToLower();
                        dir2 = file.directory[j].ToLower();

                        dir1 = dir1.Replace("/", "");
                        dir2 = dir2.Replace("/", "");

                        if (dir1.Equals(dir2))
                        {
                            ++_check;
                        }
                    }
                }

                return _check > 0;
            }

            return false;
        }

        
        /// <summary>
        /// Audio Manager Editor Method | gets the number of clips currently in this instance of the Audio Manager.
        /// </summary>
        /// <returns>Int | number of clips in the AMF on this Audio Manager.</returns>
        public static int GetNumberOfClips(AudioManagerFile file)
        {
            // 2.4.1 - fixed an issue where this if statement didn't fire where there was only 1 file, simple mistake xD, previous ">" now ">=".
            if (file.Library != null && file.Library.Count >= 1)
            {
                return file.Library.Count;
            }
            
            return 0;
        }



        public static bool NoFilesInADirectoryCheck => noClipDirs.Count > 0;
        

        public static void ShowNoFilesInDirectoryHelpMessage(AudioManagerFile file)
        {
            if (noClipDirs == null || noClipDirs.Count <= 0) return;
            
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            string _errorString = null;

            if (file.directory.Count != 0)
            {
                _errorString = NoClipsInDirMessage;

                for (int i = 0; i < file.directory.Count; i++)
                {
                    _errorString = _errorString + "assets" + BaseAudioFolderLocation + "/" + file.directory[i] + "\n";
                }
            }
            else
                _errorString = "No clips found in: " + "assets" + BaseAudioFolderLocation;

            EditorGUILayout.HelpBox(_errorString, MessageType.Warning, true);
            EditorGUILayout.EndHorizontal();
        }



        /// <summary>
        /// Shows a variety of help labels when stuff goes wrong, these just explain to the user what has happened and how they should go about fixing it.
        /// </summary>
        private static void HelpLabels(AudioManagerFile file)
        {
            if (file.directory.Count <= 0) return;


            if (file.Library.Count > 0 && file.Library.Count != CheckAmount(file))
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                string _errorString = null;

                if (file.Library.Count != 0)
                {
                    _errorString = NoClipsInDirMessage;

                    for (var i = 0; i < file.directory.Count; i++)
                    {
                        _errorString = _errorString + "assets" + BaseAudioFolderLocation + "/" + file.directory[i] +
                                       "\n";
                    }
                }
                else
                    _errorString = "No clips found in: " + "assets" + BaseAudioFolderLocation;

                EditorGUILayout.HelpBox(_errorString, MessageType.Info, true);
                EditorGUILayout.EndHorizontal();
            }
            else if (file.Library.Count != CheckAmount(file))
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                string _errorString = null;

                if (file.directory.Count != 0)
                {
                    _errorString = NoClipsInDirMessage;

                    for (int i = 0; i < file.directory.Count; i++)
                    {
                        _errorString = _errorString + "assets/" + BaseAudioFolderLocation + "/" + file.directory[i] +
                                       "\n";
                    }
                }
                else
                    _errorString = "No clips found in: " + "assets/" + BaseAudioFolderLocation;

                EditorGUILayout.HelpBox(_errorString, MessageType.Info, true);
                EditorGUILayout.EndHorizontal();
            }
            else if (CheckAmount(file) == 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                string _errorString = null;

                if (file.directory.Count != 0)
                {
                    _errorString = NoClipsInDirMessage;

                    for (int i = 0; i < file.directory.Count; i++)
                    {
                        _errorString = _errorString + "assets/" + BaseAudioFolderLocation + "/" +
                                       file.directory[i] + "\n";
                    }
                }
                else
                    _errorString = "No clips found in: " + "assets/" + BaseAudioFolderLocation;

                EditorGUILayout.HelpBox(_errorString, MessageType.Info, true);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
    }
}