using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IFaceMng_Limitless : MonoBehaviour {

    public GameObject PauseMenuUI;
    public GameObject ScoreScreenUI;
    public bool Playing = true;
    public GameMng gameMng;
    public TMP_Text scoreTxt;
    public TMP_Text tunnelsPassedTxt;
    public TMP_Text timeAliveTxt;
    public TMP_Text UIScore;
    public TMP_Text HighScoreUI;
    public GameObject ScoreUI;
    private userManager UserManager;
    public UploadHighscore upHScore;
    public SoundManager sndMng;
    float scr;

    // Use this for initialization
    void Start () {
        ScoreUI.SetActive(true);
        UserManager = GameObject.Find("_userManager").GetComponent<userManager>();
        sndMng = GameObject.Find("_SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && Playing)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Playing)
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
        PauseMenuUI.SetActive(true);
        Playing = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenuUI.SetActive(false);
        Playing = true;
    }

    public void ScoreScreen()
    {        
        Playing = false;
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
    }

    public void RetryGame()
    {
        ScoreScreenUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Level_01_Endless");
    }

    public void MainMenu()
    {
        sndMng.GameScene = false;
        sndMng.playingmusic = false;
        ScoreScreenUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        SceneManager.LoadScene("MainMenu_01");
    }
}
