using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class IFaceMng_Limitless : MonoBehaviour {

    public GameObject PauseMenuUI;
    public Button PauseMenuUI_Button_Resume;
    public Button PauseMenuUI_Button_Retry;

    public GameObject ScoreScreenUI;
    public Button ScoreScreenUI_Button_Retry;
    public Button ScoreScreenUI_Button_MainMenu;

    public bool Playing = false;
    bool ScoreScreenActive = false;

    public GameMng gameMng;
    public TMP_Text scoreTxt;
    public TMP_Text tunnelsPassedTxt;
    public TMP_Text timeAliveTxt;
    public TMP_Text UIScore;
    public TMP_Text HighScoreUI;
    public GameObject ScoreUI;
    private userManager UserManager;
    public UploadHighscore upHScore;
    private SoundManager sndMng;
    float scr;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        UserManager = GameObject.Find("_userManager").GetComponent<userManager>();
        sndMng = GameObject.Find("_SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && Playing && !ScoreScreenActive)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Playing && !ScoreScreenActive) 
        {
            ResumeGame();
        }
        scr = Mathf.RoundToInt(gameMng.Score);
        UIScore.text = scr.ToString();
    }

    public void Awake()
    {
        sndMng = GameObject.Find("_SoundManager").GetComponent<SoundManager>();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        PauseMenuUI.SetActive(true);
        PauseMenuUI_Button_Retry.Select();
        PauseMenuUI_Button_Resume.Select();
        Playing = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        PauseMenuUI.SetActive(false);
        Playing = true;
    }

    public void ScoreScreen()
    {
        Cursor.visible = true;
        Playing = false;
        ScoreScreenActive = true;
        ScoreUI.SetActive(false);
        int curscore = Mathf.RoundToInt(gameMng.Score);
        if (UserManager.Username != "")
        {
            upHScore.UploadPlayerHighScore(Mathf.RoundToInt(gameMng.Score));
            upHScore.GetPersonalHighscore();
            if (curscore > upHScore.PersonalScore.Score) HighScoreUI.text = curscore.ToString();
            else HighScoreUI.text = upHScore.PersonalScore.Score.ToString();
        }
        else
        {
            if (curscore > PlayerPrefs.GetInt("Highscore", 0))
            {
                PlayerPrefs.SetInt("Highscore", curscore);
                HighScoreUI.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
            }
            else HighScoreUI.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        }
        scoreTxt.text = curscore.ToString();
        tunnelsPassedTxt.text = gameMng.TunnelSystemsSolved.ToString();
        if(gameMng.TimeAlive >= 60)
        {
            float minutes = Mathf.Floor(gameMng.TimeAlive / 60);
            float seconds = Mathf.RoundToInt(gameMng.TimeAlive % 60);
            timeAliveTxt.text = minutes.ToString() + " MIN " + seconds.ToString() + " SEC";
        }
        else
        {
            float timeAlive = Mathf.RoundToInt(gameMng.TimeAlive % 60);
            timeAliveTxt.text = timeAlive.ToString() + " SEC";
        }
        ScoreScreenUI.SetActive(true);
        ScoreScreenUI_Button_Retry.Select();
    }

    public void RetryGame()
    {
        Cursor.visible = false;
        ScoreScreenActive = false;
        ScoreScreenUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_01_Endless");
    }

    public void MainMenu()
    {
        UserManager.FirstGamesceneStart = true;
        Cursor.visible = true;
        sndMng.GameScene = false;
        sndMng.playingmusic = false;
        ScoreScreenUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu_01");
    }
}
