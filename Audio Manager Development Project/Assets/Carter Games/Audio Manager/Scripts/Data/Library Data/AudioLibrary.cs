using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Holds all the clips
    /// </summary>
    public class AudioLibrary : AudioManagerAsset
    {
        //
        //  Fields
        //
        
        
        [SerializeField] private int editorTabPos;
        [SerializeField] private AudioData[] library;
        [SerializeField] private List<GroupData> groups = new List<GroupData>();
        [SerializeField] private string[] allClipNames;
        [SerializeField] private AudioMixerGroup[] mixers;
        [SerializeField] private List<CustomTransition> customTransitions;
        
        
        //
        //  Properties
        //
        
        
        /// <summary>
        /// Gets the number of clips stored in the library...
        /// </summary>
        public int ClipCount => library?.Length ?? 0;
        
        
        /// <summary>
        /// Gets the clip data stored in the library...
        /// </summary>
        public AudioData[] GetData => library;
        
        
        /// <summary>
        /// Gets the groups stored in the library...
        /// </summary>
        public List<GroupData> Groups
        {
            get => groups;
            set => groups = value;
        }

        
        /// <summary>
        /// Gets the names of all the clips in the library...
        /// </summary>
        public string[] AllClipNames
        {
            get
            {
                if (allClipNames.Length.Equals(library.Length)) return allClipNames;
                allClipNames = library.Select(t => t.key).ToArray();
                return allClipNames;
            }
        }

        
        //
        //  Methods
        //
        
        
        /// <summary>
        /// Removes a group fro the library...
        /// </summary>
        /// <param name="data"></param>
        public void RemoveGroup(GroupData data) => Groups.Remove(data);


        /// <summary>
        /// Does the library contain the clip...
        /// </summary>
        /// <param name="request">The clip to find...</param>
        /// <returns>Bool</returns>
        public bool HasClip(string request)
        {
            // library.Any(t => t.key.Equals(request));
            for (var i = 0; i < library.Length; i++)
            {
                if (!library[i].key.Equals(request)) continue;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Gets the clip requested if it is possible...
        /// </summary>
        /// <param name="request">The clip to find...</param>
        /// <returns>The audio data received...</returns>
        public AudioData GetClip(string request)
        {
            // library.First(t => t.key.Equals(request));
            for (var i = 0; i < library.Length; i++)
            {
                if (!library[i].key.Equals(request)) continue;
                return library[i];
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets the mixer of the entered name...
        /// </summary>
        /// <param name="name">The name to find...</param>
        /// <returns>The mixer group...</returns>
        public AudioMixerGroup GetMixer(string name) 
        {
            // mixers.FirstOrDefault(t => t.name.Equals(name));
            for (var i = 0; i < mixers.Length; i++)
            {
                if (!mixers[i].name.Equals(name)) continue;
                return mixers[i];
            }

            return null;
        }
    }
}