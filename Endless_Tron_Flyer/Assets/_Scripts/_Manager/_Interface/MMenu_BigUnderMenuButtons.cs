using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MMenu_BigUnderMenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private SoundManager SoundMng;
    public float ScaleValue;
    public float ScalingDuration;

    void start()
    {
        SoundMng = GameObject.Find("_SoundManager").GetComponent<SoundManager>();
    }

    void Awake()
    {
        SoundMng = GameObject.Find("_SoundManager").GetComponent<SoundManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(ScaleValue, ScaleValue, 1), ScalingDuration);
        SoundMng.MMenu_OnMouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(1, 1, 1), ScalingDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Left Click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SoundMng.MMenu_OnMouseEnterWithoutSwoosh();
        }
        //Right Click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
        }
    }
}
