using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip[] AudioClips_01;
    public AudioSource AudioSource_01;

    public void MMenu_OnMouseOver()
    {
        AudioSource_01.PlayOneShot(AudioClips_01[0]);
    }

    public void MMenu_OnMouseEnterWithoutSwoosh()
    {
        AudioSource_01.PlayOneShot(AudioClips_01[1]);
    }

    public void MMenu_OnMouseEnterWithSwoosh()
    {
        AudioSource_01.PlayOneShot(AudioClips_01[1]);
        AudioSource_01.PlayOneShot(AudioClips_01[2]);
    }

    public void MMenu_ReverseSwoosh()
    {
        AudioSource_01.PlayOneShot(AudioClips_01[3]);
    }
}
