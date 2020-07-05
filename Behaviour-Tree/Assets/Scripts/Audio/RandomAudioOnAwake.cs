using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioOnAwake : RandomAudio
{
 
    void Awake()
    {
        
        AudioSource source = this.gameObject.GetComponent<AudioSource>();

        source.PlayOneShot(RandomAudioClip(randomClip));

    }
   

}
