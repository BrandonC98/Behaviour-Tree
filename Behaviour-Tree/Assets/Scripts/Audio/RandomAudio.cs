using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{

    [SerializeField]
    protected AudioClip[] randomClip;

    public AudioClip RandomAudioClip(AudioClip[] clips)
    {

        int clipNum = Random.Range(0, clips.Length - 1);
        return clips[clipNum];
        
    }

    public AudioClip RandomAudioClip()
    {

        int clipNum = Random.Range(0, randomClip.Length - 1);
        return randomClip[clipNum];
        
    }

}
