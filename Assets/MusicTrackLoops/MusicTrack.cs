using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Music Track",menuName = "Music Track Data")]
public class MusicTrack : ScriptableObject
{
    [Header("Audio Track Data")]
    [Tooltip("The track's full audio clip")]
    [SerializeField] AudioClip audioTrack;
    [Min(0.0f)]
    [Tooltip("The time where the song's intro segment ends and the loopable segment starts (Leave this at 0 if the music track has no intro segment)")]
    [SerializeField] float loopStartTime = 0.0f;
    [Min(0.0f)]
    [Tooltip("The time where the song's loopable segment ends and the outro segment starts (Leave this at 0 if the music track has no outro)")]
    [SerializeField] float loopEndTime = 0.0f;

    /// <summary>
    /// The intro clip of the audio clip
    /// </summary>
    public AudioClip IntroTrack
    {
        private set;
        get;
    }

    /// <summary>
    /// The loopable clip of the music track
    /// </summary>
    public AudioClip LoopTrack
    {
        private set;
        get;
    }

    /// <summary>
    /// Initializes the music track to be played
    /// </summary>
    public void InitializeAudioTracks()
    {
        if(audioTrack == null)
        {
            Debug.LogWarning("InitializeAudioTracks() was called in a MusicTrack with no AudioTrack assigned! Null AudioTrack Error: " + this.name);
            return;
        }
        // If the intro track  and the loopable track is not null, then ignore this call
        if(IntroTrack != null && LoopTrack != null)
        {
            Debug.LogWarning("Ignored attempted at initializing loopable tracks; Intro and loop audioclips have already been initialized");
            return;
        }
        // If the loop start time is set to 0, then the intro track should be null
        if (loopStartTime <= 0)
        {
            IntroTrack = null;
        }
        // Otherwise, create a sub-clip of the intro track
        else
        {
            IntroTrack = CreateSubClip(audioTrack, audioTrack.name + " - Intro", 0.0f, loopStartTime);
        }
        // Cache the audio clip's full duration
        double audioClipDuration = GetAudioClipDuration(audioTrack);
        // If the loop end timestamp is set to 0, then use the entire audio clip duration 
        // for the end time stamp of the loop segment
        // Otherwise, use the loopend timestamp entered
        float loopEndTimeStamp = loopEndTime <= 0 ? (float) audioClipDuration : loopEndTime;
        // If the audio clip duration is less than the llop end timestamp entered, then
        // prompt an error and warn the user
        if(audioClipDuration < loopEndTimeStamp)
        {
            if((loopEndTime <= 0) == false)Debug.LogError("Loop End Timestamp Setup For " + audioTrack.name + " is Longer Than The Track's Duration! Setting the loop end timestamp to the audio track's end. Loop End Timestamp Entered: " + loopEndTimeStamp + "; Actual Duration of the Track: " + audioClipDuration);
            loopEndTimeStamp = (float) audioClipDuration;
        }
        // Create a subclip for the loopable track
        LoopTrack = CreateSubClip(audioTrack, audioTrack.name + " - Loop", (float) GetAudioClipDuration(IntroTrack), loopEndTimeStamp);
    }

    /// <summary>
    /// Returns the intro track's duration (0 if null)
    /// </summary>
    public double IntroTrackDuration
    {
        get
        {
            return GetAudioClipDuration(IntroTrack);
        }
    }

    /// <summary>
    /// Returns the loop track duration (0 if null)
    /// </summary>
    public double LoopTrackDuration
    {
        get
        {
            return GetAudioClipDuration(LoopTrack);
        }
    }






    /**
* Creates a sub clip from an audio clip based off of the start time
* and the stop time. The new clip will have the same frequency as and channels
* the original.
*/
    private AudioClip CreateSubClip(AudioClip clip, string clipName, float start, float stop)
    {
        // Reference
        // http://answers.unity.com/answers/1383912/view.html
        /* Create a new audio clip */
        // Get the frequency of the audio clip
        int frequency = clip.frequency;
        // Get the time length of our audio clip
        float timeLength = stop - start;
        // Get the count of samples by multiplying the frequency and the time length
        int samplesLength = (int)(frequency * timeLength);
        // Create a new audio clip for our subclip
        AudioClip newClip = AudioClip.Create(clip.name + clipName, samplesLength, clip.channels, frequency, false);
        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength * clip.channels];
        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));
        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);
        newClip.LoadAudioData();
        /* Return the sub clip */
        return newClip;
    }


    /// <summary>
    /// Returns the precise dureation of an audio clip
    /// </summary>
    /// <param name="clip">The audio clip</param>
    /// <returns></returns>
    private static double GetAudioClipDuration(AudioClip clip)
    {
        if (clip == null) return 0;
        return (double)clip.samples / clip.frequency;
    }
}
