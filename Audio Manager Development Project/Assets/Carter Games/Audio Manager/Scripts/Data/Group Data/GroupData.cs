using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Defines a group of clips that can be used with the manager...
    /// </summary>
    [Serializable]
    public class GroupData
    {
        //
        //  Fields
        //
        
        
        [SerializeField] private string groupName;              // The name of the group...
        [SerializeField] private List<string> clipNames;        // The clip names the group contains...
        
        
        //
        //  Properties
        //
        
        
        /// <summary>
        /// The state of the dropdown of this group in the editor...
        /// </summary>
        public bool EditorDropDownState { get; set; }

        
        /// <summary>
        /// Gets/Sets the name of the group...
        /// </summary>
        public string GroupName
        {
            get => groupName;
            set => groupName = value;
        }

        
        /// <summary>
        /// Gets the clips in the group...
        /// </summary>
        public List<string> Clips => clipNames;
        
        
        /// <summary>
        /// Returns if the clip exists in the group...
        /// </summary>
        /// <param name="clipName">The clip name to search for...</param>
        /// <returns>The result...</returns>
        public bool HasClip(string clipName) => clipName.Contains(clipName);
        
        
        /// <summary>
        /// Sets the clips stored in this group to the entered data...
        /// </summary>
        /// <param name="clips"></param>
        public void SetClips(List<string> clips) => clipNames = clips;
        

        //
        //  Constructors
        //
        
        
        /// <summary>
        /// Creates a new group data with the name of the group...
        /// </summary>
        /// <param name="groupName">The name for the group...</param>
        public GroupData(string groupName)
        {
            this.groupName = groupName;
            clipNames = new List<string>();
        }
        
        
        /// <summary>
        /// Creates a new group data with the name & data entered...
        /// </summary>
        /// <param name="groupName">The name for the group...</param>
        /// <param name="clips">The clips to entered...</param>
        public GroupData(string groupName, string[] clips)
        {
            this.groupName = groupName;
            clipNames = clips.ToList();
        }
    }
}