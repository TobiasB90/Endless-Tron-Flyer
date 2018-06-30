using System.Collections;
using UnityEngine;

public class FloorFadeOnAudio : MonoBehaviour
{
    [HideInInspector] public bool fadingIn = false;
    [HideInInspector] public bool fadingOut = false;
    [HideInInspector] public bool isReady = true;

    public int _band;
    public float ActivateCD;
    public bool ChangeBandRandomly = true;
    public float ChangingbandCD;

    Material _material;
    float FadeInTime;
    float FadeOutTime;
    bool _changingband = false;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
        if (ChangeBandRandomly == true)
        {
            _band = Random.Range(0, 7);
        }
    }
    
    void Update()
    {
        if (_changingband == false && ChangeBandRandomly == true && !fadingIn && !fadingOut)
        {
            _band = Random.Range(0, 7);
            StartCoroutine(ChangingBand(ChangingbandCD));
        }

        if (!fadingIn && !fadingOut && isReady && AudioPeer._audioBandBuffer[_band] > 0.2f)
        {
            fadingIn = true;
        }

        if (!fadingIn && !fadingOut)
            return;

        if (fadingIn)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }

        if (_material.color.a == 1)
        {
            FadeInTime = 0;
            fadingIn = false;
            fadingOut = true;
        }

        if (_material.color.a == 0)
        {
            FadeOutTime = 0;
            fadingOut = false;
        }

        if (!fadingIn && !fadingOut)
        {
            StartCoroutine(Cooldown(ActivateCD));
        }
    }
    
    void FadeIn()
    {
        
        Color _color = new Color(_material.color.r, _material.color.g, _material.color.b, Mathf.Lerp(0, 1, FadeInTime));
        FadeInTime += 20f * Time.deltaTime;
        _material.color = _color;
    }

    void FadeOut()
    {
        Color _color = new Color(_material.color.r, _material.color.g, _material.color.b, Mathf.Lerp(1, 0, FadeOutTime));
        FadeOutTime += 1f * Time.deltaTime;
        _material.color = _color;
    }

    IEnumerator ChangingBand(float changingbandCD)
    {
        _changingband = true;
        yield return new WaitForSeconds(changingbandCD);
        _changingband = false;
    }

    IEnumerator Cooldown(float ActivateCD)
    {
        isReady = false;
        yield return new WaitForSeconds(ActivateCD);
        isReady = true;
    }
}