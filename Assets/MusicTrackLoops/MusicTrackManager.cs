using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrackManager : MonoBehaviour
{
    [Header("Starting Song Playback Options")]
    [Tooltip("Whether or not a song should be played on awake")]
    [SerializeField] bool playSongOnAwake = false;
    [Tooltip("The song to play on awake")]
    [SerializeField] MusicTrack songToPlayOnAwake;
    [SerializeField] float awakeSongDelay = 5.0f;
    [Tooltip("Whether or not the song to play on awake should loop")]
    [SerializeField] bool loopSongToPlayOnAwake = true;
    [Header("Alternating Music Player References")]
    [SerializeField] MusicTrackPlayer PlayerA;
    [SerializeField] MusicTrackPlayer PlayerB;

    MusicTrackPlayer currentlyActiveTrack;

    public static MusicTrackManager MusicPlayerManager
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(MusicPlayerManager != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            MusicPlayerManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        if(playSongOnAwake == true)
        {
            if(songToPlayOnAwake == null)
            {
                Debug.LogWarning("PlaySongOnAwake was setup on a MusicTrackManager but a music track wasn't assigned to be played on awake!");
            }
            else
            {
                TransitionIntoMusicTrack(songToPlayOnAwake, awakeSongDelay, loopSongToPlayOnAwake);
            }
        }
    }

    /// <summary>
    /// Method called to transition into another music track
    /// </summary>
    /// <param name="track">The music track to play</param>
    /// <param name="transitionDuration">The transition duration (fades out the current song and plays the next track / Set this to 0.0f if the song should be played instantly)</param>
    /// <param name="loopTrack">Whether or not the music track should loop</param>
    public void TransitionIntoMusicTrack(MusicTrack track, float transitionDuration = 0.0f, bool loopTrack = true)
    {
        // If the currently active track is null, then assign playerA to the active track
        if (currentlyActiveTrack == null) currentlyActiveTrack = PlayerA;
        // If the transition duration is less than 0, reset it to 0
        if (transitionDuration < 0) transitionDuration = 0;
        // If the currently active track player is not playing anything, then
        // just have it play this track assigned
        if (currentlyActiveTrack.CurrentlyPlayingTrack == null)
        {
            currentlyActiveTrack.StartPlayingMusicTrack(track, transitionDuration, loopTrack);
        }
        //Otherwise, stop playing the current track, fade out the track with the transition duration
        // Schedule the 
        else
        {
            currentlyActiveTrack.StopPlayingMusicTrack(fadeDuration:transitionDuration);
            currentlyActiveTrack = currentlyActiveTrack == PlayerA ? PlayerB : PlayerA;
            currentlyActiveTrack.StartPlayingMusicTrack(track, delayedPlayBack: transitionDuration, loopMusic: loopTrack);
        }
    }








}
