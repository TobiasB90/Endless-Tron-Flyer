using UnityEngine;

public class GameMng : MonoBehaviour {

    private int _TunnelSystemsSolved;
    private float _Score;
    public int ScoreMultiplier;
    public float TimeAlive;
    public IFaceMng_Limitless IFaceMng;
    public float HighScore;

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
        HighScore = PlayerPrefs.GetFloat("HighScore");
        Score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (IFaceMng.Playing)
        {
            Score += Time.deltaTime * ScoreMultiplier;
            TimeAlive += Time.deltaTime;
        }
    }

    public void UpdateHighScore()
    {
        HighScore = PlayerPrefs.GetFloat("HighScore");
    }
}
