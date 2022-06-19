using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    public class AudioLibrary : AudioManagerAsset
    {
        [SerializeField] private int editorTabPos;
        [SerializeField] private AudioData[] library;
        [SerializeField] private List<GroupData> groups = new List<GroupData>();
        [SerializeField] private string[] allClipNames;
        [SerializeField] private AudioMixerGroup[] mixers;
        
        public bool HasClip(string request) => library.Any(t => t.key.Equals(request));
        public AudioData GetClip(string request) => library.First(t => t.key.Equals(request));
        public int ClipCount => library?.Length ?? 0;
        public AudioData[] GetData => library;
        public AudioMixerGroup GetMixer(string name) => mixers.FirstOrDefault(t => t.name.Equals(name));
        public List<GroupData> Groups
        {
            get => groups;
            set => groups = value;
        }

        public string[] AllClipNames
        {
            get
            {
                if (allClipNames.Length.Equals(library.Length)) return allClipNames;
                allClipNames = library.Select(t => t.key).ToArray();
                return allClipNames;
            }
        }

        public void RemoveGroup(GroupData data)
        {
            Groups.Remove(data);
        }
    }
}