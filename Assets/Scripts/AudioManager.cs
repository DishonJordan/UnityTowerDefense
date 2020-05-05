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
    public AudioSource musicAudioSource;
    public AudioClip levelMusic;
    public AudioClip levelCompleteMusicHead;
    public AudioClip levelCompleteMusicBody;
    public float levelCompleteMusicTransitionOffset;

    public void Start()
    {
        musicAudioSource.loop = true;
        musicAudioSource.clip = levelMusic;
        musicAudioSource.Play();
    }

    public void PlayGlobalSFX(AudioClip clip)
    {
        globalSFXAudioSource.PlayOneShot(clip);
    }

    public void PlayLevelCompleteMusic()
    {
        StopAllCoroutines();
        StartCoroutine(_PlayLevelCompleteMusic());
    }

    private IEnumerator _PlayLevelCompleteMusic()
    {
        musicAudioSource.loop = false;
        musicAudioSource.clip = levelCompleteMusicHead;
        musicAudioSource.Stop();
        musicAudioSource.Play();
        yield return new WaitForSeconds(levelCompleteMusicHead.length + levelCompleteMusicTransitionOffset);
        musicAudioSource.clip = levelCompleteMusicBody;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }
}
