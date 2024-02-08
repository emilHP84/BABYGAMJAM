using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound access;
    AudioSource source;

    void Awake()
    {
        access = this;
        source = GetComponent<AudioSource>();
    }
    public void Play(AudioClip desired)
    {
        source.PlayOneShot(desired);
    }

    public void Play(AudioClip desired, float volume)
    {
        source.PlayOneShot(desired,volume);
    }
    
    public void PlayWithDelay(AudioClip desired, float volume, float delay)
    {
        StopAllCoroutines();
        StartCoroutine(WaitToPlay(desired,volume,delay));
    }

    IEnumerator WaitToPlay(AudioClip desired, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(desired,volume);
    }

}
