using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private bool musicEnabled;

    [SerializeField] MusicPlayer musicPlayer;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("music"))
        {
            // Initialize PlayerPrefs values
            PlayerPrefs.SetInt("sfx", 1);
            PlayerPrefs.SetInt("music", 1);

            // Save the changes
            PlayerPrefs.Save();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        

        // set music enabled based on PlayerPrefs
        if (PlayerPrefs.GetInt("music") == 1)
        {
            musicEnabled = true;
            musicPlayer.gameObject.SetActive(true);
        } else
        {
            musicEnabled = false;
            musicPlayer.gameObject.SetActive(false);
        }       
    }

    public void EnableOrDisableMusic()
    {
        if (!musicEnabled)
        {
            //Enabling music
            PlayerPrefs.SetInt("music", 1);
            musicEnabled = true;
            musicPlayer.gameObject.SetActive(true);
        }
        else if (musicEnabled)
        {
            //Disabling music
            PlayerPrefs.SetInt("music", 0);
            musicEnabled = false;
            musicPlayer.StopMusicWithFadeOut();
        }
        
    }

    public AudioMixer audioMixer; // Reference to your Audio Mixer

    // Method to set the volume
    public void EnableSFX()
    {
        PlayerPrefs.SetInt("sfx", 1);
        audioMixer.SetFloat("sfxVol", 0f);
    }
    public void DisableSFX()
    {
        PlayerPrefs.SetInt("sfx", 0);
        audioMixer.SetFloat("sfxVol", -80f);
    }

    public AudioSource cubeMovingSound;
    public AudioSource combineSFX;
}