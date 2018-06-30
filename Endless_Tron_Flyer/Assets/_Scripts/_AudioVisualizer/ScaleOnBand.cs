using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnBand : MonoBehaviour
{
    public int _band;
    public float _startScale, _maxScale;
    public bool _useBuffer;
    Material _material;
    public float _red, _green, _blue;
    // Use this for initialization
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
        _band = Random.Range(0, 7);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_useBuffer)
        {
            transform.localScale = new Vector3((AudioPeer._audioBand[_band] * _maxScale) + _startScale, (AudioPeer._audioBand[_band] * _maxScale) + _startScale, (AudioPeer._audioBand[_band] * _maxScale) + _startScale);
            Color _color = new Color(_red * AudioPeer._audioBand[_band], _green * AudioPeer._audioBand[_band], _blue * AudioPeer._audioBand[_band]);
            _material.SetColor("_EmissionColor", _color);
        }
        if (_useBuffer)
        {
            if (float.IsNaN(transform.localScale.y) == false)
            {
                transform.localScale = new Vector3(transform.localScale.x, (AudioPeer._audioBandBuffer[_band] * _maxScale) + _startScale, transform.localScale.z);
            }
            //Color _color = new Color(_red * AudioPeer._audioBandBuffer[_band], _green * AudioPeer._audioBandBuffer[_band], _blue * AudioPeer._audioBandBuffer[_band]);
            //_material.SetColor("_EmissionColor", _color);
        }
    }
}
