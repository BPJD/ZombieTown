using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource[] audioSources = new AudioSource[2];
    public AudioClip[] clips = new AudioClip[3];

    public float fadeSpeed = 2f;

    bool isPlaying = false;

    int playingSource = 0;

    void Start()
    {
        MusicPlay(0);
    }

    IEnumerator FadeIn()
    {
        while (audioSources[playingSource].volume < 1)
        {
            audioSources[playingSource].volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while (audioSources[playingSource].volume > 0)
        {
            audioSources[playingSource].volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    public void MusicPlay(int _time)
    {
        playingSource = 1 - playingSource;
        if (isPlaying)
        {
            StartCoroutine(FadeOut());
            isPlaying = false;
        }
        else
        {
            audioSources[playingSource].clip = clips[_time];
            audioSources[playingSource].Play();
            StartCoroutine(FadeIn());
            isPlaying = true;
        }
        



    }


}
