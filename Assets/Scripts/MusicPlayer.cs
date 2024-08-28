using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] tracks; // Array of music tracks to play
    public float fadeDuration = 2.0f; // Duration for the fade out

    private int currentTrackIndex = -1; // To keep track of the current track index
    private bool isFadingOut = false; // To check if fading out is in progress



    void OnEnable()
    {
        StopAllCoroutines();
        audioSource.volume = 1.0f;
        PlayNextTrack();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        audioSource.Stop();
    }

    void PlayNextTrack()
    {
        if (tracks.Length == 0) return;

        if (tracks.Length > 1)
        {
            int newTrackIndex;
            do
            {
                newTrackIndex = Random.Range(0, tracks.Length);
            } while (newTrackIndex == currentTrackIndex);

            currentTrackIndex = newTrackIndex;
        } else
        {
            currentTrackIndex = 0;
        }
        
        audioSource.clip = tracks[currentTrackIndex];
        audioSource.Play();
        StartCoroutine(PlayNextWhenCurrentEnds());
    }

    IEnumerator PlayNextWhenCurrentEnds()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        if (!isFadingOut)
        {
            PlayNextTrack();
        }
    }

    public void StopMusicWithFadeOut()
    {
        if (!isFadingOut)
        {
            StartCoroutine(FadeOutAndStop());
        }
    }

    IEnumerator FadeOutAndStop()
    {
        isFadingOut = true;
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
        gameObject.SetActive(false); // Disable the gameObject after fading out
        isFadingOut = false;
    }
}
