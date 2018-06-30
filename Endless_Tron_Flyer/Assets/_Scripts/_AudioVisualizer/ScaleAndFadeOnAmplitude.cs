using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAndFadeOnAmplitude : MonoBehaviour
{
    Material _material;
    float FadeInTime;
    float FadeOutTime;
    bool fadingIn = false;
    bool fadingOut = false;
    public float ActivateCD;
    bool isReady = true;
    public float _cylinderSize;
    public float _cylinderThickness;
    public float _fadeOutTime;
    public float StartCD;

    // Use this for initialization
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
        StartCoroutine(StartCooldown(StartCD));
    }

    void Update()
    {
        
        if (!fadingIn && !fadingOut && isReady && AudioPeer._Amplitude > 0.6f)
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
            transform.localScale = new Vector3(_cylinderThickness, 0, _cylinderThickness);
            StartCoroutine(Cooldown(ActivateCD));
        }
    }

    void FadeIn()
    {

        Color _color = new Color(_material.color.r, _material.color.g, _material.color.b, Mathf.Lerp(0, 1, FadeInTime));
        transform.localScale = new Vector3(_cylinderThickness, _cylinderSize, _cylinderThickness);
        FadeInTime += 20f * Time.deltaTime;
        _material.color = _color;
    }

    void FadeOut()
    {
        Color _color = new Color(_material.color.r, _material.color.g, _material.color.b, Mathf.Lerp(1, 0, FadeOutTime));
        transform.localScale = new Vector3(Mathf.SmoothStep(_cylinderThickness, 0, FadeOutTime), _cylinderSize, Mathf.SmoothStep(_cylinderThickness, 0, FadeOutTime));
        FadeOutTime += _fadeOutTime * Time.deltaTime;
        _material.color = _color;
    }

    IEnumerator Cooldown(float ActivateCD)
    {
        isReady = false;
        yield return new WaitForSeconds(ActivateCD);
        isReady = true;
    }

    IEnumerator StartCooldown(float StartCD)
    {
        isReady = false;
        yield return new WaitForSeconds(StartCD);
        isReady = true;
    }
}
