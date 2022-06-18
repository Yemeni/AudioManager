namespace CarterGames.Assets.AudioManager
{
    public static class AudioEvents
    {
        //
        //
        //  Audio Management Events
        //
        //
        public static readonly Evt<AudioClipInfo> OnClipPlayed = new Evt<AudioClipInfo>();
        public static readonly Evt<AudioClipInfo> OnClipStopped = new Evt<AudioClipInfo>();
        
        
        //
        //
        //  Music Player Events
        //
        //
        public static readonly Evt OnTrackStarted;
        public static readonly Evt OnTrackEnded;
        public static readonly Evt OnTrackLooped;
        public static readonly Evt OnTrackChanged;
        public static readonly Evt OnTrackTransitionComplete;
    }
}