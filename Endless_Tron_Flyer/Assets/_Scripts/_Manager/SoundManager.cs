using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip[] AudioClips_01;

    public AudioSource AudioSource_01;
    public AudioSource MusicSource_01;
    public AudioSource MusicSource_02;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int i = Random.Range(0, 1);
        if (i == 1) MusicSource_01.Play();
        else MusicSource_02.Play();
    }

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
