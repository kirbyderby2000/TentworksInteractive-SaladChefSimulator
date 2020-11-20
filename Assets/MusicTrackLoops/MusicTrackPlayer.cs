using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrackPlayer : MonoBehaviour
{
    [SerializeField] AudioSource introAudioSource;
    [SerializeField] AudioSource loopAudioSource;

    /// <summary>
    /// The music track currently playing (Note: Null if no track is playing / Assigned while a music track is delayed to be played)
    /// </summary>
    public MusicTrack CurrentlyPlayingTrack
    {
        get;
        private set;
    } = null;

    /// <summary>
    /// Starts playing a music track
    /// </summary>
    /// <param name="musicTrack"></param>
    /// <param name="delayedPlayBack"></param>
    /// <param name="loopMusic"></param>
    public void StartPlayingMusicTrack(MusicTrack musicTrack, float delayedPlayBack = 0.0f, bool loopMusic = true)
    {
        if (musicTrack == null)
        {
            Debug.LogError("StartPlayingMusicTrack() called but no music track was passed!");
            return;
        }

        musicTrack.InitializeAudioTracks();
        if (delayedPlayBack < 0) delayedPlayBack = 0;

        StopAllCoroutines();
        introAudioSource.Stop();
        loopAudioSource.Stop();
        introAudioSource.volume = 1.0f;
        loopAudioSource.volume = 1.0f;
        introAudioSource.clip = musicTrack.IntroTrack;
        introAudioSource.PlayScheduled(AudioSettings.dspTime + delayedPlayBack + 0.1d);
        introAudioSource.loop = false;
        loopAudioSource.clip = musicTrack.LoopTrack;
        loopAudioSource.PlayScheduled(AudioSettings.dspTime + delayedPlayBack  + 0.1d + musicTrack.IntroTrackDuration);
        loopAudioSource.loop = loopMusic;
        CurrentlyPlayingTrack = musicTrack;
        if (loopMusic == false) StartCoroutine(CheckIfMusicIsLooping(delayedPlayBack + 0.1f));
    }

    /// <summary>
    /// Stops playing the current music track playing
    /// </summary>
    /// <param name="delayedStop">Optional delayed time to stop playing the music track</param>
    /// <param name="fadeDuration">Optional fade-out duration to make the music fade-out</param>
    public void StopPlayingMusicTrack(float delayedStop = 0.0f, float fadeDuration = 0.0f)
    {
        if (delayedStop < 0.0f) delayedStop = 0.0f;
        if (fadeDuration < 0.0f) fadeDuration = 0.0f;

        if(delayedStop > 0.0f)
        {
            StartCoroutine(DelayMusicStopCoroutine(delayedStop, fadeDuration));
            return;
        }

        if(fadeDuration == 0.0f)
        {
            introAudioSource.Stop();
            loopAudioSource.Stop();
            introAudioSource.clip = null;
            loopAudioSource.clip = null;
        }
        else
        {
            StartCoroutine(FadeMusicCoroutine(fadeDuration));
        }
    }

    private IEnumerator DelayMusicStopCoroutine(float seconds, float fadeDuration)
    {
        yield return new WaitForSeconds(seconds);
        StopPlayingMusicTrack(0.0f, fadeDuration);
    }

    private IEnumerator FadeMusicCoroutine(float fadeDuration)
    {
        float startTimestamp = Time.time;
        float lerpAMT = 0.0f;
        while(lerpAMT < 1.0f)
        {
            lerpAMT = (Time.unscaledTime - startTimestamp) / lerpAMT;
            if (lerpAMT > 1.0f) lerpAMT = 1.0f;

            introAudioSource.volume = 1.0f - lerpAMT;
            loopAudioSource.volume = 1.0f - lerpAMT;
            yield return null;
        }
        introAudioSource.volume = 0.0f;
        loopAudioSource.volume = 0.0f;
        introAudioSource.Stop();
        loopAudioSource.Stop();
        introAudioSource.clip = null;
        loopAudioSource.clip = null;
        CurrentlyPlayingTrack = null;
        yield return null;
        introAudioSource.volume = 1.0f;
        loopAudioSource.volume = 1.0f;
    }


    private IEnumerator CheckIfMusicIsLooping(float delayedPlaybackStartTime)
    {
        MusicTrack startedPlayingTrack = CurrentlyPlayingTrack;
        float scheduledTime = (float) (CurrentlyPlayingTrack.IntroTrackDuration + CurrentlyPlayingTrack.LoopTrackDuration + 0.19f + delayedPlaybackStartTime);
        yield return new WaitForSeconds(scheduledTime);
        if (loopAudioSource.loop == true) yield break;
        if(startedPlayingTrack == CurrentlyPlayingTrack)
        {
            CurrentlyPlayingTrack = null;
            introAudioSource.Stop();
            loopAudioSource.Stop();
            introAudioSource.clip = null;
            loopAudioSource.clip = null;
            introAudioSource.volume = 1.0f;
            loopAudioSource.volume = 1.0f;
        }
    }

    public void PauseMusic()
    {
        introAudioSource.Pause();
        loopAudioSource.Pause();
    }

    public void UnpauseMusic()
    {
        introAudioSource.UnPause();
        loopAudioSource.UnPause();
    }


}
