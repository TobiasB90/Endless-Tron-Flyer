using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IFaceMng : MonoBehaviour {

    public GameObject RetryButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RetryGame()
    {
        SceneManager.LoadScene("Level_01_Endless");
        RetryButton.SetActive(false);
    }
}
