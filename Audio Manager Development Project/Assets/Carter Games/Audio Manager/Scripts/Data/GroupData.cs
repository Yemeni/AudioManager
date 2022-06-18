using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public class GroupData
    {
        [SerializeField] private string groupName;
        [SerializeField] private List<string> clipNames;
        public bool EditorDropDownState { get; set; }

        public string GroupName
        {
            get => groupName;
            set => groupName = value;
        }

        public List<string> Clips => clipNames;
        public bool HasClip(string clipName) => clipName.Contains(clipName);
        public void SetClips(List<string> clips) => clipNames = clips;
        

        public GroupData(string groupName)
        {
            this.groupName = groupName;
            clipNames = new List<string>();
        }
        
        public GroupData(string groupName, string[] clips)
        {
            this.groupName = groupName;
            clipNames = clips.ToList();
        }
    }
}