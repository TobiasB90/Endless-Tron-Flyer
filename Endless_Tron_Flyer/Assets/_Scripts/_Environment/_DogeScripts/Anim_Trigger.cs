using UnityEngine;

public class Anim_Trigger : MonoBehaviour {

    Animator anim;
    GameMng GMng;
    public ObjectInformation ObjectInformation_Object;
    IFaceMng_Limitless IFaceMng;
    public string AnimationName; 
    [Tooltip("Animation gets started as soon as there are 'x' Tunnels in between the Player and this Tunnel.")] public int TunnelDistanceToPlayer;
    private bool playedalready = false;

	// Use this for initialization
	void Start () {
        IFaceMng = GameObject.Find("_InterfaceManager").GetComponent<IFaceMng_Limitless>();
        anim = this.gameObject.GetComponent<Animator>();
        GMng = GameObject.Find("_GameManager").GetComponent<GameMng>();
        // ObjectInformation_Object = this.gameObject.transform.parent.parent.GetComponent<ObjectInformation>();
	}
	
	// Update is called once per frame
	void Update () {
		if (ObjectInformation_Object.TunnelNumber - GMng.TunnelSystemsSolved <= TunnelDistanceToPlayer && playedalready == false && IFaceMng.Playing)
        {
            anim.Play(AnimationName);
            playedalready = true;
        }
	}
}
