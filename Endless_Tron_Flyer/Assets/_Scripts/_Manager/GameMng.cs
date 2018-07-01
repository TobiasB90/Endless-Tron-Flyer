using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameMng : MonoBehaviour {

    private int _TunnelSystemsSolved;
    private float _Score;
    public int ScoreMultiplier;
    public float TimeAlive;
    public IFaceMng_Limitless IFaceMng;
    public float HighScore;
    public PlayerController pcont;
    public CameraController camcont;
    public TMP_Text Countdown_Text;
    private userManager usrMng;
    private SoundManager sndMng;
    public Camera MainCamera;
    public InterpolatedTransform MCam_Interpol;
    public InterpolatedTransformUpdater MCam_Interpol_Updater;
    public ObjectBuilder TunnelBuilder;

    public int TunnelSystemsSolved
    {
        get { return _TunnelSystemsSolved; }
        set { _TunnelSystemsSolved = value; }
    }

    public float Score
    {
        get { return _Score; }
        set { _Score = value; }
    }

    // Use this for initialization
    void Start () {
        sndMng = GameObject.Find("_SoundManager").GetComponent<SoundManager>();
        usrMng = GameObject.Find("_userManager").GetComponent<userManager>();
        pcont.enabled = false;
        camcont.enabled = false;
        Score = 0;
        StartUpSequence();
    }

    // Update is called once per frame
    void Update () {
        if (IFaceMng.Playing)
        {
            Score += Time.deltaTime * ScoreMultiplier;
            TimeAlive += Time.deltaTime;
        }
    }

    public void StartUpSequence()
    {
        if (usrMng.FirstGamesceneStart)
        {
            StartCoroutine(CameraSequence(1));
        }
        StartCoroutine(SequenceTimer(11));
    }

    public IEnumerator SequenceTimer(float SequenceTimer)
    {
        if (usrMng.FirstGamesceneStart)
        {
            yield return new WaitForSeconds(SequenceTimer - 2);
            usrMng.FirstGamesceneStart = false;
        }
        Sequence CD3 = DOTween.Sequence();
        Countdown_Text.text = "3";
        CD3.Append(Countdown_Text.gameObject.transform.DOScale(0.5f, 1f));
        CD3.Join(Countdown_Text.gameObject.transform.DOScale(0.5f, 1f));
        yield return new WaitForSeconds(0.1f);
        sndMng.AudioSource_01.PlayOneShot(sndMng.AudioClips_01[4]);
        yield return new WaitForSeconds(1.1f);
        Countdown_Text.text = "2";
        CD3.Append(Countdown_Text.gameObject.transform.DOScale(1f, 0f));
        CD3.Join(Countdown_Text.gameObject.transform.DOScale(0.5f, 1f));
        yield return new WaitForSeconds(1.1f);
        Countdown_Text.text = "1";
        CD3.Append(Countdown_Text.gameObject.transform.DOScale(1f, 0f));
        CD3.Join(Countdown_Text.gameObject.transform.DOScale(0.5f, 1f));
        yield return new WaitForSeconds(1.5f);
        Countdown_Text.text = "GO";
        CD3.Append(Countdown_Text.gameObject.transform.DOScale(1f, 0f));
        CD3.Append(Countdown_Text.DOFade(0f, 1f));
        MCam_Interpol.enabled = true;
        MCam_Interpol_Updater.enabled = true;
        pcont.enabled = true;
        camcont.enabled = true;
        IFaceMng.ScoreUI.SetActive(true);
        IFaceMng.Playing = true;
    }

    public IEnumerator CameraSequence(float CamSequenceTimer)
    {
        MainCamera.transform.eulerAngles = new Vector3(20, 25, 10);
        MainCamera.transform.position = new Vector3(-207, 2114, -120);
        Sequence Cam_01 = DOTween.Sequence();
        Cam_01.Append(MainCamera.transform.DOMove(new Vector3(-523, 2061, 7), 4f));
        yield return new WaitForSeconds(4);
        MainCamera.transform.position = new Vector3(92, 2020, 0);
        MainCamera.transform.eulerAngles = new Vector3(0, -32, 0);
        Sequence Cam_02 = DOTween.Sequence();
        Cam_02.Append(MainCamera.transform.DOMove(new Vector3(9, 2020, -53), 4f));
        yield return new WaitForSeconds(4);
        MainCamera.transform.position = new Vector3(0, 2019, -65);
        MainCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void TunnelBuilder_buildTunnel()
    {
        TunnelBuilder.InstantiateTunnel();
    }

    public void TunnelBuilder_destroyTunnel()
    {
        TunnelBuilder.DestroyTunnel();
    }
}
