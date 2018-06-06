using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debug_Direction : MonoBehaviour {

    TMP_Text Text;
    public GameObject Player;
    PlayerController pcont;
	// Use this for initialization
	void Start () {
        Text = GetComponent<TMP_Text>();
        pcont = Player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        Text.text = pcont.TunnelDir.ToString();
	}
}
