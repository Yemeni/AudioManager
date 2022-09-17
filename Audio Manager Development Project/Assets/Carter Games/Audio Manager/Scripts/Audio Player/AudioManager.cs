using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// The main manager class of the asset... used to play clips from...
    /// </summary>
    public static class AudioManager
    {
        //
        //  Fields
        //
        
        
        private static readonly AudioLibrary Library = AssetAccessor.GetAsset<AudioLibrary>();
        private static readonly AudioManagerSettings Settings = AssetAccessor.GetAsset<AudioManagerSettings>();
        private static Dictionary<string, AudioData> _cachedDictionary;
        private static readonly string ActiveClipPrefix = "Clip_";
        
        
        //
        //  Properties
        //
        
        
        private static int ActiveClipCount { get; set; }
        
        
        private static Dictionary<string, AudioData> Dictionary
        {
            get
            {
                if (_cachedDictionary != null) return _cachedDictionary;
                
                var lib = Library.GetData;
                _cachedDictionary = new Dictionary<string, AudioData>();

                foreach (var data in lib)
                    _cachedDictionary.Add(data.key, data);

                return _cachedDictionary;
            }
        }

        
        private static bool HasClip(string request) => Dictionary.ContainsKey(request);


        private static AudioData GetDataFromString(string request)
        {
            if (HasClip(request)) return Dictionary[request];
            AmLog.Error($"Unable to find clip: {request} in the library.");
            return null;
        }
        

        private static AudioData GetRandomClip()
        {
            if (Dictionary.Count <= 0)
            {
                AmLog.Error("Library is empty, unable to play random with no clips to work with.");
                return null;
            }

            var key = Dictionary.Keys.ToArray()[Random.Range(0, Dictionary.Count - 1)];
            return Dictionary[key];
        }


        private static AudioData GetRandomClip(string[] clips)
        {
            if (Dictionary.Count <= 0)
            {
                AmLog.Warning("Library is empty, unable to play random with no clips to work with.");
                return null;
            }

            var key = clips[Random.Range(0, clips.Length - 1)];

            if (HasClip(key)) return Dictionary[key];
            AmLog.Error($"Unable to find clip: <i>\"{key}\"</i> in the library.");
            return null;
        }


        /// <summary>
        /// Sets up a player for use...
        /// </summary>
        /// <returns>AudioClipPlayer setup for use...</returns>
        private static AudioClipPlayer SetupPlayer(AudioData data, bool? variance, string mixer, int? priority, bool? loop)
        {
            var settings = AudioHandler.GenerateSettings(mixer != null ? Library.GetMixer(mixer) : Settings.ClipAudioMixer,
                variance != null && variance.Value ? Mathf.Clamp01(data.baseVolume + Settings.variantVolume) : data.baseVolume,
                variance != null && variance.Value ? Mathf.Clamp(data.basePitch + Settings.variantPitch, -3f, 3f) : data.basePitch, priority, loop);

            var clipPlayer = AudioHandler.SetupClipPlayer(data.value, settings);
            clipPlayer.ClipInfo.SetClipId($"{ActiveClipPrefix}{ActiveClipCount}");
            ActiveClipCount++;
            return clipPlayer;
        }
        
        
        /// <summary>
        /// Sets up a player for use...
        /// </summary>
        /// <returns>AudioClipPlayer setup for use...</returns>
        private static AudioClipPlayer SetupPlayer(AudioData data, float? volume, float? pitch, bool? variance, int? priority, bool? loop)
        {
            var settings = AudioHandler.GenerateSettings(Settings.ClipAudioMixer,
                variance != null && variance.Value ? Mathf.Clamp01(volume ?? data.baseVolume + Settings.variantVolume) : volume,
                variance != null && variance.Value ? Mathf.Clamp(pitch ?? data.basePitch + Settings.variantPitch, -3f, 3f) : pitch, priority, loop);

            var source = AudioHandler.SetupClipPlayer(data.value, settings);
            source.ClipInfo.SetClipId($"{ActiveClipPrefix}{ActiveClipCount}");
            ActiveClipCount++;
            return source;
        }
        
        
        /// <summary>
        /// Sets up a player for use...
        /// </summary>
        /// <returns>AudioClipPlayer setup for use...</returns>
        private static AudioClipPlayer SetupPlayer(AudioData data, AdvancedAudioSettings settings)
        {
            var source = AudioHandler.SetupClipPlayer(data.value, settings);
            source.ClipInfo.SetClipId($"{ActiveClipPrefix}{ActiveClipCount}");
            ActiveClipCount++;
            return source;
        }


        /// <summary>
        /// Sends a log about the audio being disabled.
        /// </summary>
        private static void AudioDisabledLog()
        {
            AmLog.WarningWithCode(2, "Can Play Audio is FALSE, no audio clips will play form the Audio Manager in this state.");
        }
        

        //
        //
        //
        //  Basic Play Methods
        //
        //
        //
        #region Basic Play Audio Methods
        
        /// <summary>
        /// Plays an audio clip.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo Play(string request, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }

            var setup = SetupPlayer(GetDataFromString(request), variance, mixer, priority, loop);
            
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays a random audio clip from the library.
        /// </summary>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayRandom(bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(), variance, mixer, priority, loop));
        }
        
        
        /// <summary>
        /// Plays an audio clip from the specified time.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="time">The time where the clip should start playing (in seconds)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayFromTime(string request, float time, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), variance, mixer, priority, loop);
            setup.AudioSource.time = time;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip after the specified delay.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="delay">The amount of time to wait before playing (in seconds)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayWithDelay(string request, float delay, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetDataFromString(request), variance, mixer, priority, loop), delay);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector2)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Vector2 position, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, (Vector3) position, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector3)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Vector3 position, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), variance, mixer, priority, loop);
            setup.transform.position = position;
            var source = AudioHandler.PlayClip(setup);
            return source;
        }
        
        
        /// <summary>
        /// Plays an audio clip at the position of a particular GameObject.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="gameObject">The gameObject location to play at.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, GameObject gameObject, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, gameObject.transform.position, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the position of the transform.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="transform">The Transform to play at.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Transform transform, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, transform.position, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip as a child of the gameObject defined.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="gameObject">The gameObject to attach to.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string request, GameObject gameObject, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayOnObject(request, gameObject.transform, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip as a child of the transform defined.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="transform">The transform to attach to.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string request, Transform transform, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), variance, mixer, priority, loop);
            setup.transform.SetParent(transform);
            return AudioHandler.PlayClip(setup);
        }
        
        #endregion
        
        #region Basic Play Audio Methods (Edit Volume/Pitch In Code)
        
        /// <summary>
        /// Plays an audio clip.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo Play(string request, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetDataFromString(request), volume, pitch, variance, priority, loop));
        }
        
        
        /// <summary>
        /// Plays a random audio clip from the library.
        /// </summary>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayRandom(float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(), volume, pitch, variance, priority, loop));
        }
        
        
        /// <summary>
        /// Plays an audio clip from the specified time.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="time">The time where the clip should start playing (in seconds)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayFromTime(string request, float time, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), volume, pitch, variance, priority, loop);
            setup.AudioSource.time = time;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip after the specified delay.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="delay">The amount of time to wait before playing (in seconds)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayWithDelay(string request, float delay, float? volume = 1f, bool? variance = false, float? pitch = 1f, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(), volume, pitch, variance, priority, loop), delay);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector2)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Vector2 position, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, (Vector3) position, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector3)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Vector3 position, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), volume, pitch, variance, priority, loop);
            setup.transform.position = position;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="gameObject">The gameObject location to play at.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, GameObject gameObject, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, gameObject.transform.position, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="transform">The Transform to play at.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Transform transform, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, transform.position, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip on the gameObject defined.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="gameObject">The gameObject to attach to.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string request, GameObject gameObject, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayOnObject(request, gameObject.transform, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip as a child of the transform defined.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="transform">The gameObject to attach to.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string request, Transform transform, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), volume, pitch, variance, priority, loop);
            setup.transform.SetParent(transform);
            return AudioHandler.PlayClip(setup);
        }
        
        #endregion

        #region Basic Multi Clip Play Audio Methods
        
        /// <summary>
        /// Plays a random clip from the requests provided.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo Play(string[] request, bool? variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(request), variance, mixer, priority, loop));
        }

        
        /// <summary>
        /// Plays a random clip from the requests provided from the specified time.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="time">The time where the clip should start playing (in seconds)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayFromTime(string[] request, float time, bool? variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetRandomClip(request), variance, mixer, priority, loop);
            setup.AudioSource.time = time;
            return AudioHandler.PlayClip(setup);
        }

        
        /// <summary>
        /// Plays a random clip from the requests provided after the specified delay.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="delay">The amount of time to wait before playing (in seconds)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayWithDelay(string[] request, float delay, bool? variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(request), variance, mixer, priority, loop), delay);
        }


        /// <summary>
        /// Plays a random clip from the requests provided at the requested position.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector2)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string[] request, Vector2 position, bool? variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, (Vector3) position, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays a random clip from the requests provided at the requested position.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector2)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string[] request, Vector3 position, bool? variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetRandomClip(request), variance, mixer, priority, loop);
            setup.transform.position = position;
            var source = AudioHandler.PlayClip(setup);
            return source;
        }
        
        
        /// <summary>
        /// Plays a random clip from the requests provided at the position of a particular GameObject.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="gameObject">The gameObject location to play at.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string[] request, GameObject gameObject, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, gameObject.transform.position, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays a random clip from the requests provided at the position of the transform.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="transform">The Transform to play at.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string[] request, Transform transform, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, transform.position, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays a random clip from the requests provided as a child of the gameObject defined.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="gameObject">The gameObject to attach to.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string[] request, GameObject gameObject, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            return PlayOnObject(request, gameObject.transform, variance, mixer, priority, loop);
        }
        
        
        /// <summary>
        /// Plays a random clip from the requests provided as a child of the transform defined.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="transform">The transform to attach to.</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string[] request, Transform transform, bool variance, string mixer = null, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetRandomClip(request), variance, mixer, priority, loop);
            setup.transform.SetParent(transform);
            return AudioHandler.PlayClip(setup);
        }
        
        #endregion
        
        #region Basic Multi Clip Play Audio Methods (Edit Volume/Pitch In Code)
        
        /// <summary>
        /// Plays a random clip from the requests provided.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo Play(string[] request, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(request), volume, pitch, variance, priority, loop));
        }

        
        /// <summary>
        /// Plays a random clip from the requests provided from the specified time.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="time">The time where the clip should start playing (in seconds)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayFromTime(string[] request, float time, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetRandomClip(request), volume, pitch, variance, priority, loop);
            setup.AudioSource.time = time;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip after the specified delay.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="delay">The amount of time to wait before playing (in seconds)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns> 
        public static AudioClipInfo PlayWithDelay(string[] request, float delay, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(), volume, pitch, variance, priority, loop), delay);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector2)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string[] request, Vector2 position, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, (Vector3) position, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector3)</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string[] request, Vector3 position, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetRandomClip(request), volume, pitch, variance, priority, loop);
            setup.transform.position = position;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="gameObject">The gameObject location to play at.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtObject(string[] request, GameObject gameObject, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, gameObject.transform.position, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="transform">The Transform to play at.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtObject(string[] request, Transform transform, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayAtPosition(request, transform.position, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip on the gameObject defined.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="gameObject">The gameObject to attach to.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string[] request, GameObject gameObject, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            return PlayOnObject(request, gameObject.transform, volume, pitch, variance, priority, loop);
        }
        
        
        /// <summary>
        /// Plays an audio clip as a child of the transform defined.
        /// </summary>
        /// <param name="request">The clips to choose from to play (Use Group.???? to get a group of clips accurately)</param>
        /// <param name="transform">The gameObject to attach to.</param>
        /// <param name="volume">The volume to play the clip at (overrides the base volume set for the clip with this)</param>
        /// <param name="pitch">The pitch to play the clip at (overrides the base pitch set for the clip with this)</param>
        /// <param name="variance">Should the clip vary its volume & pitch?</param>
        /// <param name="priority">The priority the clip is played at</param>
        /// <param name="loop">Should the clip loop?</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string[] request, Transform transform, float? volume = 1f, float? pitch = 1f, bool? variance = false, int? priority = 128, bool? loop = false)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetRandomClip(request), volume, pitch, variance, priority, loop);
            setup.transform.SetParent(transform);
            return AudioHandler.PlayClip(setup);
        }
        
        #endregion

        
        //
        //
        //
        //  Advanced Play Methods
        //
        //
        //
        #region Advanced Play Audio Methods
        
        /// <summary>
        /// Plays an audio clip.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo Play(string request, AdvancedAudioSettings settings)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetDataFromString(request), settings));
        }
        
        
        /// <summary>
        /// Plays a random audio clip from the library.
        /// </summary>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayRandom(AdvancedAudioSettings settings)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(), settings));
        }
        
        
        /// <summary>
        /// Plays an audio clip from the specified time.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="time">The time where the clip should start playing (in seconds)</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayFromTime(string request, float time, AdvancedAudioSettings settings)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), settings);
            setup.AudioSource.time = time;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip after the specified delay.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="delay">The amount of time to wait before playing (in seconds)</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayWithDelay(string request, float delay, AdvancedAudioSettings settings)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            return AudioHandler.PlayClip(SetupPlayer(GetRandomClip(), settings), delay);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector2)</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Vector2 position, AdvancedAudioSettings settings)
        {
            return PlayAtPosition(request, (Vector3) position, settings);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="position">The position you want to play the clip at (Vector3)</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Vector3 position, AdvancedAudioSettings settings)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), settings);
            setup.transform.position = position;
            return AudioHandler.PlayClip(setup);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="gameObject">The gameObject location to play at.</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, GameObject gameObject, AdvancedAudioSettings settings)
        {
            return PlayAtPosition(request, gameObject.transform.position, settings);
        }
        
        
        /// <summary>
        /// Plays an audio clip at the requested position.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="transform">The Transform to play at.</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayAtPosition(string request, Transform transform, AdvancedAudioSettings settings)
        {
            return PlayAtPosition(request, transform.position, settings);
        }
        
        
        /// <summary>
        /// Plays an audio clip on the gameObject defined.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="gameObject">The gameObject to attach to.</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string request, GameObject gameObject, AdvancedAudioSettings settings)
        {
            return PlayOnObject(request, gameObject.transform, settings);
        }
        
        
        /// <summary>
        /// Plays an audio clip as a child of the transform defined.
        /// </summary>
        /// <param name="request">The clip the play (Use Clip.???? to get a clip accurately)</param>
        /// <param name="transform">The gameObject to attach to.</param>
        /// <param name="settings">The complete audio settings to use on the clip.</param>
        /// <returns>Info about the clip playing</returns>
        public static AudioClipInfo PlayOnObject(string request, Transform transform, AdvancedAudioSettings settings)
        {
            if (!Settings.CanPlayAudio)
            {
                AudioDisabledLog();
                return null;
            }
            
            var setup = SetupPlayer(GetDataFromString(request), settings);
            setup.transform.SetParent(transform);
            return AudioHandler.PlayClip(setup);
        }
        
        #endregion


        //
        //
        //
        //  Stop Methods
        //
        //
        //
        #region Stop Audio Methods
        
        /// <summary>
        /// Stops the first clip of the name requested.
        /// </summary>
        /// <param name="request">The clip to find</param>
        /// <returns>If it was successful</returns>
        public static bool Stop(string request)
        {
            var clip = AudioPool.ActiveObjects.FirstOrDefault(t => t.AudioSource.clip.name.Equals(request));
            if (clip == null)
            {
                AmLog.Warning($"Unable to find clip: <i>\"{request}\"</i> to stop.");
                return false;
            }

            clip.AudioSource.Stop();
            return true;
        }
        
        
        /// <summary>
        /// Stops the first clip of the id requested.
        /// </summary>
        /// <param name="id">The id to find</param>
        /// <returns>If it was successful</returns>
        public static bool StopById(string id)
        {
            var clip = AudioPool.ActiveObjects.FirstOrDefault(t => t.ClipInfo.ClipID.Equals(id));
            if (clip == null)
            {
                AmLog.Warning($"Unable to find clip using ID: <i>\"{id}\"</i>.");
                return false;
            }
            
            clip.AudioSource.Stop();
            return true;
        }

        
        /// <summary>
        /// Stops the first clip using the entered info requested.
        /// </summary>
        /// <param name="clipInfo">The clip info to find</param>
        /// <returns>If it was successful</returns>
        public static bool Stop(AudioClipInfo clipInfo)
        {
            var clip = AudioPool.ActiveObjects.FirstOrDefault(t => t.AudioSource.Equals(clipInfo.AudioSource));
            if (clip == null)
            {
                AmLog.Warning("Unable to find clip of the clip info provided to stop.");
                return false;
            }
            
            clip.AudioSource.Stop();
            return true;
        }


        /// <summary>
        /// Stops all audio clips that are actively being played...
        /// </summary>
        /// <remarks>Ignores Music System.</remarks>
        public static void StopAll()
        {
            foreach (var obj in AudioPool.ActiveObjects)
                obj.AudioSource.Stop();

            AudioPool.ActiveObjects.Clear();
        }

        #endregion
    }
}