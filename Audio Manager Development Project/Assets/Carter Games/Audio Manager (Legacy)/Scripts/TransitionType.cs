/*
 * 
 *  Audio Manager
 *							  
 *	Transition Type
 *      the types of the transitions that can be used in the music player
 *			
 *  Written by:
 *      Jonathan Carter
 *      E: jonathan@carter.games
 *      W: https://jonathan.carter.games
 *		
 */

namespace CarterGames.Legacy.AudioManager
{
    /// <summary>
    /// Used in the Music Player to define which transition to use.
    /// </summary>
    public enum TransitionType
    {
        None,
        FadeIn,
        FadeOut,
        Fade,
        CrossFade,
    }
}