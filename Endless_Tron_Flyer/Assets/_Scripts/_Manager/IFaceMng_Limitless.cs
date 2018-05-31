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
    float scr;

    // Use this for initialization
    void Start () {
        ScoreUI.SetActive(true);
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
        float roundedScore = Mathf.Round(gameMng.Score);
        if (roundedScore > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", roundedScore);
            gameMng.UpdateHighScore();
        }
        float scr2 = Mathf.RoundToInt(PlayerPrefs.GetFloat("HighScore"));
        HighScoreUI.text = scr2.ToString();
        scoreTxt.text = roundedScore.ToString();
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
        ScoreScreenUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        SceneManager.LoadScene("MainMenu_01");
    }
}
