using UnityEngine;

public class GameMng : MonoBehaviour {

    private int _TunnelSystemsSolved;
    private float _Score;
    public int ScoreMultiplier;

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
		
	}
	
	// Update is called once per frame
	void Update () {
        Score = Time.time * ScoreMultiplier;
	}
}
