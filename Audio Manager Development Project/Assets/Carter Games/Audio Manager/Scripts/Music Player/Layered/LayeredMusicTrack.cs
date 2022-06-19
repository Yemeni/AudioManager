using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [CreateAssetMenu(menuName = "Carter Games/Audio Manager/Music/Layered Track", fileName = "New Layered Music Track")]
    public class LayeredMusicTrack : AudioManagerAsset
    {
        [SerializeField] private LayeredTrackInfo[] tracks;
    }
}