// ----------------------------------------------------------------------------
// Transition.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 11/02/2022
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CarterGames.Legacy.AudioManager
{
    [Serializable]
    public class Transition
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private double duration;
        [SerializeField] private double startAt;
        [SerializeField] private double endAt;
        [SerializeField] private Ease ease;
    }
}