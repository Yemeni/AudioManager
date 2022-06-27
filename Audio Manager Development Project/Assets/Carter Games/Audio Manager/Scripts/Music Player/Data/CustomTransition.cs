using System;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public struct CustomTransition
    {
        public int uniqueId;
        public string id;
        public AnimationCurve curve;
        public CustomFadeTypes fadeType;
    }
}