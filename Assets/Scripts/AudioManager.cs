using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _ins;

    public static AudioManager instance
    {
        get
        {
            if(_ins == null)
            {
                _ins = FindObjectOfType<AudioManager>();
                if(_ins == null)
                {
                    Debug.LogError("Could not find Audio Manager instance!");
                }
            }
            return _ins;
        }
    }

    public AudioSource globalSFXAudioSource;

    public void PlayGlobalSFX(AudioClip clip)
    {
        globalSFXAudioSource.PlayOneShot(clip);
    }
}
