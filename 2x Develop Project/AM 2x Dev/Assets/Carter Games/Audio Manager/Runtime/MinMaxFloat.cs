using System;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public class MinMaxFloat
    {
        public float min;
        public float max;


        public float Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}