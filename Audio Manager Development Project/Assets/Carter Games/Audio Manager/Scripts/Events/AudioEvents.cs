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
        public static readonly Evt<AudioClipInfo> OnClipFinishedPlaying = new Evt<AudioClipInfo>();
        public static readonly Evt<AudioClipInfo> OnClipPaused = new Evt<AudioClipInfo>();
        
        
        //
        //
        //  Music Player Events
        //
        //
        public static readonly Evt OnTrackStarted = new Evt();
        public static readonly Evt OnTrackEnded = new Evt();
        public static readonly Evt OnTrackLooped = new Evt();
        public static readonly Evt OnTrackChanged = new Evt();
        public static readonly Evt OnTrackTransitionComplete = new Evt();
    }
}