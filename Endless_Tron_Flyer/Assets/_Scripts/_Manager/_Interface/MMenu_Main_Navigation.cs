using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MMenu_Main_Navigation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public TMP_Text TextObject;
    public SoundManager SoundMng;
    public bool CameraLerpAnimation;
    public bool LerpImageColor;
    public float ColorLerpDuration;
    public bool MoveRight = true;
    public float moveRightAmount = 50;
    public float moveRightDuration;
    public Image UI_Image;
    Color startingColor;
    public Color LerpColor;
    public Color PressColor;

    void Start()
    {
        if(LerpImageColor) startingColor = UI_Image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (LerpImageColor)
        {
            DOTween.To(() => UI_Image.color, x => UI_Image.color = x, LerpColor, ColorLerpDuration).SetEase(Ease.Linear);
        }
        if (MoveRight) TextObject.rectTransform.DOLocalMoveX(moveRightAmount, moveRightDuration);
        

        SoundMng.MMenu_OnMouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (LerpImageColor) DOTween.To(() => UI_Image.color, x => UI_Image.color = x, startingColor, ColorLerpDuration);
        if (MoveRight) TextObject.rectTransform.DOLocalMoveX(0, moveRightDuration);
        if (LerpImageColor)
        {
            DOTween.To(() => UI_Image.color, x => UI_Image.color = x, startingColor, ColorLerpDuration).SetEase(Ease.Linear);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Left Click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CameraLerpAnimation) SoundMng.MMenu_OnMouseEnterWithSwoosh();
            else SoundMng.MMenu_OnMouseEnterWithoutSwoosh();
            if (MoveRight) TextObject.rectTransform.DOLocalMoveX(0, moveRightDuration);
            if (LerpImageColor)
            {
                Sequence ColorLerpSequence = DOTween.Sequence();
                ColorLerpSequence.Append(UI_Image.DOColor(PressColor, ColorLerpDuration).SetEase(Ease.Linear));
                ColorLerpSequence.Append(UI_Image.DOColor(LerpColor, ColorLerpDuration).SetEase(Ease.Linear));
                //DOTween.To(() => UI_Image.color, x => UI_Image.color = x, PressColor, ColorLerpDuration).SetEase(Ease.Linear);
            }


        }
        //Right Click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
        }
    }

}

