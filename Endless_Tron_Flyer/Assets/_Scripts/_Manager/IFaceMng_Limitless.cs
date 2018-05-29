using System.Collections;
using System.Collections.Generic;
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
    public GameObject NewHighScoreUI;

    // Use this for initialization
    void Start () {
		
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
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        Playing = false;
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        Playing = true;
    }

    public void ScoreScreen()
    {
        Playing = false;

        float roundedScore = Mathf.Round(gameMng.Score);
        if (roundedScore > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", roundedScore);
            gameMng.UpdateHighScore();
        }
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
