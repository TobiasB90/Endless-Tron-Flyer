using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.PostProcessing;
using UnityEngine.PostProcessing.Utilities;


public class IFaceMng : MonoBehaviour {

    public GameObject RetryButton;
    public GameObject MainMenu_Obj;
    public GameObject MainMenu_Play_Obj;
    public GameObject MainMenu_HighScore_Obj;
    public GameObject MainMenu_Achievements_Obj;
    public GameObject MainMenu_Skins_Obj;
    public GameObject MainMenu_Options_Obj;
    public GameObject MainMenu_BackButton_Obj;
    public GameObject MainCameraParent;
    public float CameraRotationSpeed;
    public PostProcessingController PPController;
    public bool Blurred = false;
    public bool BlurringNow = false;
    Vector3 MainCameraBasePosition;
    Vector3 MainCameraBaseRotation;
    public SoundManager SoundMng;
    public bool BackButtonAvailable = false;
    public GameObject CirclingSphere1;
    public GameObject CirclingSphere2;
    public GameObject Player;

    // Use this for initialization
    void Start () {
        MainMenu_RotateCamera();
        MainMenu_Circling_Spheres();
        MainMenu_PlayerModel_Hover();
        PPController = Camera.main.GetComponent<PostProcessingController>();
        MainCameraBasePosition = Camera.main.transform.position;
        MainCameraBaseRotation = Camera.main.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Escape) && BackButtonAvailable)
        {
            MainMenu_BackButton();
        }
	}

    private void MainMenu_Circling_Spheres()
    {
        Sequence CirclingSphere_01 = DOTween.Sequence();
        CirclingSphere_01.SetId("CirclingSpheres");
        CirclingSphere_01.Append(CirclingSphere1.transform.DORotate(new Vector3(0, 360, 0), 5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        CirclingSphere_01.Append(CirclingSphere1.transform.DORotate(new Vector3(0, 0, 0), 0f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        CirclingSphere_01.SetLoops(-1, LoopType.Restart);

        Sequence CirclingSphere_02 = DOTween.Sequence();
        CirclingSphere_02.SetId("CirclingSpheres");
        CirclingSphere_02.Append(CirclingSphere2.transform.DORotate(new Vector3(0, -360, 0), 5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        CirclingSphere_02.Append(CirclingSphere2.transform.DORotate(new Vector3(0, 0, 0), 0f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        CirclingSphere_02.SetLoops(-1, LoopType.Restart);
    }

    private void MainMenu_PlayerModel_Hover()
    {
        Sequence PlayerModel_Hover = DOTween.Sequence();
        PlayerModel_Hover.SetId("HoveringPlayerModel");
        PlayerModel_Hover.Append(Player.transform.DOMoveY(17.2f, 1f).SetEase(Ease.OutSine));
        PlayerModel_Hover.Append(Player.transform.DOMoveY(17f, 1f).SetEase(Ease.InOutSine));
        PlayerModel_Hover.SetLoops(-1, LoopType.Restart);
    }
    public void RetryGame()
    {
        SceneManager.LoadScene("Level_01_Endless");
        RetryButton.SetActive(false);
    }

    public void EnterEndlessFlyerScene()
    {
        SceneManager.LoadScene("Level_01_Endless");
    }

    public void EnterMainMenu()
    {

    }

    public void MainMenu_Play()
    {
        if (!BlurringNow)
        {
            MainMenu_Obj.SetActive(false);
            MainMenu_Play_Obj.SetActive(true);
            BackButtonAvailable = true;
            MainMenu_BackButton_Obj.SetActive(true);
        }
    }

    public void MainMenu_HighScore()
    {
        if (!BlurringNow)
        {
            MainMenu_Obj.SetActive(false);
            MainMenu_HighScore_Obj.SetActive(true);
            BackButtonAvailable = true;
            MainMenu_BackButton_Obj.SetActive(true);
        }
    }

    public void MainMenu_Achievements()
    {
        if (!BlurringNow)
        {
            MainMenu_Obj.SetActive(false);
            MainMenu_Achievements_Obj.SetActive(true);
            BackButtonAvailable = true;
            MainMenu_BackButton_Obj.SetActive(true);
        }  
    }

    public void MainMenu_Skins()
    {
        if (!BlurringNow)
        {
            MainMenu_Obj.SetActive(false);
            MainMenu_Skins_Obj.SetActive(true);
            BackButtonAvailable = true;
            MainMenu_BackButton_Obj.SetActive(true);
        }   
    }

    public void MainMenu_Options()
    {
        if (!BlurringNow)
        {
            MainMenu_Obj.SetActive(false);
            MainMenu_Options_Obj.SetActive(true);
            BackButtonAvailable = true;
            MainMenu_BackButton_Obj.SetActive(true);
        }
    }

    public void MainMenu_ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu_BackButton()
    {
        if (BlurringNow == false)
        {
            MainMenu_Obj.SetActive(true);
            MainMenu_BackButton_Obj.SetActive(false);
            MainMenu_Play_Obj.SetActive(false);
            MainMenu_HighScore_Obj.SetActive(false);
            MainMenu_Achievements_Obj.SetActive(false);
            MainMenu_Skins_Obj.SetActive(false);
            MainMenu_Options_Obj.SetActive(false);
        }

        if (Blurred && BlurringNow == false)
        {
            SoundMng.MMenu_ReverseSwoosh();
            StartCoroutine(LerpBlur(0.5f));
        }
    }

    private void MainMenu_RotateCamera()
    {
        Sequence RotatingCamera = DOTween.Sequence();
        RotatingCamera.SetId("RotatingCamera");
        RotatingCamera.Append(MainCameraParent.transform.DORotate(new Vector3(0, 360, 0), CameraRotationSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
    }

    public void LerpBlur()
    {
        if (!BlurringNow)
        {
            StartCoroutine(LerpBlur(0.5f));
        }
    }

    IEnumerator LerpBlur(float waittime)
    {
        if (!Blurred && BlurringNow == false)
        {
            BlurringNow = true;
            DOTween.Pause("CirclingSpheres");
            DOTween.Pause("RotatingCamera");
            DOTween.Pause("HoveringPlayerModel");
            Sequence Blurring = DOTween.Sequence();
            Blurring.Append(DOTween.To(() => PPController.depthOfField.focusDistance, x => PPController.depthOfField.focusDistance = x, 0.1f, 0.5f));
            Blurring.Join(Camera.main.transform.DOLocalMove(new Vector3(3, 5, -3), 0.5f));
            Blurring.Join(Camera.main.transform.DOLocalRotate(new Vector3(30, -45, -30), 0.5f));
            Blurred = true;
        }
        else if (Blurred && BlurringNow == false)
        {
            BlurringNow = true;
            DOTween.Play("CirclingSpheres");
            DOTween.Play("HoveringPlayerModel");
            DOTween.Play("RotatingCamera");
            Sequence ReverseBlurring = DOTween.Sequence();
            ReverseBlurring.Append(DOTween.To(() => PPController.depthOfField.focusDistance, x => PPController.depthOfField.focusDistance = x, 10, 0.3f));
            ReverseBlurring.Join(Camera.main.transform.DOLocalMove(MainCameraBasePosition, 0.5f));
            ReverseBlurring.Join(Camera.main.transform.DOLocalRotate(new Vector3(10, 0, 0), 0.5f));
            Blurred = false;
        }
        yield return new WaitForSeconds(waittime);
        BlurringNow = false;
    }
}
