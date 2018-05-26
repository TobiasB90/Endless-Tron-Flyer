using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_DE_Pflock : MonoBehaviour {

    Animator anim;
    GameMng GMng;
    ObjectInformation ObjInfo;
    [Tooltip("Animation gets started as soon as there are 'x' Tunnels in between the Player and this Tunnel.")] public int TunnelDistanceToPlayer;
    private bool playedalready = false;

	// Use this for initialization
	void Start () {
        anim = this.gameObject.GetComponent<Animator>();
        GMng = GameObject.Find("_GameManager").GetComponent<GameMng>();
        ObjInfo = this.gameObject.transform.parent.parent.GetComponent<ObjectInformation>();
	}
	
	// Update is called once per frame
	void Update () {
		if (ObjInfo.TunnelNumber - GMng.TunnelSystemsSolved == TunnelDistanceToPlayer && playedalready == false)
        {
            anim.Play("Pflock");
            playedalready = true;
        }
	}
}
