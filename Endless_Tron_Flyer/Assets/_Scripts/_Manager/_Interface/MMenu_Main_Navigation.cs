using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MMenu_Main_Navigation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public TMP_Text TextObject;
    public SoundManager SoundMng;
    public bool CameraLerpAnimation;
    public bool MoveRight = true;
    public float moveRightAmount = 50;
    public float moveRightDuration;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MoveRight) TextObject.rectTransform.DOLocalMoveX(moveRightAmount, moveRightDuration);
        SoundMng.MMenu_OnMouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MoveRight) TextObject.rectTransform.DOLocalMoveX(0, moveRightDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Left Click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CameraLerpAnimation) SoundMng.MMenu_OnMouseEnterWithSwoosh();
            else SoundMng.MMenu_OnMouseEnterWithoutSwoosh();
            if (MoveRight) TextObject.rectTransform.DOLocalMoveX(0, moveRightDuration);
            
        }
        //Right Click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
        }
    }

}

